package com.zyyApp.Netty.ServerTest.Login.handdler;

import com.zyyApp.Netty.ServerTest.Logic.ManagerPool;
import com.zyyApp.Netty.ServerTest.Message.MsgHanderBase;
import com.zyyApp.Netty.ServerTest.Message.protocol.AddressBookProtos;
import com.zyyApp.Netty.ServerTest.Player.struct.Player;
import com.zyyApp.netlib.protocol.Msg.Message;

/**
 * Created by zyy on 2017/4/14.
 */
public class ReqPerson extends MsgHanderBase{
    @Override
    public void action(Long connId, Message msg) {
        Player player = ManagerPool.getInstance().playerMgr.getPlayerByConnId(connId);
        AddressBookProtos.reqPerson reqMsgObj = (AddressBookProtos.reqPerson)msg.getMsgObj();

        System.out.println(String.format("get ReqPerson: {%d, %d} {%d}", player.getConnectId(), player.getPlayerId(), reqMsgObj.getId()));
    }
}
