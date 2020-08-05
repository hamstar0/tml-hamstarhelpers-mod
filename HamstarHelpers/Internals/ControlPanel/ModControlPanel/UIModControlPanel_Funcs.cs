using HamstarHelpers.Services.ModHelpers;
using System;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
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
