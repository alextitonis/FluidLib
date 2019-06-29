using FluidLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static FluidLib.Networking.TCP_Server;

namespace FluidLib.Networking
{
    public delegate void tcp_clientConnected(Client client);
    public delegate void clockTick();

    public class TCP_Server
    {
        public class Client
        {
            public delegate void tcp_receive(Client sender, Reader reader, byte PacketID);

            public string ID { get; private set; }
            public bool isConnected { get; private set; }
            public TcpClient socket { get; private set; }
            public NetworkStream stream { get; private set; }
            public event tcp_receive _receive;

            byte[] buffer;

            public Client(string ID, TcpClient socket)
            {
                isConnected = true;

                socket.ReceiveBufferSize = 4096;
                socket.SendBufferSize = 4096;

                stream = socket.GetStream();
                buffer = new byte[socket.ReceiveBufferSize];
                stream.BeginRead(buffer, 0, socket.ReceiveBufferSize, Receive, null);
            }
            public void Close()
            {
                isConnected = false;

                socket.Close();
                stream.Close();

                socket = null;
                stream = null;
            }

            void Receive(IAsyncResult _result)
            {
                try
                {
                    int length = stream.EndRead(_result);
                    if (length <= 0)
                    {
                        Logger.Log("Server", "Received data length is <= 0!", LogType.Warning);
                        return;
                    }

                    Reader reader = new Reader(buffer);
                    byte PacketID = reader.ReadByte();

                    _receive?.Invoke(this, reader, PacketID);

                    stream.BeginRead(buffer, 0, 4096, Receive, null);

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Log("Server", "Error while receiving data: " + ex, LogType.Info);
                    return;
                }
            }
        }

        TcpListener socket;
        int port = 2106;
        long msPerTick;

        public event tcp_clientConnected _clientConnected;
        public event clockTick _clockTick;
        public Thread clock { get; private set; }
        public bool running { get; private set; }

        List<Client> clients = new List<Client>();
        bool idExists(string ID)
        {
            foreach (var i in clients)
            {
                if (i.ID == ID)
                    return true;
            }

            return false;
        }

        public TCP_Server(int port, long msPerTick = 30)
        {
            this.port = port;
            this.msPerTick = msPerTick;

            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);

            clock = new Thread(new ThreadStart(clockThread));
            clock.Start();

            running = true;
        }

        void ClientConnected(IAsyncResult _result)
        {
            TcpClient _client = socket.EndAcceptTcpClient(_result);
            _client.NoDelay = false;

            string _id = Utils.GenerateRandomString(4);
            while (idExists(_id))
                _id = Utils.GenerateRandomString(4);

            Client client = new Client(_id, _client);
            clients.Add(client);

            _clientConnected?.Invoke(client);

            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
        }
        void clockThread()
        {
            if (!running)
            {
                clock.Abort();
                return;
            }

            DateTime _lastLoop = DateTime.Now;
            DateTime _nextLoop = _lastLoop.AddMilliseconds(msPerTick);

            while (running)
            {
                while (_nextLoop < DateTime.Now)
                {
                    _lastLoop = _nextLoop;
                    _nextLoop = _nextLoop.AddMilliseconds(msPerTick);

                    _clockTick?.Invoke();

                    if (_nextLoop > DateTime.Now)
                        Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }

        public void Send(Client client, Writer writer, byte PacketID)
        {
            try
            {
                if (client == null)
                    return;

                //      byte[] data = writer.ToArray();

                //      writer = new Writer();
                //      writer.Write(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);

                //      writer.Write(data);
                //      writer.Write(PacketID);

                byte[] data = writer.ToArray();

                writer = new Writer();
                writer.Write(PacketID);
                writer.Write(data);

                data = writer.ToArray();
                client.stream.BeginWrite(data, 0, data.Length, null, null);
            }
            catch (Exception ex) { Logger.Log("Server", "Error on sendind data to client with ID: " + client.ID + " with error: " + ex.Message, LogType.Error); }
        }
    }
}