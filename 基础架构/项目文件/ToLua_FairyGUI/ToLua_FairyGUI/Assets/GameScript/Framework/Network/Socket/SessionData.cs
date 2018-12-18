using LuaInterface;

namespace GameFramework
{
    /// <summary>
    /// 请求会话数据
    /// </summary>
    public class SessionData
    {
        static public readonly byte[] EMPTY = new byte[0];

        public const int NORMAL = 0;    // 会话正常

        public uint errCode = 0;

        uint size;                      // 接收的数据大小
        uint serviceId;                 // 请求类型
        uint callbackId;                // 请求ID
        byte[] sendingData;
        byte[] receiveData;
        object requestData;             // 发送的数据，会原样返回
        ByteBuffer luaSendingData;      // 发送的数据lua
        ByteBuffer luaReceiveData;      // 接收的数据lua

        //发送的数据
        public byte[] SendingData
        {
            get { return sendingData ?? EMPTY; }
            set
            {
                sendingData = value;
                if (sendingData != null)
                {
                    luaSendingData = new ByteBuffer();
                    luaSendingData.WriteBytes(sendingData);
                    luaSendingData.Flush();
                    luaSendingData.ResetPosition();
                    sendingData = null;
                }
            }
        }

        //接收的数据
        public byte[] ReceiveData
        {
            get { return receiveData; }
            set
            {
                receiveData = value;
                if (receiveData != null)
                {
                    luaReceiveData = new ByteBuffer();
                    luaReceiveData.WriteBytes(receiveData);
                    luaReceiveData.Flush();
                    luaReceiveData.ResetPosition();
                    receiveData = null;
                }
            }
        }

        public uint Size { get { return size; } }               
        public uint ServiceID { get { return serviceId; } }
        public uint CallbackID { get { return callbackId; } }
        public object RequestData { get { return requestData; } }
        public ByteBuffer LuaSendingData { get { return luaSendingData; } }
        public ByteBuffer LuaReceiveData { get { return luaReceiveData; } }

        public void Init(uint _callbackId, uint _serviceid, object _requestData)
        {
            size = 0;
            serviceId = _serviceid;
            callbackId = _callbackId;
            requestData = _requestData;

            ReceiveData = null;
        }

        public void InitMsg(byte[] _byteData, uint _size)
        {
            ReceiveData = _byteData ?? EMPTY;//左操作数不为null，则返回左操作数，否则返回右操作数
            size = _size;
        }

        public void Clear()
        {
            if (luaReceiveData != null)
            {
                luaReceiveData.Clear();
            }

            if (luaSendingData != null)
            {
                luaSendingData.Clear();
            }
            SendingData = null;
            luaSendingData = null;
            ReceiveData = null;
            luaReceiveData = null;
        }
    }
}