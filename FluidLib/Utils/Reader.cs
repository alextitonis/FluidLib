using System.IO;

namespace Multiplayer_Game_Library
{
    public class Reader
    {
        BinaryReader reader;
        MemoryStream stream;

        public Reader(byte[] data)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }

        public int ReadInt() { return reader.ReadInt32(); }
        public string ReadString() { return reader.ReadString(); }
        public char ReadChar() { return reader.ReadChar(); }
        public float ReadFloat() { return reader.ReadSingle(); }
        public  short ReadShort() { return reader.ReadInt16(); }
        public ushort ReadUShort() { return reader.ReadUInt16(); }
        public uint ReadUInt() { return reader.ReadUInt32(); }
        public long ReadLong() { return reader.ReadInt64(); }
        public ulong ReadULong() { return reader.ReadUInt64(); }
        public double ReadDouble() { return reader.ReadDouble(); }
        public byte ReadByte() { return reader.ReadByte(); }
        public byte[] ReadBytes()
        {
            int count = ReadInt();
            return reader.ReadBytes(count);
        }
        public sbyte ReadSByte() { return reader.ReadSByte(); }
        public bool ReadBool() { return reader.ReadBoolean(); }

        public void Dispose()
        {
            reader.Close();
            stream.Close();

            reader.Dispose();
            stream.Dispose();
        }
    }
}
