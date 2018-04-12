using System;
namespace Loadbalancer
{
    public interface Balancer
    {
        Server GetServer();
    }
}
