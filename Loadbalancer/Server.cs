using System;
using System.Net;

namespace Loadbalancer
{
    public struct Server
    {
        public string Url;
        public int Port;
        public bool IsAvailable {
            get {
                return GetAvailibility();
            }
        }

        public Server(string url, int port) {
            this.Url = url;
            this.Port = port;
        }

        private bool GetAvailibility() 
        {
            try
            {
                HttpWebRequest httpRequest = WebRequest.CreateHttp(string.Concat("http://", Url, ":", Port));
                httpRequest.Method = "HEAD";
                httpRequest.Timeout = Config.Timeout;
                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                httpResponse?.Close();
                return true;
            }
            catch (WebException)
            {
                // exception is thrown when the server is offline or time out has exceeded
                return false;
            }
        }
    }
}