using System;
using AppKit;

namespace Loadbalancer
{
	public class ServerDataSource : NSTableViewDataSource
    {
        public ServerDataSource()
        {
        }

		public override nint GetRowCount(NSTableView tableView)
        {
            return Config.Servers.Count;
        }

		public void DeleteServer(int index) 
		{
			Config.Servers.RemoveAt(index);
		}
    }
}
