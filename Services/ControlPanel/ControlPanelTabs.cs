using HamstarHelpers.Internals.ControlPanel;
using System;


namespace HamstarHelpers.Services.ControlPanel {
	public class ControlPanelTabs {
		public static void AddTab( string title, UIControlPanelTab tab ) {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.AddTab( title, tab );
		}


		////////////////

		public static void ChangeTab( string tabName ) {

		}


		public static void CloseDialog() {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.Close();
			//this.SetDialogToClose = false;
			//this.Close();
		}
	}
}
