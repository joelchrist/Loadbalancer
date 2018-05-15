using System;
namespace Loadbalancer
{
    public class RoundRobinBalancer : Balancer
    {
        private static int index = 0;
        
        public Server GetServer()
        {
			if (index > Config.Servers.Count) {
				index = 0;
			}
            index = index != Config.Servers.Count - 1 ? index + 1 : 0;
            return Config.Servers[index];
        }
    }
}
