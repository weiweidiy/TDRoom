using JFramework;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;

namespace Game
{
    public enum TDRoomProtocolType
    {
        ReqRoomReady = 1,
        ResRoomReady = 2,
        ReqPlayerData = 3,
        ResPlayerData = 4,
        // 其他协议类型...
    }

    public class ReqRoomReady : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ReqRoomReady; }
        public string RoomId { get; set; } = string.Empty;
    }

    public class ResRoomReady : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ResRoomReady; }

        public int Code;
    }

    public class ReqPlayerData : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ReqPlayerData; }
        public string PlayerId { get; set; }
    }

    public class ResPlayerData : JNetMessage
    {
        public override string Uid { get; set; } = Guid.NewGuid().ToString();
        public override int TypeId { get => (int)TDRoomProtocolType.ResPlayerData; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
    }



    /// <summary>
    /// 进程通信工具类，负责与主进程进行通信
    /// </summary>
    public class ProcessNetwork
    {
        public string RoomId { get; set; }
        public ushort Port { get; set; }

        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        public bool ConnectToMainProcess(int communicationPort)
        {
            try
            {
                //Console.WriteLine($"尝试连接到主进程，端口: {communicationPort}");
                UnityEngine.Debug.Log($"try to connect ，port: {communicationPort}");
                client = new TcpClient("127.0.0.1", communicationPort);
                stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);

                Console.WriteLine("connected");


                //string json = JsonConvert.SerializeObject(data);
                
                // 连接成功后，启动消息接收
                System.Threading.Tasks.Task.Run(() => ReceiveMessages());



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败: {ex.Message}");
                UnityEngine.Debug.Log($"connect failed: {ex.Message}");
                return false;
            }
        }

        public void SendMessage(object data)
        {
            string message = JsonConvert.SerializeObject(data);
            try
            {
                if (client?.Connected == true && writer != null)
                {
                    writer.WriteLine(message);
                    Console.WriteLine($"已发送消息: {message}");
                    UnityEngine.Debug.Log($"send message: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送消息失败: {ex.Message}");
                UnityEngine.Debug.Log($"send failed: {message}");
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (client?.Connected == true)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine($"收到主进程消息: {line}");
                        UnityEngine.Debug.Log($"received message: {line}");

                        // 你可以在这里反序列化并处理消息
                        // 例如:
                        var msg = JsonConvert.DeserializeObject<JNetMessage>(line);
                        var msgTypeId = msg.TypeId;
                        CreateMessageObject(msgTypeId, line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"接收消息失败: {ex.Message}");
                UnityEngine.Debug.Log($"receive failed: {ex.Message}");
            }
        }


        void CreateMessageObject(int typeId, string message)
        {
            switch(typeId)
            {
                case (int)TDRoomProtocolType.ResRoomReady:
                    var res = JsonConvert.DeserializeObject<ResRoomReady>(message);
                    Console.WriteLine($"处理 ResRoomReady 消息，Code: {res.Code}");
                    UnityEngine.Debug.Log($"handle ResRoomReady message, Code: {res.Code}");
                    break;
                case (int)TDRoomProtocolType.ResPlayerData:
                    var playerData = JsonConvert.DeserializeObject<ResPlayerData>(message);
                    Console.WriteLine($"处理 ResPlayerData 消息，PlayerId: {playerData.PlayerId}, PlayerName: {playerData.PlayerName}");
                    UnityEngine.Debug.Log($"handle ResPlayerData message, PlayerId: {playerData.PlayerId}, PlayerName: {playerData.PlayerName}");
                    break;
                default:
                    throw new Exception($"未知的消息类型: {typeId}");
            }
        }
    }
}
