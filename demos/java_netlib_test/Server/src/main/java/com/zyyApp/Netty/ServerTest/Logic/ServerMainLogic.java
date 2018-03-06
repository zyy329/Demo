package com.zyyApp.Netty.ServerTest.Logic;

import java.util.Scanner;

/**
 * 主逻辑框架;
 * Created by zyy on 2017/4/13.
 */
public class ServerMainLogic {
    public boolean closeSign = false;
    private Scanner sc=new Scanner(System.in);

    public void logic() {
        //
        start();

        //
        try {
            while (run()) {
                Thread.sleep(1);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        //
        System.exit(0);     // 结束整个进程;
    }

    private void start() {
        ManagerPool.getInstance().Init();
        ManagerPool.getInstance().myServer.Init(7731, -1);

        // 注册服务器关闭线程
        Runtime.getRuntime().addShutdownHook(new Thread(new CloseByExit()));
    }

    private boolean run() {
        if (closeSign) {
            return false;
        }

        // 通过输入命令来控制;
        if (sc.hasNext()) {
            String t=sc.next();
            System.out.println(String.format("get Input: %s;", t));
            if (t.equals("q")) {
                closeSign = true;
            }
        }
        return true;
    }

    // 关闭时的处理逻辑;
    private void onStop() {
        System.out.println(String.format("run Stop! %d",System.currentTimeMillis()));
        ManagerPool.getInstance().myServer.shutdown();
        System.out.println(String.format("end Stop! %d",System.currentTimeMillis()));
    }

    // 服务器关闭线程
    private class CloseByExit implements Runnable {
        CloseByExit() {
        }

        @Override
        public void run() {
            // 执行关闭事件
            System.out.println("Server Stop!");
            onStop();
            System.out.println("Server Stop!");
        }

    }
}
