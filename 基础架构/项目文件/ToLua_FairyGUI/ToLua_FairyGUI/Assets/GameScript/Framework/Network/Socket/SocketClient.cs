using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Libs.zlib;

namespace GameFramework
{
    /// <summary>
    /// socekt通讯代理
    /// </summary>
    public class SocketClient : SocketTcp
    {
        // -------------------- 协议头变更 --------------------
        //const int DATA_LEN = 4;             // 消息长度(字节)
        //const int DATA_TOKEN_LEN = 2;       // token(字节)
        //const int DATA_MSGID_LEN = 4;       // msgid(字节)
        //const int DATA_CALLBACK_ID_LEN = 4; // 回调id
        //const int DATA_ERRID_LEN = 4;

        //消息头,(字节)
        //const int SEND_DATA_HEAD_LEN = DATA_TOKEN_LEN + DATA_MSGID_LEN + DATA_CALLBACK_ID_LEN;
        //const int RECV_DATA_HEAD_LEN = DATA_LEN + DATA_MSGID_LEN + DATA_CALLBACK_ID_LEN + DATA_ERRID_LEN;
        // ----------------------- End ------------------------

        const int DATA_LEN = 4;                 // 消息总长度
        const int REQ_ID_LEN = 4;               // ReqId
        const int SERVICE_ID_LEN = 4;           // serviceId
        const int IS_COMPRESS_LEN = 1;          // 压缩信息字段
        const int DATA_HEAD_LEN = DATA_LEN + REQ_ID_LEN + SERVICE_ID_LEN + IS_COMPRESS_LEN;
        const int DATA_HEAD_NOCOMP_LEN = DATA_LEN + REQ_ID_LEN + SERVICE_ID_LEN;

        protected uint package_errCode = 0;
        protected uint package_size { get; private set; }
        protected uint package_service { get; private set; }
        protected uint package_callbackId { get; private set; }
        private bool ifCompress = true;         // 是否启用压缩解析
        private int dataHeadLen;
        
        Int32 token = -1;       // 自增长并复原 
        Int32 callbackId = 1;   // 自增长并复原 

        byte[] cachedByteData;
        byte[] uint32Byte = new byte[4];
        byte[] uint16Byte = new byte[2];
        byte[] bool8Byte = new byte[1];

        // 沙箱操作
        byte[] tempData;                // 沙箱数据
        bool inSandbox = true;          // 是否要接收沙箱文件
        bool hasSandbox = false;        // 是否接收水箱文件
        ushort sandboxFileLength = 111; // 沙箱文件字节数
        // end
        
        public SocketClient(int _serverType = 1) : base(_serverType) { }

        public override void Connect()
        {
            if (IsConnected())
            {
                return;
            }

            if (inSandbox)
            {
                hasSandbox = false;
            }

            token = 0;
            base.Connect();
        }

        public override void Connect(string _host, int _port)
        {
            if (IsConnected())
            {
                return;
            }

            if (inSandbox)
            {
                hasSandbox = false;
            }

            token = 0;
            base.Connect(_host, _port);
        }

        //public override void ConnectAsync(string _host, int _port, NetCallback _onConnected, NetCallback _onDisconnected, NetCallback _onConnectFailed)
        //{
        //    if (IsConnected())
        //    {
        //        return;
        //    }

        //    if (inSandbox)
        //    {
        //        hasSandbox = false;
        //    }

        //    base.ConnectAsync(_host, _port, _onConnected, _onDisconnected, _onConnectFailed);
        //}

        //包长度4 + messid 4 + callbackid 4 + errcode 4
        protected override void DoReceive(byte[] _inData, int _len)
        {
            byte[] allByteData;
            bool isCompress = false;

            if (ifCompress)
            {
                dataHeadLen = (int)DATA_HEAD_LEN;
            }
            else
            {
                dataHeadLen = (int)DATA_HEAD_NOCOMP_LEN;
            }

            if (cachedByteData != null)
            {
                //合并断包
                allByteData = new byte[cachedByteData.Length + _len];

                _len = allByteData.Length;

                Buffer.BlockCopy(cachedByteData, 0, allByteData, 0, cachedByteData.Length);
                Buffer.BlockCopy(_inData, 0, allByteData, cachedByteData.Length, _inData.Length);

                cachedByteData = null;
            }
            else
            {
                allByteData = _inData;
            }

            //// 排除沙箱文件
            //if (SecurityPolicy(allByteData, allByteData.Length))
            //{
            //    return;
            //}

            if (_len < dataHeadLen)
            {
                //包不完整， 不满足最小包大小
                cachedByteData = allByteData;
                return;
            }

            int index = 0;
            package_size = ReadUInt32(allByteData, index);//读取大小
            index += DATA_LEN;

            package_callbackId = ReadUInt32(allByteData, index);
            index += REQ_ID_LEN;

            package_service = ReadUInt32(allByteData, index);//读取请求ID
            if (ifCompress)
            {
                index += SERVICE_ID_LEN;
                isCompress = ReadBoolean(allByteData, index);
            }
           
            int leftDataLenth = 0;
            leftDataLenth = _len - dataHeadLen;

            byte[] packageData = null;
            if (package_size > 0 && leftDataLenth >= package_size)
            {
                packageData = new byte[package_size];//第一个包

                Buffer.BlockCopy(allByteData, dataHeadLen, packageData, 0, packageData.Length);
            }

            if (leftDataLenth == package_size)
            {
                //正常情况, 数据不多也不少
                if (ifCompress && isCompress)
                {
                    //UnityEngine.Debug.LogError(isCompress.ToString() + "--" + package_service + "--" + package_size + "--" + package_callbackId);
                    packageData = DecompressPacket(packageData);
                }
                DecodePackage(packageData);
            }
            else if (leftDataLenth < package_size)
            {
                //断包保存
                cachedByteData = allByteData;
            }
            else if (leftDataLenth > package_size)
            {
                //粘包分解
                if (ifCompress && isCompress)
                {
                    //UnityEngine.Debug.LogError(isCompress.ToString() + "--" + package_service + "--" + package_size + "--" + package_callbackId);
                    packageData = DecompressPacket(packageData);
                }
                DecodePackage(packageData);

                //粘包处理
                byte[] lessData = new byte[leftDataLenth - package_size];
                Buffer.BlockCopy(allByteData, (int)(dataHeadLen + package_size), lessData, 0, lessData.Length);

                //继续读取后面的数据
                DoReceive(lessData, lessData.Length);
            }
        }

        UInt32 ReadUInt32(byte[] _bs, int _offset)
        {
            Buffer.BlockCopy(_bs, _offset, uint32Byte, 0, uint32Byte.Length);
            Array.Reverse(uint32Byte);

            return BitConverter.ToUInt32(uint32Byte, 0);
        }

        UInt16 ReadUInt16(byte[] _bs, int _offset)
        {
            Buffer.BlockCopy(_bs, _offset, uint16Byte, 0, uint16Byte.Length);
            Array.Reverse(uint16Byte);

            return BitConverter.ToUInt16(uint16Byte, 0);
        }

        bool ReadBoolean (byte[] _bs, int _offset) {
            Buffer.BlockCopy(_bs, _offset, bool8Byte, 0, bool8Byte.Length);

            return BitConverter.ToBoolean(bool8Byte, 0);
        }

        /**
         * 数据解压缩
         * _srcArr byte[]
         * _isCompress bool
         */
        protected byte[] DecompressPacket (byte[] _srcArr) 
        {
            byte[] rtnArr;

            byte[] tmpArr;
            MemoryStream mInStream;
            MemoryStream mOutStream;
            ZOutputStream zouStream;

            tmpArr = new byte[_srcArr.Length];
            mInStream = new MemoryStream();
            mOutStream = new MemoryStream();

            Buffer.BlockCopy(_srcArr, 0, tmpArr, 0, _srcArr.Length);
            mInStream.Write(tmpArr, 0, tmpArr.Length);
            mInStream.Position = 0;
            zouStream = new ZOutputStream(mOutStream);
            try
            {
                CopyStream(mInStream, zouStream);
                rtnArr = new byte[mOutStream.Length];
                mOutStream.Position = 0;
                mOutStream.Read(rtnArr, 0, rtnArr.Length);
            }
            finally
            {
                mInStream.Close();
                zouStream.Close();
                mOutStream.Close();
            }

            return rtnArr;
        }

        protected void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[1024];
            int len;
            while ((len = input.Read(buffer, 0, 1024)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="_dataArr">去除协议头的业务数据</param>
        protected virtual void DecodePackage(byte[] _dataArr){}

        public virtual bool Send(byte[] _data, uint _service)
        {
            if (IsConnected())
            {
                Send(_data, (int)_service);
                return true;
            }

            OnDisConnect();
            return false;
        }

        public virtual int Send(byte[] _data, int _service)
        {
            callbackId++;
            if (callbackId >= Int32.MaxValue)
            {
                callbackId = 1;
            }

            base.Send(WrapHead(_data, _service, callbackId));

            return callbackId;
        }

        protected virtual byte[] WrapHead(byte[] _data, int _service, int _callbackid)
        {
            int dataLength = _data != null ? _data.Length : 0;
            byte[] sendByteData = new byte[dataLength + DATA_HEAD_NOCOMP_LEN];

            uint index = 0;
            IntSaveToBytes(sendByteData, index, dataLength, DATA_LEN);          // 大小

            index += REQ_ID_LEN;
            IntSaveToBytes(sendByteData, index, _callbackid, SERVICE_ID_LEN);   // callback

            index += DATA_LEN;
            IntSaveToBytes(sendByteData, index, _service, REQ_ID_LEN);          // msgid  

            if (_data != null)
            {
                Buffer.BlockCopy(_data, 0, sendByteData, DATA_HEAD_NOCOMP_LEN, dataLength);
            }

            return sendByteData;
        }

        /// <summary>
        /// 整数保存在数组bs
        /// </summary>
        /// <param name="_bs">保存在字节数组</param>
        /// <param name="_offset">保存位置偏移</param>
        /// <param name="_data">数据,仅支持正整数</param>
        /// <param name="_byteNum">保存数据字节数,超过的掉弃</param>
        static void IntSaveToBytes(byte[] _bs, uint _offset, Int32 _data, int _byteNum)
        {
            int count = 0;
            while (_byteNum > count)
            {
                _bs[_offset + count] = (byte)(_data >> ((_byteNum - count - 1) * 8));

                count++;
            }
        }

        public override void Clear()
        {
            cachedByteData = null;
            base.Clear();
        }


        /**保存沙箱信息*/
        protected bool SecurityPolicy(byte[] allData, int len)
        {
            if (!inSandbox)
            {
                return false;
            }

            if (hasSandbox) return false;

            if (len < sandboxFileLength)
            {
                tempData = allData;
            }
            else
            {
                string receiveData = Encoding.ASCII.GetString(allData);
                if (receiveData.IndexOf("<?xml version='1.0'?>") > -1)
                {
                    hasSandbox = true;
                }
                else
                {
                    UnityEngine.Debug.LogError("SocketClient.SecurityPolicy(),沙箱文件错误.");
                }

                // 沙箱文件和普通包粘了
                if (len > sandboxFileLength) 
                {
                    byte[] lessData = new byte[len - sandboxFileLength];
                    Buffer.BlockCopy(allData, sandboxFileLength, lessData, 0, lessData.Length);
                    DoReceive(lessData, lessData.Length);
                }
            }
            return true;
        }
    }
}