using System;
using System.IO;
using System.Net.Sockets;

namespace Game
{
    /// <summary>
    /// 进程通信工具类，负责与主进程进行通信
    /// </summary>
    public class ProcessNetwork
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;

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

                SendMessage("这是房间进程发送给主工程消息");
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

    }
}
