package com.zyyApp.Netty.ServerTest.Logic;

import com.google.protobuf.MessageLite;
import com.zyyApp.Netty.ServerTest.Message.MsgHanderBase;
import com.zyyApp.Netty.ServerTest.Message.protocol.AddressBookProtos;
import com.zyyApp.Netty.ServerTest.Player.struct.Player;
import com.zyyApp.netlib.protocol.Msg.Head.HeadType;
import com.zyyApp.netlib.protocol.Msg.Message;
import com.zyyApp.netlib.server.Netty.NettyServer;
import com.zyyApp.netlib.server.TcpServerBase;
import io.netty.util.ResourceLeakDetector;


/**
 * Created by zyy on 2017/4/13.
 */
public class MyServer extends TcpServerBase{

    MyServer() {
        super(new NettyServer(), HeadType.HEAD_ID_ONLY, HeadType.HEAD_ID_ONLY);
        ResourceLeakDetector.setLevel(ResourceLeakDetector.Level.ADVANCED);
    }

    /** 新连接建立回调; 供子类回调使用*/
    @Override
    public void onConnect(Long connectId) {
        Player player = new Player();
        player.setConnectId(connectId);
        ManagerPool.getInstance().playerMgr.addPlayer(player);
        System.out.println(String.format("onConnect: %d", connectId));

        // test.
        AddressBookProtos.Person.Builder oBui = AddressBookProtos.Person.newBuilder();
        oBui.setName("张逸云");
        oBui.setId(33);
        oBui.setEmail("zyy@email.com");

        AddressBookProtos.Person.PhoneNumber.Builder pnBui = AddressBookProtos.Person.PhoneNumber.newBuilder();
        pnBui.setNumber("123");
        pnBui.setType(AddressBookProtos.Person.PhoneType.HOME);
        oBui.addPhones(pnBui);

        AddressBookProtos.Person.PhoneNumber.Builder pnBui_2 = AddressBookProtos.Person.PhoneNumber.newBuilder();
        pnBui_2.setNumber("asd3");
        pnBui_2.setType(AddressBookProtos.Person.PhoneType.WORK);
        oBui.addPhones(pnBui_2);

//        Login.reslogin.Builder lgBui = Login.reslogin.newBuilder();
//        lgBui.setPlayerId(1354);

        ManagerPool.getInstance().myServer.sendMsg(connectId, oBui.build());
    }

    /** 连接断开回调; 供子类回调使用*/
    @Override
    public void onDisConnect(Long connectId) {
        ManagerPool.getInstance().playerMgr.removePlayerByConnId(connectId);
        System.out.println(String.format("onDisConnect: %d", connectId));
    }

    /** 收到消息回调; */
    @Override
    public void onReceive(Long connectId, Message msg) {
        // 依据 msgId, 获得对应的 处理器;
        MsgHanderBase hander = ManagerPool.getInstance().handerPool.getMessageHander(msg.getMsgId());

        // 执行处理逻辑
        if (hander != null) {
            hander.action(connectId, msg);
        }
    }


    public void sendMsg(Long connectId, MessageLite msgObj) {
        Message resMsg = Message.poolPop();
        if (resMsg.init(msgObj)) {
            writeAndFlush(connectId, resMsg);
        }
    }
}
