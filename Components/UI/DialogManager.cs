using HamstarHelpers.Components.UI.Elements;
using HamstarHelpers.Helpers.TmlHelpers;
using Terraria;


namespace HamstarHelpers.Components.UI {
	class DialogManager {
		public static DialogManager Instance {
			get {
				try {
					var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );
					return myplayer.Logic.DialogManager;
				} catch { }
				return null;
			}
		}


		////////////////

		public bool ForcedModalDialog { get; private set; }
		public UIDialog CurrentDialog { get; private set; }


		////////////////

		public DialogManager() {
			this.ForcedModalDialog = false;
			this.CurrentDialog = null;
		}

		////////////////

		internal void SetForcedModality() {
			if( this.CurrentDialog != null ) {
				this.ForcedModalDialog = true;
			}
		}

		internal void UnsetForcedModality() {
			this.ForcedModalDialog = false;
		}


		internal void SetCurrentDialog( UIDialog dlg ) {
			if( this.CurrentDialog != null && this.CurrentDialog != dlg ) {
				this.CurrentDialog.Close();
			}
			this.CurrentDialog = dlg;
		}

		////////////////

		internal void Update() {
			if( this.CurrentDialog == null ) {
				return;
			}

			if( Main.InGameUI.CurrentState != this.CurrentDialog ) {
				this.CurrentDialog.Close();
			}
			
			if( !this.CurrentDialog.IsOpen ) {
				if( this.ForcedModalDialog ) {
					this.CurrentDialog.Open();
				} else {
					this.CurrentDialog = null;
				}
			}
		}
	}
}
