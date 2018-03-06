package com.zyyApp.Netty.ServerTest.Message;

import com.zyyApp.netlib.protocol.Msg.Message; /**
 * 消息处理器接口;
 * Created by zyy on 2017/4/14.
 */
public abstract class MsgHanderBase {
    public abstract void action(Long connId, Message msg);
}
