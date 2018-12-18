using System;
using System.IO;
using System.Text;

namespace GameFramework
{
    public class ByteBuffer
    {
        MemoryStream stream = null;
        BinaryWriter writer = null;
        BinaryReader reader = null;

        public ByteBuffer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }

        public ByteBuffer(byte[] data)
        {
            if (data != null)
            {
                stream = new MemoryStream(data);
                reader = new BinaryReader(stream);
            }
            else
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }
        }

        public void Close()
        {
            if (writer != null)
            {
                writer.Close();
            }

            if (reader != null)
            {
                reader.Close();
            }

            if (stream != null)
            {
                stream.Close();
            }

            writer = null;
            reader = null;
            stream = null;
        }

        public void WriteByte(byte _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Write(_v);
        }

        public void WriteInt(int _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Write(_v);
        }

        public void WriteShort(ushort _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Write(_v);
        }

        public void WriteLong(long _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Write(_v);
        }

        public void WriteFloat(float _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            byte[] temp = BitConverter.GetBytes(_v);
            Array.Reverse(temp);

            writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            byte[] temp = BitConverter.GetBytes(_v);
            Array.Reverse(temp);

            writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(_v);
            writer.Write((ushort)bytes.Length);
            writer.Write(bytes);
        }

        public void WriteBytes(byte[] _v)
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Write(_v.Length);
            writer.Write(_v);
        }

        public float ReadFloat()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
            Array.Reverse(temp);

            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
            Array.Reverse(temp);

            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            ushort len = ReadShort();
            byte[] buffer = new byte[len];
            buffer = reader.ReadBytes(len);

            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ToBytes()
        {
            if (stream == null)
            {
                throw new Exception("stream value or null");
            }

            Flush();
            return stream.ToArray();
        }

        /// <summary>
        /// 清理当前编写器的所有缓冲区，使所有的缓冲数据写入基础设备
        /// </summary>
        public void Flush()
        {
            if (writer == null)
            {
                throw new Exception("writer value or null");
            }

            writer.Flush();
        }

        public void Clear()
        {
            if (stream == null)
            {
                throw new Exception("stream value or null");
            }

            stream.Position = 0;
            stream.SetLength(0);
        }

        public void ResetPosition()
        {
            if (stream == null)
            {
                throw new Exception("stream value or null");
            }

            stream.Position = 0;
        }

        public byte ReadByte()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.ReadByte();
        }

        public int ReadInt()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.ReadInt32();
        }

        public long ReadLong()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.ReadInt64();
        }

        public ushort ReadShort()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return (ushort)reader.ReadInt16();
        }

        public byte[] ReadBytes()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.ReadBytes(ReadInt());
        }

        public byte[] ReadBytes(int _len)
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.ReadBytes(_len);
        }

        public long ReadableLen()
        {
            if (reader == null)
            {
                throw new Exception("reader value or null");
            }

            return reader.BaseStream.Length - reader.BaseStream.Position;
        }

        public LuaInterface.LuaByteBuffer ReadBuffer() { return new LuaInterface.LuaByteBuffer(ReadBytes()); }
    }
}