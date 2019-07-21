using HamstarHelpers.Services.ModHelpers;
using System;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		private void ApplyConfigChanges() {
			this.RequestClose = true;
		}

		private void ToggleModLock() {
			if( !ModLockService.IsWorldLocked() ) {
				ModLockService.LockWorld();
			} else {
				ModLockService.UnlockWorld();
			}

			this.RefreshModLockButton();
		}
	}
}
