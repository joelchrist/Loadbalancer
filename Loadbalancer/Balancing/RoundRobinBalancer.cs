using System;
namespace Loadbalancer
{
    public class RoundRobinBalancer : Balancer
    {
        private static int index = 0;

        public Server GetServer()
        {
            index = index != Config.Servers.Count - 1 ? index + 1 : 0;
            Console.WriteLine("Using server with index {0}", index);
            return Config.Servers[index];
        }
    }
}
