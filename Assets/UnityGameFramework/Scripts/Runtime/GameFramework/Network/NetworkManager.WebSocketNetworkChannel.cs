using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using UnityWebSocket;
using WebSocket = UnityWebSocket.WebSocket;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        /// <summary>
        /// WebSocket 网络频道。
        /// </summary>
        private sealed class WebSocketNetworkChannel : NetworkChannelBase
        {
            /// <summary>
            /// 初始化网络频道的新实例。
            /// </summary>
            /// <param name="name">网络频道名称。</param>
            /// <param name="networkChannelHelper">网络频道辅助器。</param>
            public WebSocketNetworkChannel(string name, INetworkChannelHelper networkChannelHelper)
                : base(name, networkChannelHelper)
            {
            }

            /// <summary>
            /// 获取网络服务类型。
            /// </summary>
            public override ServiceType ServiceType
            {
                get
                {
                    return ServiceType.Tcp;
                }
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="userData">用户自定义数据。</param>
            public override void Connect(IPAddress ipAddress, int port, object userData)
            {
                base.Connect(ipAddress, port, userData);

                string address = string.Format("ws://{0}:{1}", ipAddress, port);
                m_Socket = new WebSocket(address);
                m_Socket.OnOpen += Socket_OnOpen;
                m_Socket.OnMessage += Socket_OnMessage;
                m_Socket.OnClose += Socket_OnClose;
                m_Socket.OnError += Socket_OnError;
                
                if (m_Socket == null)
                {
                    string errorMessage = "Initialize network channel failure.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SocketError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                m_NetworkChannelHelper.PrepareForConnecting();
                m_Socket.ConnectAsync();
            }

            protected override bool ProcessSend()
            {
                if (base.ProcessSend())
                {
                    SendAsync();
                    return true;
                }

                return false;
            }

            private void SendAsync()
            {
                try
                {
                    // var buffer = m_SendState.Stream.GetBuffer();
                    var buffer = m_SendState.Stream.ToArray();
                    m_Socket.SendAsync(buffer);
                    
                    int bytesSent = buffer.Length;
    
                    m_SendState.Stream.Position += bytesSent;

                    m_SentPacketCount++;
                    m_SendState.Reset();
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        SocketException socketException = exception as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.SendError, socketException != null ? socketException.SocketErrorCode : SocketError.Success, exception.ToString());
                        return;
                    }

                    throw;
                }
            }

            private void Socket_OnOpen(object sender, OpenEventArgs e)
            {
                // AddLog(string.Format("Connected: {0}", m_Socket.Address));
                

                m_SentPacketCount = 0;
                m_ReceivedPacketCount = 0;

                lock (m_SendPacketPool)
                {
                    m_SendPacketPool.Clear();
                }

                m_ReceivePacketPool.Clear();

                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(true);
                }

                if (NetworkChannelConnected != null)
                {
                    NetworkChannelConnected(this, m_Socket.Address);
                }

                m_Active = true;
            }
    
            private void Socket_OnMessage(object sender, MessageEventArgs e)
            {
                int bytesReceived = e.RawData.Length;
                if (bytesReceived <= 0)
                {
                    Close();
                    return;
                }

                // m_ReceiveState.Stream.Write(e.RawData, (int)m_ReceiveState.Stream.Position, 1);
                // m_ReceiveState.Stream.Position += bytesReceived;
                //
                // m_ReceiveState.Stream.Position = 0L;
                //
                // bool processSuccess = false;
                // if (m_ReceiveState.PacketHeader != null)
                // {
                //     processSuccess = ProcessPacket();
                //     m_ReceivedPacketCount++;
                // }
                // else
                // {
                //     processSuccess = ProcessPacketHeader();
                // }

                byte[] recBuffer = e.RawData.ToArray();

                // 检验包是否合法
                bool isValid = Utility.Package.CheckPackage(recBuffer);
                if (!isValid) return;
                
                //解包开始======================
                //协议ID
                int protoId = Utility.Package.GetProtoID(recBuffer);
                //protoBody
                byte[] protoBody = Utility.Package.GetProtoBody(recBuffer);
                //包体长度
                int packageLen = Utility.Package.GetPackageLength(recBuffer);
                //解包结束======================
                
                Packet packet = m_NetworkChannelHelper.DeserializeProto(protoId, protoBody);
                m_ReceivedPacketCount++;
                
                if (packet != null)
                {
                    m_ReceivePacketPool.Fire(this, packet);
                }

                ProcessPacket();

                return;
            }
            
            protected override bool ProcessPacket()
            {
                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(m_ResetHeartBeatElapseSecondsWhenReceivePacket);
                }

                return true;
            }
    
            private void Socket_OnClose(object sender, CloseEventArgs e)
            {
                int jj = 0;
                // AddLog(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
            }
    
            private void Socket_OnError(object sender, ErrorEventArgs e)
            {
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SocketError, SocketError.Success, e.Message);
                    return;
                }
            }
        }
    }
}
