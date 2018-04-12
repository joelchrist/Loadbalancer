using System;
using System.Net.Sockets;
using System.Text;

namespace Loadbalancer
{
    public static class TcpClientExtensions
    {
        public static int BufferSize = 1024;

        public static string ReceiveData(this TcpClient client) 
        {
            while (client.Connected)
            {
                var buffer = new byte[BufferSize];
                string data = "";
                do
                {
                    int read = client.GetStream().Read(buffer, 0, buffer.Length);
                    data += Encoding.ASCII.GetString(buffer, 0, read);
                } while (client.GetStream().DataAvailable);
                return data;
            }
            return null;
        }

        public static void SendData(this TcpClient client, string data, bool shouldClose)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            client.GetStream().Write(bytes, 0, bytes.Length);
            if (shouldClose) {
                client.GetStream().Close();
                client.Close();   
            }
#if DEBUG
            Console.WriteLine("Closed connection with client");
#endif
        }
    }
}
