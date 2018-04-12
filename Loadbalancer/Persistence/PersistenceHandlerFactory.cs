using System;
namespace Loadbalancer 
{
    public static class PersistenceHandlerFactory
    {
        public static PersistenceHandler GetInstance(PersistenceMethod method) {
            switch (method)
            {
                case PersistenceMethod.CookieBased:
                    return new CookieHandler();
                default:
                case PersistenceMethod.SessionBased:
                    return new SessionHandler();
            }
        }
    }
}
