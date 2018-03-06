using System;
using System.Collections.Generic;

/// <summary>
/// 消息通信 管理器; 主要负责 消息ID, 消息协议, 消息处理器 的注册和获取;
/// </summary>
public class ProtocolManager
{
    #region Define
    private struct MessageHandler
    {
        public Type type;
        public MessageEvent msgEvent;

        public MessageHandler(Type type, MessageEvent msgEvent)
        {
            this.type = type;
            this.msgEvent = msgEvent;
        }
    }
    #endregion

    #region Param
    private Dictionary<int, MessageHandler> _protocolDictionary = new Dictionary<int, MessageHandler>();
    #endregion



    #region public Function
    public void Regist(int msgId, Type msgType, MessageEvent msgEvent)
    {
        if (_protocolDictionary.ContainsKey(msgId))
        {
            Log.Loggers.net.Debug(string.Format("Duplicate regist msgId {0}", msgId));
            return;
        }
        _protocolDictionary.Add(msgId, new MessageHandler(msgType, msgEvent));
    }

    public MessageEvent GetMessageEvent(int msgId)
    {
        var messageHandler = GetMessageHandler(msgId);
        if (messageHandler.type == null)
        {
            return null;
        }

        return messageHandler.msgEvent;
    }


    public Message GetMessage(int msgId)
    {
        var messageHandler = GetMessageHandler(msgId);
        if (messageHandler.type == null)
        {
            return null;
        }
        Message msg = Activator.CreateInstance(messageHandler.type) as Message;
        msg.MsgId = msgId;
        return msg;
    }
    #endregion


    #region private Function
    MessageHandler GetMessageHandler(int msgId)
    {
        MessageHandler messageHandler;
            
        if (!_protocolDictionary.TryGetValue(msgId, out messageHandler))
        {
            Log.Loggers.net.Warning(string.Format("Protocol {0} not regist", msgId));
            return new MessageHandler(null, null);
        }

        return messageHandler;
    }
    #endregion
}