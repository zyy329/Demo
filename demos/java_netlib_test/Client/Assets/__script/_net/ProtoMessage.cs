using System;
using System.IO;

/// <summary>
/// protobuf 消息通信对象封装;
/// </summary>
/// <typeparam name="T"></typeparam>
public class ProtoMessage<T> : Message
    where T : global::ProtoBuf.IExtensible, new()
{
    // 消息对象;
    private T _msgObj = new T();

    #region Param interface
    public T MsgObj
    {
        get
        {
            return _msgObj;
        }

        set
        {
            _msgObj = value;
        }
    }
    #endregion

    public override void Serialize(Stream stream)
    {
        // 序列化消息对象到stream,  依据 SerializeUtils.WriteBytes 的写入规则;
        long bodyBeginPos = stream.Position;
        SerializeUtils.WriteInt(stream, 0);                 // _msgObj 长度占位;
        ProtoBuf.Serializer.Serialize<T>(stream, _msgObj);  // _msgObj 内容;
        long bodyEndPos = stream.Position;                  // 记录结束时的 Position

        // 写入 _msgObj 长度信息;
        stream.Position = bodyBeginPos;
        SerializeUtils.WriteInt(stream, (int)(bodyEndPos - bodyBeginPos - sizeof(int)));
        stream.Position = bodyEndPos;                       // 还原 Position;
    }
    public override void Deserialize(Stream stream)
    {
        // 反序列化对象;
        byte[] bs = SerializeUtils.ReadBytes(stream);
        _msgObj = ProtoBuf.Serializer.Deserialize<T>(new MemoryStream(bs));
    }
}