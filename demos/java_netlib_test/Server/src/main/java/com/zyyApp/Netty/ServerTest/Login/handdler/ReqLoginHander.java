package com.zyyApp.Netty.ServerTest.Login.handdler;

import com.zyyApp.Netty.ServerTest.Logic.ManagerPool;
import com.zyyApp.Netty.ServerTest.Message.MsgHanderBase;
import com.zyyApp.Netty.ServerTest.Message.protocol.Login;
import com.zyyApp.Netty.ServerTest.Player.struct.Player;
import com.zyyApp.netlib.protocol.Msg.Message;

/**
 * Created by zyy on 2017/4/14.
 */
public class ReqLoginHander extends MsgHanderBase{
    @Override
    public void action(Long connId, Message msg) {
        Player player = ManagerPool.getInstance().playerMgr.getPlayerByConnId(connId);
        Login.reqlogin reqMsgObj = (Login.reqlogin)msg.getMsgObj();

        player.setPlayerName(reqMsgObj.getPlayerName());
        System.out.println(String.format("get Login: {%d, %d}", player.getConnectId(), player.getPlayerId()));

        // 返回消息;
        Login.reslogin.Builder oBui = Login.reslogin.newBuilder();
        oBui.setPlayerId(player.getPlayerId());
        ManagerPool.getInstance().myServer.sendMsg(connId, oBui.build());


//        Login.resQuit.Builder oBuiQuit = Login.resQuit.newBuilder();
//        oBuiQuit.setPlayerId(player.getPlayerId());
//        ManagerPool.getInstance().myServer.sendMsg(connId, MessageID_Define.GC_Quit, oBuiQuit.build());
    }
}
