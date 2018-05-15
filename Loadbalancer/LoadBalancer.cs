using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.Http;
using System.Text;

namespace Loadbalancer
{
    public class LoadBalancer
    {
        private const string MESSAGE_MARKER = "\0";
        private const string CHUNK_MARKER = "0\r\n\r\n";

        private TcpListener listener;
        private bool listening = false;

        private Thread dataThread;

        public LoadBalancer()
        {
        }

        public void Start()
        {
            listener = new TcpListener(IPAddress.Parse(Config.Ip), Config.Port);
            this.listener.Start();
            this.listening = true;
            Console.WriteLine("Start listening for clients on {0}:{1}", Config.Ip, Config.Port);
            AcceptClients();
        }

        public void Stop() {
            // needs to be wrapped in a while loop in case ther are still open connections
            while (true)
            {
                // when ther are still open connections then wait before stopping the listener
                if (listener.Pending()) continue;
                listening = false;
                listener.Stop();
                break;
            }
        }

        private async void AcceptClients()
        {
            while (listening)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Established connection with client");
                    dataThread = new Thread(() => HandleClient(client));
                    dataThread.Start();
                }
                catch (ObjectDisposedException)
                {
                    // exception has to be thrown when TcpListener stops, AcceptTcpClientAsync doesn't take cancellation token
                    Console.WriteLine(string.Concat("Stopped listening for clients on ", Config.Ip, ":", Config.Port));
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            Server server;
            do
            {
                server = BalancerFactory.GetInstance(Config.BalanceMethod).GetServer();
            } while (!server.IsAvailable);

            var persistenceHandler = PersistenceHandlerFactory.GetInstance(Config.PersistenceMethod);

            var req = client.ReceiveData();

			if (persistenceHandler.HasPersistence(req)) {
			    server = persistenceHandler.GetServer(req);
			}

            
			Console.WriteLine("Getting data from server, {0}:{1}", server.Url, server.Port);
			if (!server.IsAvailable) {
				var response = CreateServerErrorStatusCode();
				client.SendData(response, true);
				return;
			}
			var res = RetreiveDataFromServer(req, server);
			
            if (!persistenceHandler.HasPersistence(req)) {
                res = persistenceHandler.AddPersistence(res, server);
            }


            client.SendData(res, true);
        }

        private string RetreiveDataFromServer(string request, Server server)
        {
            TcpClient client = new TcpClient(server.Url, server.Port);
            client.SendData(request, false);
            string marker = MESSAGE_MARKER;
            var buffer = new byte[Config.Buffersize];
            string data = "";
            do
            {
                int read = client.GetStream().Read(buffer, 0, buffer.Length);
                data += Encoding.ASCII.GetString(buffer, 0, read);
                // when data is transfered chunked then use a different marker
                if (data.Contains("Transfer-Encoding: chunked")) marker = CHUNK_MARKER;
            } while (client.GetStream().DataAvailable || !data.Contains(marker));
            client.GetStream().Close();
            client.Close();
            return data;
        }

		private static string CreateServerErrorStatusCode()
        {
            return "HTTP/1.1 504 Service Unavailable\r\nContent-Type: text/html\r\n\r\n" +
                   "<p>503 Serivce Unavailable</p><p>Upstream server is temporarily unavailible, please try again later.</p>";
        }

    }
}