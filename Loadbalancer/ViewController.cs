using System;

using AppKit;
using Foundation;

namespace Loadbalancer
{
    public partial class ViewController : NSViewController
    {
		private LoadBalancer loadBalancer = new LoadBalancer();
		private ServerDataSource DataSource = new ServerDataSource();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			tableView.DataSource = DataSource;
			tableView.Delegate = new ServerDelegate(DataSource);
            
			var startTimeSpan = TimeSpan.Zero;
			var periodTimeSpan = TimeSpan.FromSeconds(5);

            var timer = new System.Threading.Timer((e) =>
            {
				InvokeOnMainThread(() =>
				{
					this.tableView.ReloadData();
				});
            }, null, startTimeSpan, periodTimeSpan);
            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

		partial void balancerTypeSelected(NSPopUpButton sender)
		{
            try {
                Config.BalanceMethod = (BalanceMethod)Enum.Parse(typeof(BalanceMethod), sender.TitleOfSelectedItem);
                Console.WriteLine(Config.BalanceMethod);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("'{0}' is not a member of the BalanceMethod enumeration.", sender.TitleOfSelectedItem);
            }
		}

		partial void persistenceTypeSelected(NSPopUpButton sender)
		{
            try
            {
                Config.PersistenceMethod = (PersistenceMethod)Enum.Parse(typeof(PersistenceMethod), sender.TitleOfSelectedItem.Replace(" ", ""));
                Console.WriteLine(Config.PersistenceMethod);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("'{0}' is not a member of the PersistenceMethod enumeration.", sender.TitleOfSelectedItem);
            }		
        }

		partial void startLoadbalancer(NSButton sender)
		{
			if (sender.Title == "Start")
			{
				loadBalancer.Start();
				balancerPopup.Enabled = false;
				persistencePopup.Enabled = false;
				statusLabel.Title = "Currently balancing requests.";
				progressIndicator.StartAnimation(sender);
				sender.Title = "Stop";
			}
			else if (sender.Title == "Stop")
			{
				loadBalancer.Stop();
				balancerPopup.Enabled = true;
				persistencePopup.Enabled = true;
				statusLabel.Title = "Loadbalancer inactive.";
				progressIndicator.StopAnimation(sender);
				sender.Title = "Start";
			}
		}

		partial void deleteButtonClicked(NSButton sender)
		{
			if (tableView.SelectedRow == -1) {
				return;
			}
			DataSource.DeleteServer((int) tableView.SelectedRow);
			tableView.ReloadData();
		}
	}
}
