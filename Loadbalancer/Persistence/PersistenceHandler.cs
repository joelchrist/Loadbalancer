using System;
namespace Loadbalancer
{
    public interface PersistenceHandler
    {
        bool HasPersistence(string req);
        Server GetServer(string req);
        string AddPersistence(string res, Server server);
    }
}
