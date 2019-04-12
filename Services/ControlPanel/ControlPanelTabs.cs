using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Internals.ControlPanel;
using System;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.ControlPanel {
	public class ControlPanelTabs {
		public static void AddTab( string title, UIControlPanelTab tabBody ) {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.AddTab( title, tabBody );
			ControlPanelTabs.InitializeTab( tabBody );
		}

		internal static void InitializeTab( UIControlPanelTab tabBody ) {
			var uiCtrlPanel = ModHelpersMod.Instance.ControlPanel;
			var closeButton = new UITextPanelButton( tabBody.Theme, "X" );

			closeButton.Top.Set( -8f, 0f );
			closeButton.Left.Set( -16f, 1f );
			closeButton.Width.Set( 24f, 0f );
			closeButton.Height.Set( 24f, 0f );

			closeButton.OnClick += ( _, __ ) => {
				uiCtrlPanel.Close();
				Main.PlaySound( SoundID.MenuClose );
			};
			closeButton.OnMouseOver += ( _, __ ) => {
				tabBody.Theme.ApplyButtonLit( closeButton );
			};
			closeButton.OnMouseOut += ( _, __ ) => {
				tabBody.Theme.ApplyButton( closeButton );
			};
			
			tabBody.Append( closeButton );
		}


		////////////////

		public static void CloseDialog() {
			var mymod = ModHelpersMod.Instance;

			mymod.ControlPanel.Close();
			//this.SetDialogToClose = false;
			//this.Close();
		}
	}
}
