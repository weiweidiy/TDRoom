using System;
using Unity.Netcode;
using UnityEngine;

public static class NetworkObjectExtensions
{
    /// <summary>
    /// 生成网络对象并立即生成
    /// </summary>
    public static GameObject SpawnNetworkObject(this GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        if (prefab.GetComponent<NetworkObject>() == null)
        {
            Debug.LogError("Prefab does not have NetworkObject component!");
            return null;
        }

        GameObject instance = GameObject.Instantiate(prefab);
        instance.GetComponent<NetworkObject>().Spawn();
        return instance;
    }

    /// <summary>
    /// 生成网络对象到指定位置和旋转
    /// </summary>
    public static GameObject SpawnNetworkObject(this GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        GameObject instance = GameObject.Instantiate(prefab, position, rotation);
        NetworkObject netObj = instance.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogError("Prefab does not have NetworkObject component!");
            GameObject.Destroy(instance);
            return null;
        }
        netObj.Spawn();
        return instance;
    }

    public static GameObject SpawnNetworkObject<T>(this GameObject prefab, Vector3 position, Quaternion rotation, Action<T> beforSpawn = null) where T : MonoBehaviour
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        GameObject instance = GameObject.Instantiate(prefab, position, rotation);
        NetworkObject netObj = instance.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogError("Prefab does not have NetworkObject component!");
            GameObject.Destroy(instance);
            return null;
        }
        if (beforSpawn != null)
        {
            //Debug.Log("Executing beforSpawn action");
            beforSpawn(instance.GetComponent<T>());
        }


        netObj.Spawn();
        return instance;
    }

    public static GameObject SpawnNetworkObjectAsPlayer<T>(this GameObject prefab, ulong clientId, Vector3 position, Quaternion rotation, Action<T> beforSpawn = null) where T : MonoBehaviour
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return null;
        }

        GameObject instance = GameObject.Instantiate(prefab, position, rotation);
        NetworkObject netObj = instance.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogError("Prefab does not have NetworkObject component!");
            GameObject.Destroy(instance);
            return null;
        }
        if (beforSpawn != null)
        {
            Debug.Log("Executing beforSpawn action");
            beforSpawn(instance.GetComponent<T>());
        }


        netObj.SpawnAsPlayerObject(clientId);
        return instance;
    }

    /// <summary>
    /// 生成网络对象并分配给特定客户端
    /// </summary>
    public static GameObject SpawnNetworkObject(this GameObject prefab, ulong clientId)
    {
        if (prefab == null) return null;

        GameObject instance = GameObject.Instantiate(prefab);
        NetworkObject netObj = instance.GetComponent<NetworkObject>();

        if (netObj == null)
        {
            Debug.LogError("Prefab does not have NetworkObject component!");
            GameObject.Destroy(instance);
            return null;
        }

        netObj.SpawnWithOwnership(clientId);
        return instance;
    }
}