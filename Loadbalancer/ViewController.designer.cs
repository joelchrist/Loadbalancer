// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Loadbalancer
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSPopUpButton balancerPopup { get; set; }

        [Outlet]
        AppKit.NSPopUpButton persistencePopup { get; set; }

        [Outlet]
        AppKit.NSProgressIndicator progressIndicator { get; set; }

        [Outlet]
        AppKit.NSTextFieldCell statusLabel { get; set; }

        [Action ("balancerTypeSelected:")]
        partial void balancerTypeSelected (AppKit.NSPopUpButton sender);

        [Action ("persistenceTypeSelected:")]
        partial void persistenceTypeSelected (AppKit.NSPopUpButton sender);

        [Action ("startLoadbalancer:")]
        partial void startLoadbalancer (AppKit.NSButton sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (progressIndicator != null) {
                progressIndicator.Dispose ();
                progressIndicator = null;
            }

            if (balancerPopup != null) {
                balancerPopup.Dispose ();
                balancerPopup = null;
            }

            if (persistencePopup != null) {
                persistencePopup.Dispose ();
                persistencePopup = null;
            }

            if (statusLabel != null) {
                statusLabel.Dispose ();
                statusLabel = null;
            }
        }
    }
}
