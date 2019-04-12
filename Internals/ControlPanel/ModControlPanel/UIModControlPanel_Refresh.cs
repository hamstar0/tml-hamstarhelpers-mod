using HamstarHelpers.Services.ModHelpers;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	partial class UIModControlPanel : UIPanel {
		public void RefreshModLockButton() {
			var mymod = ModHelpersMod.Instance;
			bool areModsLocked = ModLockService.IsWorldLocked();
			string status = areModsLocked ? ": ON" : ": OFF";
			bool isEnabled = true;

			if( !mymod.Config.WorldModLockEnable ) {
				status += " (disabled)";
				isEnabled = false;
			} else if( Main.netMode != 0 ) {
				status += " (single-player only)";
				isEnabled = false;
			}

			if( !isEnabled ) {
				if( this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Disable();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Enable();
				}
			}

			this.ModLockButton.SetText( UIControlPanel.ModLockTitle + status );
		}

		public void RefreshApplyConfigButton() {
			if( Main.netMode == 0 ) {
				if( !this.ApplyConfigButton.IsEnabled ) {
					this.ApplyConfigButton.Enable();
				}
			} else {
				if( this.ApplyConfigButton.IsEnabled ) {
					this.ApplyConfigButton.Disable();
				}
			}
		}


		////////////////

		public void UpdateElements() {
			var mymod = ModHelpersMod.Instance;

			if( !mymod.Config.WorldModLockEnable ) {
				if( this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			}
		}
	}
}
