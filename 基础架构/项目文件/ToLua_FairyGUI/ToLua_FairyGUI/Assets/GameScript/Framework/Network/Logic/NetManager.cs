using UnityEngine;
using LuaInterface;
using System.Collections.Generic;

namespace GameFramework
{
    public class NetManager : MonoBehaviour
    {
        public static NetManager mInst = null;
        void Awake()
        {
            mInst = this;
        }

        /// <summary>
        /// 最大重新连接数量
        /// </summary>
        const int MAX_RECONNECT_COUNT = 30;
        /// <summary>
        /// 重连的超时时间
        /// </summary>
        const int reConnectCheckTimeout = 3;

        /// <summary>
        /// 重新连接等待时间
        /// </summary>
        int reConnectWaitTime = 8;
        /// <summary>
        /// 重新连接的框架
        /// </summary>
        int reConnectCheckFrame = 30;
        /// <summary>
        /// 重连的列表
        /// </summary>
        List<int> reConnectList = new List<int>();

        /// <summary>
        /// 检查连接的心跳空包
        /// </summary>
        uint Connect_Empty;

        /// <summary>
        /// 发送间距
        /// </summary>
        int sendGap = 1;
        /// <summary>
        /// 设置连接超时,(秒,重连间隔时间 socket当前重连时间
        /// </summary>
        float reconnectTime;
        /// <summary>
        ///  从断开到自动重连的次数
        /// </summary>
        int reconnectNum = 0;
        /// <summary>
        /// 从断开到自动重连的间距
        /// </summary>
        int reconnectGap = 0;
        /// <summary>
        /// 是否可以重连
        /// 目前设定只有在登录的时候才会启动
        /// </summary>
        bool isConnected;

        /// <summary>
        /// 连接成功会触发的 Lua 事件
        /// </summary>
        LuaFunction onConnected;
        /// <summary>
        /// 断开连接会触发 Lua 事件
        /// </summary>
        LuaFunction onDisconnected;
        /// <summary>
        /// 连接失败会触发的 Lua 事件
        /// </summary>
        LuaFunction onConnectFailed;
        /// <summary>
        /// 断线重连失败会触发的 Lua 事件
        /// </summary>
        LuaFunction onReConnectFailed;

        /// <summary>
        /// 服务器列表，连接多个服务器
        /// </summary>
        List<SocketClientProxy> socketList = new List<SocketClientProxy>();

        void Start()
        {
            Events.msInst.AddEventListener(NetState.CONNECT_SUCCEED, OnConnected);
            Events.msInst.AddEventListener(NetState.DISCONNECTED, OnDisconnected);
            Events.msInst.AddEventListener(NetState.CONNECT_FAILED, OnConnectFailed);
        }

        public void Update()
        {
            if (reConnectList != null && reConnectList.Count > 0)
            {
                if (Time.time >= reconnectTime + reConnectCheckTimeout)
                {
                    for (int i = 0; i < reConnectList.Count; i++)
                    {
                        IsReConnect(reConnectList[i], false);
                    }

                    reconnectTime = Time.time;
                }
            }

            if (socketList == null)
            {
                return;
            }

            for (int i = 0; i < socketList.Count; i++)
            {
                // 非等待重连的才更新
                socketList[i].Loop();
            }
        }

        /// <summary>
        /// 判断服务器是否连接
        /// </summary>
        /// <param name="_serverType">服务器类型，服务器是可以有很多个的</param>
        public bool IsConnected(int _serverType, bool _really = false)
        {
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return false;
            }

            return tempSocket.IsConnected(_really);
        }

        /// <summary>
        /// 是否重新连接
        /// </summary>
        /// <param name="_serverType">服务器ID</param>
        /// <param name="_isFirstConnect">是否第一次连接</param>
        public bool IsReConnect(int _serverType, bool _isFirstConnect = true)
        {
            // 这里控制能重新登录指定的次数
            if (reconnectNum > MAX_RECONNECT_COUNT)
            {
                // 重新登录的次数达到目标次数，返回连接失败
                ReConnectFail(_serverType);
                return false;
            }

            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return false;
            }

            reconnectNum++;
            if (_isFirstConnect)
            {
                reconnectTime = Time.time;
                tempSocket.ReConnect();
                return true;
            }

            // 重置连接的数据
            tempSocket.ReConnect();
            return true;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="_serverType">服务器类型</param>
        /// <param name="_host">主机</param>
        /// <param name="_port">端口</param>
        public void ConnectAsync(int _serverType, string _host, int _port, LuaFunction _onConnected, LuaFunction _onDisconnected, LuaFunction _onConnectFailed, LuaFunction _onReConnectFailed)
        {
            if (socketList == null)
            {
                return;
            }
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return;
            }
            isConnected = false;
            onConnected = _onConnected;
            onDisconnected = _onDisconnected;
            onConnectFailed = _onConnectFailed;
            onReConnectFailed = _onReConnectFailed;
            
            // 同步连接
            tempSocket.Connect(_host, _port);
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <param name="_serverType">服务器类型</param>
        public void ReConnect(int _serverType, bool _isReConnect)
        {
            if (_isReConnect && reConnectList != null && !reConnectList.Contains(_serverType))
            {
                reConnectList.Add(_serverType);
            }
        }

        /// <summary>
        /// 添加网络服务监听
        /// </summary>
        /// <param name="service">服务接口类型</param>
        /// <param name="_callBack">回调函数</param>
        public void AddListener(int _serverType, uint _serviceId, LuaFunction _callBack)
        {
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return;
            }

            tempSocket.AddListener(_serviceId, _callBack);
        }

        /// <summary>
        /// 删除网络服务监听
        /// </summary>
        /// <param name="service">服务接口类型</param>
        /// <param name="_callBack">回调函数</param>
        public void RemoveListener(int _serverType,  uint _serviceId, LuaFunction _callBack)
        {
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return;
            }

            tempSocket.RemoveListener(_serviceId, _callBack);
        }

        /// <summary>
        /// 关闭和清除连接
        /// </summary>
        /// <param name="_serverType">服务器类型</param>
        public void OnCloseAndClearTcp(int _serverType)
        {
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return;
            }

            tempSocket.CloseAndInitTcp();
        }

        /// <summary>
        /// 关闭和清除全部连接
        /// </summary>
        public void OnCloseAndClearAllTcp()
        {
            for (int i = 0; i < socketList.Count; i++)
            {
                OnCloseAndClearTcp(socketList[i].ServerType);
            }
        }

        /// <summary>
        /// 发送成功就回调的发送方式,不需要返回数据,支持断网缓存用户操作
        /// </summary>
        /// <param name="_data">发送数据</param>
        /// <param name="_serviceid">协议ID</param>
        /// <param name="_requestData">上传的数据，会原样返回</param>
        public void SendData(int _serverType, byte[] _data, uint _serviceid, object _requestData)
        {
            SocketClientProxy tempSocket = GetSocket(_serverType);
            if (tempSocket == null)
            {
                return;
            }

            tempSocket.SendData(_data, _serviceid, _requestData);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < socketList.Count; i++)
            {
                socketList[i].Clear();
            }
            socketList.Clear();
        }

        /// <summary>
        /// 成功连接会触发这个事件
        /// </summary>
        /// <param name="_socker">连接成功的Socket</param>
        void OnConnected(object _socker)
        {
            SocketTcp tempSocketTcp = _socker as SocketTcp;
            if (tempSocketTcp == null)
            {
                return;
            }

            if (reConnectList != null && reConnectList.Contains(tempSocketTcp.ServerType))
            {
                reConnectList.Remove(tempSocketTcp.ServerType);
            }

            // 发送成功连接事件
            if (onConnected != null)
            {
                onConnected.Call(tempSocketTcp);
            }

            // 登录成功之后，复位断线重连次数
            reconnectNum = 0;
            isConnected = true;
        }

        /// <summary>
        /// 从服务器断开连接会响应这个事件
        /// </summary>
        /// <param name="_socker">断开连接的Socket</param>
        void OnDisconnected(object _socker)
        {
            SocketTcp tempSocketTcp = _socker as SocketTcp;
            if (tempSocketTcp == null)
            {
                return;
            }

            // 非连接状态才会启动断线重连，目前只有在登录的时候才会启动断线重连
            if (!isConnected && reConnectList != null && !reConnectList.Contains(tempSocketTcp.ServerType))
            {
                reConnectList.Add(tempSocketTcp.ServerType);
            }

            if (onDisconnected != null)
            {
                // 通知 Lua 网络断开
                onDisconnected.Call(tempSocketTcp);
            }
        }

        /// <summary>
        /// 连接服务器失败会响应这个事件
        /// </summary>
        /// <param name="_socketTcp">断开连接的Socket</param>
        void OnConnectFailed(object _socket)
        {
            SocketTcp tempSocketTcp = _socket as SocketTcp;
            if (tempSocketTcp == null)
            {
                return;
            }

            if (reConnectList != null && !reConnectList.Contains(tempSocketTcp.ServerType))
            {
                reConnectList.Add(tempSocketTcp.ServerType);
            }

            if (onConnectFailed != null)
            {
                onConnectFailed.Call(tempSocketTcp);
            }
        }

        /// <summary>
        /// 重新连接失败
        /// </summary>
        /// <param name="_serverType">服务器类型</param>
        void ReConnectFail(int _serverType)
        {
            for (int i = 0; i < socketList.Count; i++)
            {
                if (socketList[i].ServerType == _serverType)
                {
                    socketList[i].Clear();
                    socketList.RemoveAt(i);
                    if (onReConnectFailed != null)
                    {
                        onReConnectFailed.Call(_serverType);
                    }
                    break;
                }
            }
        }

        void OnApplicationQuit()
        {
            Clear();
            Debug.LogError("应用程序退出");
        }

        /// <summary>
        /// 取socket实例
        /// </summary>
        SocketClientProxy GetSocket(int _serverType = 1)
        {
            if (socketList == null)
            {
                return null;
            }

            SocketClientProxy tempSocket = socketList.Find(delegate (SocketClientProxy _socket) { return _socket.ServerType == _serverType; });
            if (tempSocket == null)
            {
                tempSocket = new SocketClientProxy(_serverType);
                socketList.Add(tempSocket);
            }

            return tempSocket;
        }
    }
}