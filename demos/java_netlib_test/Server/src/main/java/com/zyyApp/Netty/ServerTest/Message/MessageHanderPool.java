package com.zyyApp.Netty.ServerTest.Message;

import java.util.HashMap;

/**
 * 消息处理器 池;
 * Created by zyy on 2017/4/15.
 */
public class MessageHanderPool {
    private HashMap<Integer, MsgHanderBase> msgHanderMap = new HashMap<>();

    /**
     * 注册 消息;
     * @param msgId  消息ID;
     * @param hander 消息处理器;
     */
    public void register(int msgId, MsgHanderBase hander){
        if (msgHanderMap.containsKey(msgId)) {
            System.err.println(String.format("MessageHanderPool register err; repeate hander:%d", msgId));
            return;
        }

        msgHanderMap.put(msgId, hander);
    }

    /**
     * 获取 消息对应的转换器;
     * @param msgId  消息ID;
     */
    public MsgHanderBase getMessageHander(int msgId){
        return msgHanderMap.get(msgId);
    }
}
