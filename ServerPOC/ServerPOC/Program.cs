using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerPOC
{
    class LineBufferedClient
    {
        public LineBufferedClient(TcpClient client) {
            ReadBuffer = new byte[256];
            CurrentLine = new StringBuilder();
            Client = client;
        }

        public TcpClient Client { get; private set; }
        public Byte[] ReadBuffer { get; private set; }
        public StringBuilder CurrentLine { get; set; }
    }

    class ClientManager
    {
        ConcurrentDictionary<LineBufferedClient, LineBufferedClient> _clients = new ConcurrentDictionary<LineBufferedClient, LineBufferedClient>();

        public void Add(TcpClient tcpClient)
        {
            var client = new LineBufferedClient(tcpClient);

            var result = tcpClient.GetStream().BeginRead(client.ReadBuffer, 0, client.ReadBuffer.Length, DataReceived, client);

            if (!_clients.TryAdd(client, client))
            {
                throw new InvalidOperationException("Tried to add connection twice");
            }
        }

        private void HandleCompleteLine(LineBufferedClient client, string line)
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

        private void DataReceived(IAsyncResult ar)
        {
            var client = ar.AsyncState as LineBufferedClient;

            var bytesRead = client.Client.GetStream().EndRead(ar);

            if(bytesRead > 0)
            {
                var readString = Encoding.UTF8.GetString(client.ReadBuffer, 0, bytesRead);

                while(readString.Contains("\n"))
                {
                    var indexOfNewLine = readString.IndexOf('\n');
                    var left = readString.Substring(0, indexOfNewLine);
                    client.CurrentLine.Append(left);

                    var line = client.CurrentLine.ToString();

                    client.CurrentLine.Clear();
                    if(indexOfNewLine != readString.Length-1)
                    {
                        readString = readString.Substring(indexOfNewLine + 1);
                    }
                    else
                    {
                        readString = string.Empty;
                    }

                    HandleCompleteLine(client, line);
                }

                if(!string.IsNullOrEmpty(readString))
                {
                    client.CurrentLine.Append(readString);
                }

                try
                {
                    client.Client.GetStream().BeginRead(client.ReadBuffer, 0, 256, DataReceived, client);
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is System.IO.IOException)
                {
                    RemoveClient(client);
                }
            }
            else
            {
                RemoveClient(client);
            }
        }

        private void RemoveClient(LineBufferedClient client)
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
