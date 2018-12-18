require "Game/Framework/Network/NetworkType"
require "Game/Framework/Network/NetworkName"
require "Game/Framework/Network/NetworkServiceId"

NetworkManager = {}

local function ReConnect(_serverType)
    GameFramework.NetManager.mInst:ReConnect(_serverType, true)
end

local function OnConnected(_socket)
    logError("服务器类型 --> " .. tostring(_socket.ServerType) .. " <-- 连接成功");
    Dispatcher.Brocast(NetworkName.Connected, _socket);
end

local function OnDisconnected(_socket)
    logError("服务器类型 --> " .. tostring(_socket.ServerType) .. " <-- 断开连接");
    Dispatcher.Brocast(NetworkName.Disconnected, _socket);
end

local function OnConnectFailed(_socket)
    logError("服务器类型 --> " .. tostring(_socket.ServerType) .. " <-- 连接失败    错误信息 -----> " .. tostring(_socket.ErrorMessage));

    Dispatcher.Brocast(NetworkName.ConnectFailed, _socket)
end

local function OnReConnectFailed(_serverType)
    logError("服务器类型 --> " .. tostring(_serverType) .. " <-- 断线重连失败");

    Dispatcher.Brocast(NetworkName.ReConnectFailed, _serverType);
end

-- 缓存转字符串
local function BufferToString(_value)
    if _value == nil then
        return "";
    end

    return tostring(_value);
end

-- 获取 Protobuf 的数据
local function GetEncodeBuffer(_bufferArray)
    local tempValue = "";
    if _bufferArray ~= nil and type(_bufferArray) == "table" then
        tempValue = tempValue .. BufferToString(_bufferArray[1]);
        for i = 2, #_bufferArray do
            tempValue = tempValue .. "Ω" .. BufferToString(_bufferArray[i]);
        end
    end
    
    return protobuf.encode("com.ftkj.proto.MoData", { msg = tempValue; });
end

--[Comment]
-- 连接服务器
-- 服务器类型   ： _serverType
-- 服务器主机IP ： _host
-- 服务器端口   ：_port
function NetworkManager.Connect(_serverType, _host, _port)
    GameFramework.NetManager.mInst:ConnectAsync(_serverType, _host, _port, OnConnected, OnDisconnected, OnConnectFailed, OnReConnectFailed);
end

--[Comment]
-- 连接 Logic 服务器
-- 服务器主机IP ： _host
-- 服务器端口   ：_port
function NetworkManager.ConnectLogic(_host, _port)
    NetworkManager.Connect(NetworkType.Logic, _host, _port)
end

--[Comment]
-- 添加服务器的监听事件
-- 服务器类型 ： _serverType
-- 服务ID     ：_serviceID
-- 回调函数   ：_function
function NetworkManager.AddListener(_serverType, _serviceID, _function)
    if not _function or type(_function) ~= "function" then
		logError("NetworkManager.AddListener _function value error");
        return;
	end

    GameFramework.NetManager.mInst:AddListener(_serverType, _serviceID, _function);
end

--[Comment]
-- 添加Logic服务器的监听事件
-- 服务ID     ：_serviceID
-- 回调函数   ：_function
function NetworkManager.AddLogicListener(_serviceID, _function)
    NetworkManager.AddListener(NetworkType.Logic, _serviceID, _function)
end

--[Comment]
-- 移除服务器的监听事件
-- 服务器类型 ： _serverType
-- 服务ID     ：_serviceID
-- 回调函数   ：_function
function NetworkManager.RemoveListener(_serverType, _serviceID, _function)
    if not _function or type(_function) ~= "function" then
		logError("NetworkManager.RemoveListener _function value error");
        return;
	end

    GameFramework.NetManager.mInst:RemoveListener(_serverType, _serviceID, _function);
end

--[Comment]
-- 移除 Logic 服务器的监听事件
-- 服务ID     ：_serviceID
-- 回调函数   ：_function
function NetworkManager.RemoveLogicListener(_serviceID, _function)
    NetworkManager.RemoveListener(NetworkType.Logic, _serviceID, _function)
end

--[Comment]
-- 断开服务器
-- 服务器类型 ： _serverType
function NetworkManager.Disconnected(_serverType)
    GameFramework.NetManager.mInst:OnCloseAndClearTcp(_serverType);
end

--[Comment]
-- 断开逻辑服务器
function NetworkManager.DisconnectedLogin()
    GameFramework.NetManager.mInst:OnCloseAndClearTcp(NetworkType.Logic);
end

--[Comment]
-- 发送数据
function NetworkManager.SendData(_serverType, _serviceID, _bufferArray)
    if GameFramework.NetManager == nil or GameFramework.NetManager.mInst == nil then
        logError("SendData Error");
        return;
    end
    log("Request SID: " .. _serviceID);
    GameFramework.NetManager.mInst:SendData(_serverType, LuaStringBuffer(GetEncodeBuffer(_bufferArray)).buffer, _serviceID, _bufferArray);
end

--[Comment]
-- 发送 Logic 数据
-- _serviceID   : 协议ID
-- _bufferArray ： 缓存数组，格式为：{ value， value }，为 nil 时默认发送空字符串
function NetworkManager.SendLogicData(_serviceID, _bufferArray)
    NetworkManager.SendData(NetworkType.Logic, _serviceID, _bufferArray)
end

--[Comment]
-- 服务器是否连接
function NetworkManager.IsConnected(_networkType)
    return GameFramework.NetManager.mInst:IsConnected(_networkType, false);
end

--[Comment]
-- 逻辑服务器是否连接
function NetworkManager.IsLogicConnected()
    return NetworkManager.IsConnected(NetworkType.Logic)
end

--[Comment]
-- 解析默认的 Buffer
function ReadDefaultBuffer(_serverType, _sessionData)
    local tempBuffer = ReadBuffer("DefaultData", _sessionData);
    if tempBuffer.code ~= 0 then
        local tempType = ErrorCodeParse.GetTargetType(tempBuffer.bigNum);
        if tempType == 1 then
            Dispatcher.Brocast(EventName.ShowErrorTitleBox, tempBuffer.code);
        else
            Dispatcher.Brocast(EventName.ShowErrorMessageBox, tempBuffer.code);
        end
        
        return tempBuffer;
    end

    return tempBuffer;
end

--[Comment]
-- 解析默认的 Buffer
function IsNormalReadDefaultBuffer(_serverType, _sessionData)
    local tempBuffer = ReadBuffer("DefaultData", _sessionData);
    if tempBuffer.code ~= 0 then
        local tempType = ErrorCodeParse.GetTargetType(tempBuffer.bigNum);
        if tempType == 1 then
            Dispatcher.Brocast(EventName.ShowErrorTitleBox, tempBuffer.code);
        else
            Dispatcher.Brocast(EventName.ShowErrorMessageBox, tempBuffer.code);
        end
        return false;
    end

    return true;
end