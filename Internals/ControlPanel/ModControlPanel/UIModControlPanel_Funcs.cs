using HamstarHelpers.Services.ModHelpers;
using System;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	partial class UIModControlPanel : UIPanel {
		private void ApplyConfigChanges() {
			this.Logic.ApplyConfigChanges();

			this.SetDialogToClose = true;
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
