using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerManager
{
    public event Action<IUnit, BaseAction, Vector3> onActionCast;

    GameObject playerPrefab;

    Dictionary<ulong, PlayerObject> playerObjects = new Dictionary<ulong, PlayerObject>();

    public PlayerManager(GameObject playerPrefab)
    {
        this.playerPrefab = playerPrefab;
    }

    public PlayerObject Spawn(ulong clientId, PlayerData playerData, Vector3 pos, IFinder finder)
    {
        if (playerObjects.ContainsKey(clientId))
            throw new System.Exception("已经创建了客户端player :" + clientId);

        var go = playerPrefab.SpawnNetworkObject<PlayerObject>(pos, Quaternion.identity, (player) =>
        {
            player.Init(playerData, finder);
            player.onActionCast += OnActionCast;
        });
        var playerObject = go.GetComponent<PlayerObject>();
        playerObjects.Add(clientId, playerObject);
        return playerObject;
    }

    private void OnActionCast(IUnit unit, BaseAction action, Vector3 vector)
    {
        onActionCast?.Invoke(unit, action, vector);
    }

    public PlayerObject GetPlayerObject(ulong clientId)
    {
        if (playerObjects.TryGetValue(clientId, out var playerObject))
        {
            return playerObject;
        }
        return null;
    }

    public List<PlayerObject> GetAllPlayerObjects()
    {
        return playerObjects.Values.ToList();
    }

    public void UpdateLogic(float deltaTime)
    {
        foreach (var playerObject in playerObjects.Values)
        {
            // 这里可以添加玩家对象的更新逻辑
            playerObject.UpdateLogic(deltaTime);
        }
    }
}
