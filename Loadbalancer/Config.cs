using System;
using System.Collections.Generic;

namespace Loadbalancer
{
    public static class Config
    {
        public static BalanceMethod BalanceMethod;
        public static PersistenceMethod PersistenceMethod;
        public static List<Server> Servers = new List<Server>() { new Server("localhost", 8081), new Server("localhost", 8082), new Server("localhost", 8083), new Server("localhost", 8084) };

        public static string Ip = "127.0.0.1";
        public static int Port = 8080;

        public static int Buffersize = 1024;

        public static int Timeout = 7500;
    }
}
