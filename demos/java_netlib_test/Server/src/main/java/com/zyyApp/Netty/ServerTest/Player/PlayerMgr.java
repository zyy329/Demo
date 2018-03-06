package com.zyyApp.Netty.ServerTest.Player;

import com.zyyApp.Netty.ServerTest.Player.struct.Player;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;

/**
 * Created by zyy on 2017/4/13.
 */
public class PlayerMgr {
    private AtomicLong idCreater = new AtomicLong(0);
    private Map<Long, Player> playerMap = new ConcurrentHashMap<>();
    private Map<Long, Long> connectIDMap = new ConcurrentHashMap<>();

    public long addPlayer(Player player) {
        long id = idCreater.getAndIncrement();
        playerMap.put(id, player);
        connectIDMap.put(player.getConnectId(), id);
        return id;
    }

    public Player getPlayerById(Long playerId) {
        return playerMap.get(playerId);
    }
    public Player getPlayerByConnId(Long connectId) {
        Long playerId = connectIDMap.get(connectId);
        if (playerId != null) {
            return getPlayerById(playerId);
        }
        return null;
    }
    public void removePlayerById(Long playerId) {
        Player player = getPlayerById(playerId);
        if (player != null) {
            connectIDMap.remove(player.getConnectId());
            playerMap.remove(player.getPlayerId());
        }
    }
    public void removePlayerByConnId(Long connectId) {
        Player player = getPlayerByConnId(connectId);
        if (player != null) {
            connectIDMap.remove(player.getConnectId());
            playerMap.remove(player.getPlayerId());
        }
    }
}
