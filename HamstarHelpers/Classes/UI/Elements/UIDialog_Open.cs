using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI dialog (stand-alone, centered panel) element. All dialogs are modal, and exclusively capture all
	/// interactions until closed.
	/// </summary>
	public abstract partial class UIDialog : UIThemedState {
		private void UpdateOpenState() {
			if( Main.playerInventory ) {
				this.Close();
				return;
			}
			if( Main.npcChatText != "" ) {
				this.Close();
				return;
			}
			if( this.Backend == null || this.Backend.CurrentState != this ) {
				this.Close();
				return;
			}

			if( this.SetDialogToClose ) {
				this.SetDialogToClose = false;
				this.Close();
				return;
			}
		}


		////////////////

		/// <returns>`true` if dialog can be opened (UI not otherwise captured, no other dialogs, etc.).</returns>
		public virtual bool CanOpen() {
			return !this.IsOpen && !Main.inFancyUI &&
				(DialogManager.Instance != null && DialogManager.Instance.CurrentDialog == null);
		}


		/// <summary>
		/// Opens the dialog. All input and UI context is captured.
		/// </summary>
		public virtual void Open() {
			this.IsOpen = true;
			this.NeedsRecalc = true;

			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";

			Main.inFancyUI = true;
			Main.InGameUI.SetState( (UIState)this );

			this.Backend = Main.InGameUI;

			this.Recalculate();

			if( DialogManager.Instance != null ) {
				DialogManager.Instance.SetCurrentDialog( this );
			}
		}


		/// <summary>
		/// Closes the current dialog. All UI context is reverted to the game's normal state.
		/// </summary>
		public virtual void Close() {
			this.IsOpen = false;

			if( Main.InGameUI.CurrentState == this ) {
				Main.inFancyUI = false;
				Main.InGameUI.SetState( (UIState)null );
			}

			this.Backend = null;
		}
	}
}
