package com.zyyApp.Netty.ServerTest.Player.struct;

/**
 * Created by zyy on 2017/4/13.
 */
public class Player {
    private long playerId;
    private long connectId;
    private String playerName;


    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
    public long getConnectId() {
        return connectId;
    }

    public void setConnectId(long connectId) {
        this.connectId = connectId;
    }

    public String getPlayerName() {
        return playerName;
    }

    public void setPlayerName(String playerName) {
        this.playerName = playerName;
    }
}
