package com.zyyApp.Netty.ServerTest.Message;

import com.google.protobuf.MessageLite;
import com.zyyApp.Netty.ServerTest.Logic.ManagerPool;
import com.zyyApp.Netty.ServerTest.Login.handdler.ReqLoginHander;
import com.zyyApp.Netty.ServerTest.Login.handdler.ReqPerson;
import com.zyyApp.Netty.ServerTest.Login.handdler.ReqQuitHander;
import com.zyyApp.Netty.ServerTest.Message.protocol.AddressBookProtos;
import com.zyyApp.Netty.ServerTest.Message.protocol.Login;
import com.zyyApp.netlib.protocol.Msg.MessagePool;
import com.zyyApp.netlib.protocol.Msg.MsgTrans.MsgTransPool;
import com.zyyApp.netlib.protocol.Msg.MsgTrans.MsgTrans_ProtocolBuf;


/**
 * 消息管理器;
 * Created by zyy on 2017/5/3.
 */
public class MessageManager {
    public void init() {
        // Client to Server  消息
        register(MessageID_Define.CG_Login, Login.reqlogin.getDefaultInstance(), new ReqLoginHander());
        register(MessageID_Define.CG_Quit, Login.reqQuit.getDefaultInstance(), new ReqQuitHander());
        register(MessageID_Define.CG_Person, AddressBookProtos.reqPerson.getDefaultInstance(), new ReqPerson());

        // Server to Client  消息
        register(MessageID_Define.GC_Login, Login.reslogin.getDefaultInstance(), null);
        register(MessageID_Define.GC_Quit, Login.resQuit.getDefaultInstance(), null);
        register(MessageID_Define.GC_Person, AddressBookProtos.Person.getDefaultInstance(), null);
    }

    private void register(int msgId, MessageLite msgPrototype, MsgHanderBase hander) {
        // 注册消息;
        MessagePool.getInstance().register(msgId, msgPrototype.getClass());
        // 注册消息转换器;
        MsgTransPool.getInstance().register(msgId, new MsgTrans_ProtocolBuf<>(msgPrototype));

        // 注册处理函数;
        if (hander != null) {
            ManagerPool.getInstance().handerPool.register(msgId, hander);
        }
    }
}
