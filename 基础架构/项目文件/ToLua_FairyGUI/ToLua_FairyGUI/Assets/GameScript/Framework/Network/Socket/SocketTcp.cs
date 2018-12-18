using System;
using System.Net;
using System.Net.Sockets;

namespace GameFramework
{
    public delegate void NetCallback(SocketTcp _socketTcp);

    public class SocketTcp
    {
        protected Socket socket;
        protected SocketError errorCode;

        int port;                        // 端口
        string host;                     // 主机
        string errorMessage;             // 异常消息
        int serverType = -1;             // 服务器类型，多连接标识（本来打算做成枚举的，但是考虑到要和Lua交互，做成枚举还是太麻烦了）
        bool isStartConnect = false;     // 是否开始连接

        public int Port { get { return port; } }
        public string Host { get { return host; } }
        public int ServerType { get { return serverType; } }
        public string ErrorMessage { get { return errorMessage; } }
        public bool IsStartConnect { get { return isStartConnect; } }

        bool responseConnect = true;
        ByteBuffer sendedData = new ByteBuffer();

        public SocketTcp(int _serverType)
        {
            Init(_serverType);
        }

        void Init(int _serverType)
        {
            serverType = _serverType;
            errorCode = new SocketError();
        }

        /// <summary>
        /// 设置服务器地址
        /// </summary>
        /// <param name="_host">目标主机</param>
        /// <param name="_port">目标端口</param>
        public void SetAddress(string _host, int _port)
        {
            host = _host;
            port = _port;
        }

        /// <summary>
        /// 同步连接服务器,自动启用接收数据线程
        /// </summary>
        /// <param name="_host">目标主机</param>
        /// <param name="_port">目标端口</param>
        public virtual void Connect(string _host, int _port)
        {
            SetAddress(_host, _port);
            try
            {
                // 同步连接
                // socket.Connect(_host, _port);

                IPAddress[] tempAddressArry = Dns.GetHostAddresses(_host);
                if (tempAddressArry[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                }
                else
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                socket.BeginConnect(tempAddressArry, _port, new AsyncCallback(AsyncConnect), socket);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Events.msInst.DispatchEvent(NetState.CONNECT_FAILED, this);
            }
        }

        void AsyncConnect(IAsyncResult _iar)
        {
            Socket client = (Socket)_iar.AsyncState;
            try
            {
                client.EndConnect(_iar);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                Events.msInst.DispatchEvent(NetState.CONNECT_FAILED, this);
            }
        }

        /// <summary>
        /// 检查有没有数据可接收
        /// </summary>
        /// <returns>连接已断开</returns>
        protected int CheckReceive()
        {
            if (!socket.Connected)
            {
                UnityEngine.Debug.LogError("--------------------NetState.DISCONNECTED-------------------CheckReceive------- " + socket.Connected.ToString());
                OnDisConnect();
                return -1;
            }

            // 获取可供读取的数据量
            int available = socket.Available;
            if (available > 0)
            {
                int length;
                byte[] bs = new byte[available];

                try
                {
                    length = socket.Receive(bs, 0, available, SocketFlags.None, out errorCode);
                }
                catch (ObjectDisposedException e)
                {
                    errorMessage = e.Message;
                    UnityEngine.Debug.LogError("--------------------NetState.DISCONNECTED-------------------CheckReceive2------- " + errorMessage.ToString());
                    OnDisConnect();
                    return -1;
                }

                DoReceive(bs, length);
            }

            return available;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="len">有效字节数</param>
        protected virtual void DoReceive(byte[] data, int len) { }

        /// <summary>
        /// 清除缓冲数据
        /// </summary>
        public void ClearCachedData()
        {
            sendedData.Clear();
        }

        /// <summary>
        /// 发送缓冲数据
        /// </summary>
        public void ReSendCachedData()
        {
            if (sendedData.ReadableLen() > 0)
            {
                while (sendedData.ReadableLen() > 0)
                {
                    int readLen = int.MaxValue;

                    if (sendedData.ReadableLen() < int.MaxValue)
                    {
                        readLen = (int)sendedData.ReadableLen();
                    }

                    DoSend(sendedData.ReadBytes(readLen));
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="_data">数据</param>
        /// /// <param name="onSended">发送成功回调</param>
        public virtual void Send(byte[] _data)
        {
            sendedData.WriteBytes(_data);
            DoSend(_data);
        }

        void DoSend(byte[] _data)
        {
            try
            {
                if (IsConnected(true))
                {
                    try
                    {
                        socket.BeginSend(_data, 0, _data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                    }
                    catch (ObjectDisposedException e)
                    {
                        errorMessage = e.Message;
                        UnityEngine.Debug.LogError("--------------------DoSend-------------------CheckReceive1------- " + errorMessage.ToString());
                        OnDisConnect();
                    }
                    catch (SocketException e)
                    {
                        errorMessage = e.Message;
                        UnityEngine.Debug.LogError("--------------------DoSend-------------------CheckReceive2------- " + errorMessage.ToString());
                        OnDisConnect();
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("--------------------DoSend-------------------CheckReceive3------- ");
                    OnDisConnect();
                }
            }
            catch (ObjectDisposedException e)
            {
                errorMessage = e.Message;
                UnityEngine.Debug.LogError("--------------------DoSend-------------------CheckReceive4------- " + errorMessage.ToString());
                OnDisConnect();
            }
            catch (SocketException e)
            {
                errorMessage = e.Message;
                UnityEngine.Debug.LogError("--------------------DoSend-------------------CheckReceive5------- " + errorMessage.ToString());
                OnDisConnect();
            }
        }

        protected virtual void SendCallback(IAsyncResult _ar)
        {
            Socket socket = (Socket)_ar.AsyncState;
            int len = socket.EndSend(_ar);

            //将成功发送的数据从缓存中剔除，数据全部发送完后清空缓存释放内存
            sendedData.ReadBytes(len);
            if (sendedData.ReadableLen() == 0)
            {
                sendedData.Clear();
            }
        }

        /// <summary>
        /// 当与服务器断开的时候
        /// </summary>
        /// <param name="tc"></param>
        protected virtual void OnDisConnect()
        {
            //if (socket == null)
            //{
            //    UnityEngine.Debug.LogError("socket value or null");
            //    return;
            //}

            //socket.Close();
            //socket = null;
            UnityEngine.Debug.LogError("--------------------NetState.DISCONNECTED--------------------------");
            Events.msInst.DispatchEvent(NetState.DISCONNECTED, this);
        }

        public virtual void Loop()
        {
            if (socket == null)
            {
                return;
            }

            if (socket.Connected && responseConnect)
            {
                responseConnect = false;
                Events.msInst.DispatchEvent(NetState.CONNECT_SUCCEED, this);
            }
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnected(bool really = false)
        {
            if (socket == null)
            {
                return false;
            }

            if (socket.Connected && IsNerworkNormal())
            {
                if (really)
                {
                    try
                    {
                        // Poll 只能用于检测网络电缆正常且远程主机不关闭的情况
                        if (socket.Poll(100, SelectMode.SelectRead) && CheckReceive() < 1)
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError("--------------------NetState.DISCONNECTED-------------------IsConnected------- "+really.ToString());
                        OnDisConnect();
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 重置连接
        /// </summary>
        public virtual void ReConnect()
        {
            CloseAndInitTcp();
            Connect();
        }

        public void CloseAndInitTcp()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }

            responseConnect = true;
            Init(ServerType);
        }

        /// <summary>
        /// 同步连接服务器,延用最后一次的连接
        /// </summary>
        public virtual void Connect()
        {
            if (host != null && port > 0)
            {
                Connect(host, port);
            }
        }

        /// <summary>
        /// 清除连接及资源
        /// </summary>
        public virtual void Clear()
        {
            ClearCachedData();

            errorMessage = "";
            responseConnect = true;
            isStartConnect = false;

            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }

        bool IsNerworkNormal() { return UnityEngine.Application.internetReachability != UnityEngine.NetworkReachability.NotReachable; }
    }
}