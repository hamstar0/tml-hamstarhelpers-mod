using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagResetButton : UIMenuButton {
		private readonly ModInfoTagsMenuContext UIManager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagResetButton( ModInfoTagsMenuContext modtagui )
				: base( UITheme.Vanilla, "Reset", 72f, 40f, 214f, 172f, 0.55f, true ) {
			this.UIManager = modtagui;

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			this.UIManager.ResetTagButtons();
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
		}
		

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}
			
			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( this.UIManager.ModName ) ) {
				this.Disable();
				return;
			}

			if( this.UIManager.GetTagsOfState(1).Count > 0 ) {
				if( this.UIManager.FinishButton.Text == "Modify" ) {
					this.Disable();
				} else {
					this.Enable();
				}
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
