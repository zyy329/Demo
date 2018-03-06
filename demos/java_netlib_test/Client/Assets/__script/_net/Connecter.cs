using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
/// <summary>
/// 网络连接器;
/// 统一对应用层提供网络的操作接口
/// 处理连接的线程问题
/// 统一处理消息的编码, 解析, 和触发回调;
/// 处理断线重连问题;
/// </summary>
public class Connecter
{
    #region Const
    private enum E_AskSign
    {
        None = 0,           // 无请求;
        AskStartNet,        // 请求开始网络;
        AskStopNet,         // 请求结束网络;
    }
    #endregion

    #region Param
    // 状态标记;
    private bool _askStartNet = false;
    private bool _isRunning = false;

    // 网络线程;
    private Thread _netThread = null;
    private bool _stopNetThread = false;

    // 待处理接受消息列表;
    private Queue<Message> _protocolList = new Queue<Message>();
    private Queue<Message> _tempProtocolList = new Queue<Message>();
    private const int PROCESS_NUM_PERFRAME = 20;        // 每一帧最大处理消息数量;

    // tcp 连接;
    private TcpConnect _tc = null;
    private string _host = "";
    private int _port = 0;

    // 消息协议处理;
    public Protocols _pros = new Protocols();
    #endregion


    #region public Function
    public Connecter()
    {
        _netThread = new Thread(NetThreadWork);
        _netThread.Start();
    }
    ~Connecter()
    {
        Log.Loggers.net.Debug("~Connecter");

        // 结束网络线程;
        _stopNetThread = true;
    }

    // 请求开启网络
    public void AskStartNetWork(string host, int port)
    {
        // 记录连接地址,端口;
        _host = host;
        _port = port;

        _askStartNet = true;
    }

    // 请求关闭网络;
    // 结束前, 必须显示调用该函数; 否则可能会被阻塞在网络消息接收中, 卡死; (无法放在析构函数中, 结束阻塞后才会执行到析构)
    public void StopNetWork()
    {
        Log.Loggers.net.Debug("StopNetWork");
        _isRunning = false;

        // 断开原有连接;
        if (_tc != null)
        {
            _tc.CloseConnect();
            _tc = null;
        }
    }


    /// <summary>
    /// 循环处理函数; 执行消息处理的回调
    /// 用于确保消息回调流程和应用层逻辑 处于同一线程中, 避免多线程问题;
    /// 注意调用该函数的线程; 消息处理回调就是用的该线程;
    /// </summary>
    public void ProcessProtocol(bool processAll = false)
    {
        // 从网络线程缓存中取出待处理的消息;
        lock (_protocolList)
        {
            while (_protocolList.Count > 0)
            {
                _tempProtocolList.Enqueue(_protocolList.Dequeue());
            }
        }

        // 计算本帧处理消息数量;
        int leftCount;
        if (processAll)
        {
            leftCount = _tempProtocolList.Count;
        }
        else
        {
            leftCount = Mathf.Min(PROCESS_NUM_PERFRAME, _tempProtocolList.Count);
        }

        // 消息处理;
        while (leftCount > 0)
        {
            var msg = _tempProtocolList.Dequeue();
            var msgEvent = _pros.GetMessageEvent(msg.MsgId);
            if (msgEvent != null)
            {
                msgEvent.Dispatch(msg);
            }

            --leftCount;
        }
    }

    public void SendMsg(Message msg)
    {
        _tc.SendMessage(msg);
    }
    #endregion


    #region NetWork 网络线程;
    private void NetThreadWork()
    {
        while (!_stopNetThread)
        {
            if (!_isRunning)
            {
                if (_askStartNet)
                {
                    // 请求开始网络;
                    StartNetWork();
                    continue;
                }
                else
                {
                    // 休眠等待开启指令;
                    Thread.Sleep(10);
                }
            }
            else
            {
                _askStartNet = false;        // 已经处于开启状态, 开启请求置空

                // 网络连接并没有正真建立成功, 重新进行建立;
                if (_tc == null || _tc.CurNetState == NetState.Disconnected)
                {
                    StartNetWork();
                    continue;
                }

                // 断线, 执行断线重连;
                if (_tc.CurNetState == NetState.Droped)
                {
                    _tc.StartConnect(_host, _port);
                    continue;
                }

                // 执行消息的接收,解析,处理;
                ReceiveMessage();
            }
        }

        Log.Loggers.net.Info("Net thread stoped!");
    }

    private void StartNetWork()
    {
        Log.Loggers.net.Debug("StartNetWork");
        _isRunning = true;

        // 初始化连接对象;
        _tc = new TcpConnect();
        _tc.StartConnect(_host, _port);
    }
    private void ReceiveMessage()
    {
        // 阻塞式接收消息;
        Stream msgStream = _tc.ReceiveMessage();
        if (msgStream == null)
        {
            return;
        }

        // 消息ID;
        int msgID = SerializeUtils.ReadInt(msgStream);


        Message msg = _pros.GetMessage(msgID);
        if (msg != null)
        {
            // 消息解析;
            msg.Deserialize(msgStream);

            // 加入待处理列表;
            lock (_protocolList)
            {
                _protocolList.Enqueue(msg);
            }
        }
    }
    #endregion

}
