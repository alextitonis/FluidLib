using System.IO;

namespace Multiplayer_Game_Library
{
    public class Writer
    {
        BinaryWriter writer;
        MemoryStream stream;

        public Writer()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void Write(int value) { writer.Write(value); }
        public void Write(float value) { writer.Write(value); }
        public void Write(string value) { writer.Write(value); }
        public void Write(char value) { writer.Write(value); }
        public void Write(short value) { writer.Write(value); }
        public void Write(ushort value) { writer.Write(value); }
        public void Write(uint value) { writer.Write(value); }
        public void Write(long value) { writer.Write(value); }
        public void Write(ulong value) { writer.Write(value); }
        public void Write(double value) { writer.Write(value); }
        public void Write(byte value) { writer.Write(value); }
        public void Write(byte[] value)
        {
            writer.Write(value.Length);
            writer.Write(value);
        }
        public void Write(sbyte value) { writer.Write(value); }
        public void Write(bool value) { writer.Write(value); }

        public byte[] ToArray(bool dispose = true)
        {
            byte[] data = stream.ToArray();

            if (dispose)
                Dispose();

            return data;
        }
        public void Dispose()
        {
            writer.Close();
            stream.Close();

            writer.Dispose();
            stream.Dispose();
        }
    }
}
