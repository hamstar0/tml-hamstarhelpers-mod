using HamstarHelpers.Internals.ControlPanel;
using System;


namespace HamstarHelpers.Services.ControlPanel {
	public class ControlPanelTabs {
		public static void AddTab( string title, UIControlPanelTab tab ) {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.AddTab( title, tab );
		}

		////////////////

		public static string GetCurrentTab() {
			var mymod = ModHelpersMod.Instance;

			return mymod.ControlPanel?.CurrentTabName;
		}

		public static void OpenTab( string tabName ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.ControlPanel != null ) {
				if( !mymod.ControlPanel.IsOpen ) {
					mymod.ControlPanel.Open();
				}

				mymod.ControlPanel.ChangeToTab( tabName );
			}
		}


		////////////////

		public static bool IsDialogOpen() {
			return ModHelpersMod.Instance.ControlPanel?.IsOpen ?? false;
		}

		public static void CloseDialog() {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel?.Close();
			//this.SetDialogToClose = false;
			//this.Close();
		}
	}
}
