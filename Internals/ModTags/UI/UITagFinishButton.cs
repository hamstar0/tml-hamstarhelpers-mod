using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModInfoTagsMenuContext UIManager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( ModInfoTagsMenuContext modtagui )
				: base( UITheme.Vanilla, "", 72f, 40f, -300f, 172f, 0.55f, true ) {
			this.UIManager = modtagui;

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify" ) {
				this.SetModeSubmit();
			} else {
				this.UIManager.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
			this.UIManager.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.UIManager.EnableTagButtons();
			
			if( this.UIManager.ResetButton.IsLocked ) {
				this.UIManager.ResetButton.Unlock();
			}
		}
		

		////////////////

		public void SetModeReadOnly() {
			this.SetText( "Modify" );
			
			this.UpdateEnableState();
			this.UIManager.ResetButton.UpdateEnableState();
		}

		public void SetModeSubmit() {
			this.SetText( "Submit" );
			
			this.UIManager.EnableTagButtons();

			this.UpdateEnableState();
			this.UIManager.ResetButton.UpdateEnableState();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			if( string.IsNullOrEmpty(this.UIManager.CurrentModName) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( this.UIManager.CurrentModName ) ) {
				this.Disable();
				return;
			}

			if( this.UIManager.GetTagsOfState(1).Count >= 2 ) {
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
