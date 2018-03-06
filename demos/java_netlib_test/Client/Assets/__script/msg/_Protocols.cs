/// <summary>
/// 消息协议集中声明管理;
/// </summary>
public class Protocols
{
    // 消息通信 管理器;
    private ProtocolManager _proMgr = new ProtocolManager();

    // 消息处理事件声明;
    public MessageEvent OnRes_Person = new MessageEvent();

    public Protocols()
    {
        RegistCS();
        RegistSC();
    }

    public MessageEvent GetMessageEvent(int msgId)
    {
        return _proMgr.GetMessageEvent(msgId);
    }

    public Message GetMessage(int msgId)
    {
        return _proMgr.GetMessage(msgId);
    }

    private void RegistCS()
    {
        // Client to Server  消息注册
        _proMgr.Regist(MessageID_Define.CG_Person, typeof(ProtoMessage<tutorial.reqPerson>), null);
    }

    private void RegistSC()
    {
        // Server to Client  消息注册
        _proMgr.Regist(MessageID_Define.GC_Person, typeof(ProtoMessage<tutorial.Person>), OnRes_Person);
    }
}
