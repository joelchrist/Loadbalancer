using System;
namespace Loadbalancer
{
    public class BalancerFactory
    {
        public BalancerFactory()
        {
        }

        public static Balancer GetInstance(BalanceMethod method)
        {
            switch (method) {
                case BalanceMethod.Random:
                    return new RandomBalancer();
                default:
                case BalanceMethod.RoundRobin:
                    return new RoundRobinBalancer();
            }
        }
    }
}
