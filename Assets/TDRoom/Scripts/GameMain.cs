using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        ProcessNetwork network;
        private void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();

            var obj = Instantiate(spawner.gameObject);

            network = new ProcessNetwork();

        }

        private async void Start()
        {
            var args = GetEnviromentArgs();
            var ip = isClient ? GlobalBoard.Ip : "0.0.0.0";
            var port = isClient ? GlobalBoard.Port : args.port;
            var maxPlayers = isClient ? 1 : args.maxPlayers;
            var roomId = isClient ? "" : args.roomId;
            var playerIds = args.playerIds;

            Debug.Log($"GameMain Start. isClient:{isClient}, ip:{ip}, port:{port}");
            m_NetworkManager.GetComponent<UnityTransport>().SetConnectionData(ip, port);

            //#if !TDROOM_SERVER
            if (isClient)
                m_NetworkManager.StartClient();
            //#else
            else
            {
                m_NetworkManager.StartServer();
                network.RoomId = roomId;
                Debug.Log($"GameMain Start Server. roomId:{roomId}, maxPlayers:{maxPlayers}");
                network.Port = port;
                //服务器启动了，通知主服务器
                if (network.ConnectToMainProcess(9999))
                {
                    Debug.Log("Connected to main process successfully.");
                    await UniTask.Delay(1000); // 等待一段时间，确保连接稳定
                    var data = new ReqRoomReady()
                    {
                        RoomId = roomId,
                        //Port = Port
                    };
                    network.SendMessage(data);
                }
            }
            //#endif

        }


        (string roomId, ushort port, int maxPlayers, int[] playerIds) GetEnviromentArgs()
        {
            var args = Environment.GetCommandLineArgs();
            string roomId = null;
            ushort port = 7777;
            int maxPlayers = 2;
            int[] playerIds = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-roomId" && i + 1 < args.Length)
                    roomId = args[i + 1];
                else if (args[i] == "-port" && i + 1 < args.Length)
                    port = ushort.Parse(args[i + 1]);
                else if (args[i] == "-maxPlayers" && i + 1 < args.Length)
                    maxPlayers = int.Parse(args[i + 1]);
                else if (args[i] == "-playerIds")
                {
                    // 解析玩家ID列表
                    var ids = args[i + 1].Split(',');
                    foreach(var id in ids)
                    {
                        Debug.Log($"playerId: {id}");
                    }
                    playerIds = Array.ConvertAll(ids, int.Parse);
                }

            }
            return (roomId, port, maxPlayers, playerIds);
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
