using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Loadbalancer
{
    public class SessionHandler : PersistenceHandler
    {
        private const string COOKIE_HEADER = "Set-Cookie: LB={0}";
        private const string COOKIE_ID = "LB";

        private static int identifier = 0;

        private static IDictionary<int, Server> sessionTable = new Dictionary<int, Server>();

        public string AddPersistence(string res, Server server)
        {
            var split = res.Split(new char[] { '\r', '\n' });
            split[0] = split[0] + "\n" + String.Format(COOKIE_HEADER, identifier);
            sessionTable.Add(identifier, server);
            identifier++;
            return string.Join("\r\n", split);
        }

        public Server GetServer(string req)
        {
            var cookies = getCookies(req);
            var cookie = cookies.First(c => c.Contains(COOKIE_ID));
            var value = cookie.Split('=')[1];
            Server server;
            sessionTable.TryGetValue(int.Parse(value), out server);
            return server;

        }

        public bool HasPersistence(string req)
        {
            var cookies = getCookies(req);
            return cookies.Any(cookie => cookie.Contains(COOKIE_ID));


        }

        private string[] getCookies(string request)
        {
            return Regex.Match(request, "(?i)(?<=Cookie: )(?-i)(.*?)(?=\r\n)").Value.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

        }
    }
}
