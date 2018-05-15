using System;
using AppKit;

namespace Loadbalancer
{
	public class ServerDelegate : NSTableViewDelegate
    {
        
		private ServerDataSource DataSource;

        public ServerDelegate(ServerDataSource dataSource)
        {
			this.DataSource = dataSource;
        }

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view.
            // If a non-null view is returned, you modify it enough to reflect the new data.
            NSTextField view = (NSTextField)tableView.MakeView("ActivityCell", this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = "ActivityCell";
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

			var server = Config.Servers[(int)row];

			var status = server.IsAvailable ? "online" : "offline";

			// Set up view based on the column and row
			view.StringValue = server.Url + ":" + server.Port + "|" + status;

            return view;
        }
    }
}
