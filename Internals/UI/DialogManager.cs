using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.TModLoader;
using Terraria;


namespace HamstarHelpers.Internals.UI {
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

		public bool ForcedPersistenceDialog { get; private set; }
		public UIDialog CurrentDialog { get; private set; }



		////////////////

		public DialogManager() {
			this.ForcedPersistenceDialog = false;
			this.CurrentDialog = null;
		}

		////////////////

		internal void SetForcedPersistence() {
			if( this.CurrentDialog != null ) {
				this.ForcedPersistenceDialog = true;
			}
		}

		internal void UnsetForcedModality() {
			this.ForcedPersistenceDialog = false;
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
				if( this.ForcedPersistenceDialog ) {
					this.CurrentDialog.Open();
				} else {
					this.CurrentDialog = null;
				}
			}
		}
	}
}
