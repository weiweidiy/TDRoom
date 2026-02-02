using Unity.Netcode;
using UnityEngine;

public class DynamicSpawnManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject[] sceneNetworkObjectsPrefabs;

    [SerializeField] private GameObject[] otherGameObjects;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {

            // 服务器动态生成所有需要的对象
            foreach (var prefab in sceneNetworkObjectsPrefabs)
            {
                var obj = Instantiate(prefab);
                obj.Spawn();
            }
        }


        if(IsClient)
        {
            foreach (var go in otherGameObjects)
            {
                Debug.Log("DynamicSpawnManager Instantiate otherGameObjects:" + go.name);
                Instantiate(go);
            }
        }

    }
}