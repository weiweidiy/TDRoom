using JFramework;
using System;
using UnityWebSocket;

namespace Game
{
    public class UnityWebSocket : JBaseSocket
    {
        WebSocket socket;
        private bool isOpen;

        public override bool IsOpen
        {
            get => socket != null && socket.ReadyState == WebSocketState.Open;
            set => isOpen = value;
        }

        public override void Init(string url, string token = null)
        {
            socket = new WebSocket(url);
            // 注册事件
            socket.OnOpen += Socket_OnOpen;
            socket.OnMessage += Socket_OnMessage;
            socket.OnClose += Socket_OnClose;
            socket.OnError += Socket_OnError;
        }

        private void Socket_OnError(object sender, ErrorEventArgs e)
        {
            isOpen = false;
            OnError(this, e.Message);
        }

        private void Socket_OnClose(object sender, CloseEventArgs e)
        {
            isOpen = false;
            // 关闭时触发基类事件
            OnClosed(this, SocketStatusCodes.NormalClosure, e.Reason ?? string.Empty);
        }

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            // 根据消息类型分发
            if (e.Opcode == Opcode.Text && !string.IsNullOrEmpty(e.Data))
            {
                OnMessage(this, e.Data);
            }
            else if (e.Opcode == Opcode.Binary && e.RawData != null)
            {
                OnBinary(this, e.RawData);
            }
        }

        private void Socket_OnOpen(object sender, OpenEventArgs e)
        {
            isOpen = true;
            OnOpen(this);
        }

        public override void Open()
        {
            socket?.ConnectAsync();
        }

        public override void Close()
        {
            socket?.CloseAsync();
        }

        public override void Send(byte[] data)
        {
            if (IsOpen)
            {
                socket.SendAsync(data);
            }
        }
    }
}