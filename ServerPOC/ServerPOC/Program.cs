using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerPOC
{
    class LineBufferedClient
    {
        public LineBufferedClient(TcpClient client, ClientManager clientManager) {
            ReadBuffer = new byte[256];
            CurrentLine = new StringBuilder();
            Client = client;
            Stream = new StreamReader(client.GetStream());
            ClientManager = clientManager;            
        }

        public TcpClient Client { get; private set; }
        public Byte[] ReadBuffer { get; private set; }
        public StringBuilder CurrentLine { get; set; }
        public StreamReader Stream { get; set; }
        public ClientManager ClientManager { get; set; }

        public void HandleLine(Task<string> input)
        {
            try
            {
                var received = input.Result;
                if (null == received)
                {
                    Console.WriteLine("Disconnected");
                    ClientManager.RemoveClient(this);
                }
                else
                {
                    ClientManager.SendToAllBut(this, received);
                    Stream.ReadLineAsync().ContinueWith(HandleLine);
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Disconnected!");
                ClientManager.RemoveClient(this);
            }
        }
    }

    class ClientManager
    {
        ConcurrentDictionary<LineBufferedClient, LineBufferedClient> _clients = new ConcurrentDictionary<LineBufferedClient, LineBufferedClient>();

        public void Add(TcpClient tcpClient)
        {
            var client = new LineBufferedClient(tcpClient, this);

            if (!_clients.TryAdd(client, client))
            {
                throw new InvalidOperationException("Tried to add connection twice");
            }

            client.Stream.ReadLineAsync().ContinueWith(client.HandleLine);
        }

        public void SendToAllBut(LineBufferedClient client, string line)
        {
            Console.WriteLine(line);
            Thread.Sleep(2000);
            var buffer = Encoding.ASCII.GetBytes(line + "\n");
            foreach(var entry in _clients)
            {
                var connectedClient = entry.Value;
                if (connectedClient != client)
                {
                    try
                    {
                        connectedClient.Client.GetStream().Write(buffer, 0, buffer.Length);
                    }
                    catch(Exception ex) when (ex is InvalidOperationException || ex is System.IO.IOException)
                    {
                        RemoveClient(connectedClient);
                    }
                }
            }
        }


        public void RemoveClient(LineBufferedClient client)
        {
            LineBufferedClient ignored;
            _clients.TryRemove(client, out ignored);
        }
    }

    class Server
    {
        CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _shutdown = false;
        int _serverPort=0;
        private Thread _listenerThread;
        private ClientManager _clientManager;

        public Server(ClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public void Run(int serverPort)
        {
            _serverPort = serverPort;
            _listenerThread = new Thread(ListenLoop);
            _listenerThread.Start();
        }

        public void ListenLoop()
        {        
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, _serverPort));
            listener.Start();

            while (!_shutdown)
            {
                try
                {
                    var acceptTask = listener.AcceptTcpClientAsync();

                    acceptTask.Wait(_cts.Token);

                    var newClient = acceptTask.Result;

                    _clientManager.Add(newClient);
                }
                catch (OperationCanceledException)
                {
                    // NOP - Shutting down
                }
            }
        }

        public void Stop()
        {
            _shutdown = true;
            _cts.Cancel();
            _listenerThread.Join();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var clientManager = new ClientManager();
            var server = new Server(clientManager);
            server.Run(4040);

            Console.WriteLine("Server running, press Enter to quit.");
            Console.ReadLine();

            server.Stop();
        }
    }
}
