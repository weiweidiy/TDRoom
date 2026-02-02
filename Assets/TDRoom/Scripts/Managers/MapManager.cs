using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class MapManager
{
    GameObject mapPrefab;

    MapObject mapObject;

    public MapManager(GameObject mapPrefab)
    {
        this.mapPrefab = mapPrefab;
    }

    public MapObject Spawn(int mapId)
    {
        var go = mapPrefab.SpawnNetworkObject();
        mapObject = go.GetComponent<MapObject>();
        return mapObject;
    }

    public Vector3 GetSeatPosition(int index)
    {
        return mapObject.GetSeatPosition(index);
    }

    public Vector3 GetDoorPosition(int index)
    {
        return mapObject.GetDoorPosition(index);
    }


}
