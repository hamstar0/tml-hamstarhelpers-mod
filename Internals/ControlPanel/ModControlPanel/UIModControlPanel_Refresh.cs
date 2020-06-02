using Terraria;
using Terraria.ID;
using HamstarHelpers.Services.ModHelpers;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		public void RefreshModLockButton() {
			bool areModsLocked = ModLockService.IsWorldLocked();
			string status = areModsLocked ? ": ON" : ": OFF";
			bool isEnabled = true;

			if( !ModHelpersConfig.Instance.WorldModLockEnable ) {
				status += " (disabled)";
				isEnabled = false;
			} else if( Main.netMode != NetmodeID.SinglePlayer ) {
				status += " (single-player only)";
				isEnabled = false;
			}

			if( !isEnabled ) {
				if( this.ModLockButton.IsInteractive ) {
					this.ModLockButton.Disable();
				}
			} else {
				if( !this.ModLockButton.IsInteractive ) {
					this.ModLockButton.Enable();
				}
			}

			this.ModLockButton.SetText( UIModControlPanelTab.ModLockTitle + status );
		}


		////////////////

		public void UpdateElements() {
			if( !ModHelpersConfig.Instance.WorldModLockEnable ) {
				if( this.ModLockButton.IsInteractive ) {
					this.RefreshModLockButton();
				}
			} else {
				if( !this.ModLockButton.IsInteractive ) {
					this.RefreshModLockButton();
				}
			}
		}
	}
}
