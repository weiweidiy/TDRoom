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

                Console.WriteLine("connected");

                var data = new ReqRoomReady()
                {
                    RoomId = RoomId,
                    //Port = Port
                };
                string json = JsonConvert.SerializeObject(data);
                SendMessage(json);

                // 连接成功后，启动消息接收
                //System.Threading.Tasks.Task.Run(() => ReceiveMessages());

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败: {ex.Message}");
                UnityEngine.Debug.Log($"connect failed: {ex.Message}");
                return false;
            }
        }

        public void SendMessage(string message)
        {
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
                        // var msg = JsonConvert.DeserializeObject<ResRoomReady>(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"接收消息失败: {ex.Message}");
                UnityEngine.Debug.Log($"receive failed: {ex.Message}");
            }
        }

    }
}
