using System;
namespace Loadbalancer
{
    public class RandomBalancer : Balancer
    {
        private static readonly Random random = new Random();

        public Server GetServer()
        {
            return Config.Servers[random.Next(Config.Servers.Count)];
        }
    }
}
