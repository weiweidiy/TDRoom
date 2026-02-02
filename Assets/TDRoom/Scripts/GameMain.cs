using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Add this component to the same GameObject as
    /// the NetworkManager component.
    /// </summary>
    public class GameMain : MonoBehaviour
    {
        private NetworkManager m_NetworkManager;

        [SerializeField] DynamicSpawnManager spawner;

        [SerializeField] bool isClient = false;

        private void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();

            var obj = Instantiate(spawner.gameObject);

        }

        private void Start()
        {
            var ip = GlobalBoard.Ip;
            var port = GlobalBoard.Port;
            if (!isClient)
            {
                ip = "0.0.0.0";
                port = 7777;
            }
            //else
            //{
            //    ip = "127.0.0.1";
            //    port = 7777;
            //}
            Debug.Log($"GameMain Start. isClient:{isClient}, ip:{ip}, port:{port}");
            m_NetworkManager.GetComponent<UnityTransport>().SetConnectionData(ip, port);

            if (isClient)
                m_NetworkManager.StartClient();
        }

        //private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
        //{
        //    SpawnPrefab();
        //}

        //public void SpawnPrefab()
        //{
        //    if (!IsServer) return;

        //    // 方法1.15：遍历查找预制体


        //    var prefabList = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs;
        //    Debug.Log($"共有 {prefabList.Count} 个预制体");

        //    foreach (var networkPrefab in prefabList)
        //    {
        //        if (networkPrefab != null && networkPrefab.Prefab != null)
        //        {
        //            GameObject newObject = Instantiate(networkPrefab.Prefab, Vector3.zero, Quaternion.identity);
        //            NetworkObject networkObject = newObject.GetComponent<NetworkObject>();
        //            networkObject.Spawn();
        //            Debug.Log($"已生成: {networkPrefab.Prefab.name}");
        //        }
        //        else
        //        {
        //            Debug.LogError($"找不到哈希值为 {mapHash} 的预制体");
        //        }
        //    }


        //}



        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        private void StartButtons()
        {
            if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
            if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
            if (GUILayout.Button("Server")) m_NetworkManager.StartServer();


        }

        private void StatusLabels()
        {
            var mode = m_NetworkManager.IsHost ?
                "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        private void SubmitNewPosition()
        {

            //if (GUILayout.Button(m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
            //{
            //    if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
            //    {
            //        foreach (ulong uid in m_NetworkManager.ConnectedClientsIds)
            //            m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerObject>().Move();
            //    }
            //    else
            //    {
            //        var playerObject = m_NetworkManager.SpawnManager.GetLocalPlayerObject();
            //        var player = playerObject.GetComponent<PlayerObject>();
            //        player.Move();
            //    }
            //}
        }
    }
}
