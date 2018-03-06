package com.zyyApp.Netty.ServerTest.Login.handdler;

import com.zyyApp.Netty.ServerTest.Logic.ManagerPool;
import com.zyyApp.Netty.ServerTest.Message.MsgHanderBase;
import com.zyyApp.Netty.ServerTest.Message.protocol.Login;
import com.zyyApp.Netty.ServerTest.Player.struct.Player;
import com.zyyApp.netlib.protocol.Msg.Message;

/**
 * Created by zyy on 2017/4/14.
 */
public class ReqQuitHander extends MsgHanderBase{
    @Override
    public void action(Long connId, Message msg) {
        Player player = ManagerPool.getInstance().playerMgr.getPlayerByConnId(connId);
        //Login.reqQuit reqMsgObj = (Login.reqQuit)msg.getMsgObj();

        System.out.println(String.format("get Quit: {%d, %d}", player.getConnectId(), player.getPlayerId()));

        // 返回消息;
        Login.resQuit.Builder oBui = Login.resQuit.newBuilder();
        oBui.setPlayerId(player.getPlayerId());
        ManagerPool.getInstance().myServer.sendMsg(connId, oBui.build());

        // 关闭连接;
        ManagerPool.getInstance().myServer.closeClient(connId);
    }
}
