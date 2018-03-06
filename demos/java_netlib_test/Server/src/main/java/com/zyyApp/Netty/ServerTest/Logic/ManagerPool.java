package com.zyyApp.Netty.ServerTest.Logic;

import com.zyyApp.Netty.ServerTest.Message.MessageHanderPool;
import com.zyyApp.Netty.ServerTest.Message.MessageManager;
import com.zyyApp.Netty.ServerTest.Player.PlayerMgr;

/**
 * Created by zyy on 2017/4/13.
 */
public class ManagerPool {
    public PlayerMgr playerMgr = new PlayerMgr();
    public MyServer myServer = new MyServer();
    public MessageHanderPool handerPool = new MessageHanderPool();
    public MessageManager messageManager = new MessageManager();

    // 单件;
    private static final ManagerPool instance = new ManagerPool();
    public static ManagerPool getInstance() {
        return instance;
    }

    private ManagerPool() {
    }

    public void Init() {
        messageManager.init();
    }
}
