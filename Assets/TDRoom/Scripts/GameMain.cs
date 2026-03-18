using Cysharp.Threading.Tasks;
using JFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Game
{
    public class RoomNetMessageTypeRegister : ITypeRegister
    {
        public Dictionary<int, Type> GetTypes()
        {
            var result = new Dictionary<int, Type>();

            result.Add((int)TDRoomProtocolType.ReqRoomReady, typeof(ReqRoomReady));
            result.Add((int)TDRoomProtocolType.ResRoomReady, typeof(ResRoomReady));
            result.Add((int)TDRoomProtocolType.ReqPlayerData, typeof(ReqPlayerData));
            result.Add((int)TDRoomProtocolType.ResPlayerData, typeof(ResPlayerData));
            return result;
        }
    }

    public class RoomMessageHandler : INetworkMessageHandler
    {
        public void Handle(IJNetMessage message)
        {
            switch(message.TypeId)
            {

                case (int)TDRoomProtocolType.ResRoomReady:
                    var res = message as ResRoomReady;
                    Debug.Assert(res != null, "message should be of type ResRoomReady");
                    Debug.Log($"Received ResRoomReady. Code:{res.Code}");
                    // 处理房间准备就绪响应的逻辑
                    break;

                case (int)TDRoomProtocolType.ResPlayerData:
                    var resPlayerData = message as ResPlayerData;
                    Debug.Log($"Received from server PlayerId:{resPlayerData.PlayerId}, PlayerName:{resPlayerData.PlayerName}");
                    // 处理玩家数据响应的逻辑
                    break;
                default:
                    Debug.LogWarning($"Unknown message type: {message.TypeId}");
                    break;
            }
        }
    }

    /// <summary>
    /// Add this component to the same GameObject as
    /// the NetworkManager component.
    /// </summary>
    public class GameMain : MonoBehaviour
    {
        private NetworkManager m_NetworkManager;

        [SerializeField] DynamicSpawnManager spawner;

        [SerializeField] bool isClient = false;

        Dictionary<ulong, string> clientIdToPlayerId = new Dictionary<ulong, string>();

        //ProcessNetwork network;
        IJNetwork network;
        private void Awake()
        {
            m_NetworkManager = GetComponent<NetworkManager>();

            var obj = Instantiate(spawner.gameObject);

            //network = new ProcessNetwork();
            var builder = new JNetworkBuilder()
                .SetProtocolRegister(new RoomNetMessageTypeRegister())
                .SetMessageHandler(new RoomMessageHandler());
            network =  builder.Build();
            

        }

        private async void Start()
        {
            var args = GetEnviromentArgs();
            var ip = isClient ? GlobalBoard.Ip : "0.0.0.0";
            var port = isClient ? GlobalBoard.Port : args.port;
            var maxPlayers = isClient ? 1 : args.maxPlayers;
            var roomId = isClient ? "" : args.roomId;
            var playerIds = args.playerIds;
            var playerId = GlobalBoard.PlayerUid;

            Debug.Log($"GameMain Start. isClient:{isClient}, ip:{ip}, port:{port}");
            m_NetworkManager.GetComponent<UnityTransport>().SetConnectionData(ip, port);

            //#if !TDROOM_SERVER
            if (isClient)
            {
                m_NetworkManager.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(playerId.ToString());
                //m_NetworkManager.StartHost();
                m_NetworkManager.StartClient();
            }
               
            //#else
            else
            {
                m_NetworkManager.ConnectionApprovalCallback += ApprovalCheck;
                m_NetworkManager.StartServer();
                //network.RoomId = roomId;
                Debug.Log($"GameMain Start Server. roomId:{roomId}, maxPlayers:{maxPlayers}");
                //network.Port = port;
                //服务器启动了，通知主服务器

                await network.Connect("127.0.0.1:9999");

                Debug.Log("Connected to main process successfully.");
                await UniTask.Delay(1000); // 等待一段时间，确保连接稳定
                var data = new ReqRoomReady()
                {
                    RoomId = roomId,
                    //Port = Port
                };
                var res = await network.SendMessage<ResRoomReady>(data);

                Debug.Assert(res != null, "res should not be null");

                Debug.Log("SendMessage roomready successfully." + res.Code);
            }
            //#endif

        }

        private void OnDestroy()
        {
            m_NetworkManager.ConnectionApprovalCallback -= ApprovalCheck;
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            var args = GetEnviromentArgs();
            var playerIds = args.playerIds;

            Debug.Log("ApprovalCheck :" + request.ClientNetworkId);
            // 将客户端传来的字节数据解析为字符串（假设发送的是 userId）
            string userId = System.Text.Encoding.UTF8.GetString(request.Payload);
            Debug.Log($"Client {request.ClientNetworkId}  userId: {userId}");

            // 验证逻辑：例如检查数据库或与合法用户列表比对
            bool isApproved = ValidateUser(userId, playerIds);

            // 可以自定义返回给客户端的消息（可选）
            // 使用 Create() 创建自定义消息（需要定义 NetworkObject 等），此处简化
            //response(true, null, isApproved, null, null);

            // 设置响应
            response.Approved = isApproved;
            response.CreatePlayerObject = false; // 或者 true 如果需要自动生成玩家对象
                                                 // response.PlayerPrefabHash = 如果 CreatePlayerObject 为 true，需要指定 prefab hash
            response.Reason = isApproved ? "Welcome" : "Invalid user"; // 拒绝时可提供原因
            response.Pending = false; // 同步完成

            if(isApproved)
            {
                clientIdToPlayerId.Add(request.ClientNetworkId, userId);
            }
            

            // 如果 isValid 为 false，客户端会被断开
        }

        private bool ValidateUser(string userId, string[] playersId)
        {
            // 实现具体验证，例如查询用户表
            return playersId.Contains(userId);
        }

        public string GetPlayerId(ulong clientId)
        {
            if (clientIdToPlayerId.TryGetValue(clientId, out string playerId))
            {
                return playerId;
            }
            
            throw new Exception($"ClientId {clientId} not found in clientIdToPlayerId mapping.");
        }

        public async Task<TResponse> SendMessage<TResponse>(IJNetMessage pMsg, TimeSpan? timeout = null) where TResponse : class, IJNetMessage
        {
            var res = await network.SendMessage<TResponse>(pMsg);
            return res;
        }


        (string roomId, ushort port, int maxPlayers, string[] playerIds) GetEnviromentArgs()
        {
            var args = Environment.GetCommandLineArgs();
            string roomId = null;
            ushort port = 7777;
            int maxPlayers = 2;
            string[] playerIds = null;

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
                    playerIds = args[i + 1].Split(',');
                    foreach(var id in playerIds)
                    {
                        //Debug.Log($"playerId: {id}");
                    }
                    //playerIds = Array.ConvertAll(ids, int.Parse);
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
