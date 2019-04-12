using System;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Services.ControlPanel {
	public class ControlPanelTabs {
		public static void AddTab( string title, UIPanel body ) {
			var mymod = ModHelpersMod.Instance;

			//mymod.ControlPanel.AddTab( title, body );
		}


		public static void CloseDialog() {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.Close();
			//this.SetDialogToClose = false;
			//this.Close();
		}
	}
}
