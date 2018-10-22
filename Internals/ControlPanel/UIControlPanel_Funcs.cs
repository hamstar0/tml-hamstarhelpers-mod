using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	partial class UIControlPanel : UIState {
		public bool CanOpen() {
			return !this.IsOpen && !Main.inFancyUI;
		}


		public void Open() {
			this.IsOpen = true;

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";

			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );

			this.Backend = Main.InGameUI;

			this.RecalculateMe();
		}


		public void Close() {
			this.IsOpen = false;

			Main.inFancyUI = false;
			Main.InGameUI.SetState( (UIState)null );

			this.Backend = null;
		}

		////////////////

		private void ApplyConfigChanges( ModHelpersMod mymod ) {
			this.Logic.ApplyConfigChanges( mymod );

			this.SetDialogToClose = true;
		}

		private void ToggleModLock( ModHelpersMod mymod ) {
			if( !ModLockHelpers.IsWorldLocked() ) {
				ModLockHelpers.LockWorld();
			} else {
				ModLockHelpers.UnlockWorld();
			}

			this.RefreshModLockButton( mymod );
		}
	}
}
