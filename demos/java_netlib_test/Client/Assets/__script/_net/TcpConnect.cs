using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// tcp 连接类
/// 对 socket 的一个简单应用封装
/// 具象化为:   1. TCP 连接
///             2. 阻塞模式 执行网络连接和消息接收; 异步模式 进行消息发送;  理由是阻塞模式下消息接收(分成了两步, 收消息长度,消息体), 代码更简洁, 易于理解;
///             3. 恒定消息格式为  [msgSize--4字节,msg]
///             4. 消息立即发送; SocketOptionName.NoDelay
/// </summary>
public class TcpConnect
{
    #region Const
    // 消息长度信息占位大小 msgSize--4字节
    private const int MSG_RECEIVE_SIZE_LENGTH = 4;
    #endregion

    #region Param
    // Socket 连接对象;
    private Socket _socket = null;
    // 是否延迟(合并消息包)发送;
    private bool _noDelay = false;
    //当前网络状态
    private int _curNetState = (int)NetState.Disconnected;

    // 消息接收缓存;
    private byte[] _receiveSizeBuffer = new byte[MSG_RECEIVE_SIZE_LENGTH];
    private byte[] _receiveBuffer = new byte[NetDefine.MAX_RECEIVE_BUFFER_LENGTH];
    private MemoryStream _receiveStream;
    // 消息发送缓存;
    byte[] _sendBuffer = new byte[NetDefine.MAX_SEND_BUFFER_LENGTH];
    MemoryStream _sendStream;
    #endregion


    #region Param interface
    public NetState CurNetState
    {
        get { return (NetState)_curNetState; }
        private set
        {
            Interlocked.Exchange(ref _curNetState, (int)value);
            Log.Loggers.net.Debug("TcpConnect.CurNetState: " + (NetState)_curNetState);
        }
    }

    /// <summary>
    /// NoDelay, 为发送合并禁用 Nagle 算法。是否禁用Nagle 算法
    /// 禁用后 效果是直接发送消息, 不合并消息包;
    /// Nagle 算法介绍: http://blog.csdn.net/ithzhang/article/details/8520026
    /// </summary>
    private bool NoDelay
    {
        get
        {
            return _noDelay;
        }
        set
        {
            if (_noDelay != value)
            {
                _noDelay = value;
#if !UNITY_WINRT
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, _noDelay);
#endif
            }
        }
    }
    #endregion




    #region public Function
    public TcpConnect()
    {
        _sendStream = new MemoryStream(_sendBuffer);
        _receiveStream = new MemoryStream(_receiveBuffer);
    }
    ~TcpConnect()
    {
        CloseConnect();
    }

        
    /// <summary>
    /// 建立连接; 阻塞式
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public void StartConnect(string host, int port)
    {
        if (CurNetState == NetState.Connecting || CurNetState == NetState.Connected)
        {
            Log.Loggers.net.Warning("Tcp is already connected");
            return;
        }
        if (CurNetState == NetState.Droped)
        {
            // 清理掉原有连接, 以便重连;
            CloseConnect();
        }

        CurNetState = NetState.Connecting;

        // 建立网络连接;
        try
        {
            // 初始化套接字;
            var ipAddresses = Dns.GetHostAddresses(host);

            if (ipAddresses[0].AddressFamily == AddressFamily.InterNetwork)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (ipAddresses[0].AddressFamily == AddressFamily.InterNetworkV6)
            {
                _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            }

            // 开启 Nagle 算法;
            NoDelay = false;

            // 建立连接;
            _socket.Connect(ipAddresses, port);
            CurNetState = NetState.Connected;

            Log.Loggers.net.Debug("成功连接服务器:" + host + "  端口:" + port);
        }
        catch (System.Exception ex)
        {
            NetDrop(ex.Message);
        }
    }

    public void CloseConnect()
    {
        try
        {
            if (_socket != null
                && CurNetState != NetState.Disconnected)
            {
                CurNetState = NetState.Disconnected;

                if (_socket.Connected) _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
        }
        catch (System.Exception ex)
        {
            NetDrop(ex.Message);
        }
    }

    /// <summary>
    /// 阻塞式接收一条消息;
    /// 返回Stream, 在下一次调用Receive接口时. 该值内容将会被覆盖为新的消息;
    /// </summary>
    /// <returns></returns>
    public Stream ReceiveMessage()
    {
        if (CurNetState != NetState.Connected)
            return null;

        // 获取消息长度;
        int headReadSize = 0;
        while (headReadSize < MSG_RECEIVE_SIZE_LENGTH)
        {
            try
            {
                headReadSize += _socket.Receive(_receiveSizeBuffer, headReadSize, MSG_RECEIVE_SIZE_LENGTH - headReadSize, SocketFlags.None);
            } catch (Exception e)
            {
                NetDrop("Receive message failed: " + e.ToString());
                return null;
            }
        }
        int msgSize = SerializeUtils.ReadInt(_receiveSizeBuffer);
        if (msgSize <= 0 || msgSize >= NetDefine.MAX_RECEIVE_BUFFER_LENGTH)
        {
            // 消息长度错误;
            NetDrop(string.Format("Invalid Message size: {0}!", msgSize));
            return null;
        }

        // 获取消息;
        int bodyReadSize = 0;
        while (bodyReadSize < msgSize)
        {
            bodyReadSize += _socket.Receive(_receiveBuffer, bodyReadSize, NetDefine.MAX_RECEIVE_BUFFER_LENGTH - bodyReadSize, SocketFlags.None);
        }
        _receiveStream.Position = 0;
        _receiveStream.SetLength(bodyReadSize);

        return _receiveStream;
    }
    /// <summary>
    /// 异步模式发送消息;  TODO, 不应该写在 此处;
    /// </summary>
    public void SendMessage(Message message)
    {
        // 重置发送缓存;
        _sendStream.SetLength(0);
        _sendStream.Position = 0;

        // 占位;
        SerializeUtils.WriteInt(_sendStream, 0);        // 消息长度

        // 写入消息ID;
        SerializeUtils.WriteInt(_sendStream, message.MsgId);
        // 填充信息;
        SerializeUtils.WriteBean(_sendStream, message);

        // 消息长度, 除去存储长度本身的数据外, 剩余数据所占长度;
        int msgLength = (int)_sendStream.Length - sizeof(int);
        _sendStream.Position = 0;
        SerializeUtils.WriteInt(_sendStream, msgLength);

        // 发起发送;
        var data = _sendStream.ToArray();
        var dataLength = data.Length;
        SendMessage(data, dataLength);
    }
    #endregion


    #region private Function
    private void NetDrop(string error = "")
    {
        CurNetState = NetState.Droped;
        Log.Loggers.net.Warning(string.Format("Network dropped, error: {0}", error));
    }

    private void SendMessage(byte[] data, int length)
    {
        if (CurNetState != NetState.Connected)
            return;

        try
        {
            _socket.BeginSend(data, 0, length, SocketFlags.None, OnSend, null);
        }
        catch (System.Exception e)
        {
            NetDrop(e.Message);
        }
    }
    private void OnSend(IAsyncResult result)
    {
        if (CurNetState != NetState.Connected)
            return;

        var bytes = 0;
        try
        {
            bytes = _socket.EndSend(result);
            if (bytes == 0)
            {
                NetDrop("Failed to send msg.");
            }

            Log.Loggers.net.Debug("Finish send msg");
        }
        catch (System.Exception e)
        {
            NetDrop(e.Message);
        }
    }
    #endregion
}
