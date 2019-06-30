using FluidLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluidLib.Networking
{
    public delegate void tcp_receive(Reader reader, byte PacketID);
    
    public class TCP_Client
    {
        public TcpClient socket { get; private set; }
        public NetworkStream stream { get; private set; }

        public event tcp_receive _receive;

        public byte[] buffer;

        public TCP_Client(string ip, int port)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = 4096,
                SendBufferSize = 4096,
                NoDelay = false,
            };

            buffer = new byte[socket.ReceiveBufferSize];
            socket.BeginConnect(ip, port, OnConnection, socket);
        }

        void OnConnection(IAsyncResult _result)
        {
            try
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                    return;
                else
                {
                    socket.NoDelay = true;
                    stream = socket.GetStream();
                    stream.BeginRead(buffer, 0, socket.ReceiveBufferSize, Receive, null);
                }
            }
            catch (Exception ex) { Logger.Log("Client", "Error on connection: " + ex.Message, LogType.Error); }
        }

        public void Send(Writer writer, byte PacketID)
        {
            byte[] data = writer.ToArray();

            writer = new Writer();
            writer.Write(PacketID);
            writer.Write(data);

            data = writer.ToArray();
            stream.BeginWrite(data, 0, data.Length, null, null);
        }

        void Receive(IAsyncResult _result)
        {
            try
            {
                int length = stream.EndRead(_result);
                if (length <= 0)
                    return;

                Reader reader = new Reader(buffer);
                byte PacketID = reader.ReadByte();

                Console.WriteLine("Handling the data!");
                _receive?.Invoke(reader, PacketID);

                stream.BeginRead(buffer, 0, socket.ReceiveBufferSize, Receive, null);

                reader.Dispose();
            }
            catch (Exception ex) { Logger.Log("Client", "Error on receive data: " + ex.Message, LogType.Error); }
        }
    }
}
