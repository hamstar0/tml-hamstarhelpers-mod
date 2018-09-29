using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagResetButton : UIMenuButton {
		private readonly TagsMenuContextBase UIManager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagResetButton( TagsMenuContextBase modtagui )
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

			var mod_info_context = this.UIManager as ModInfoTagsMenuContext;

			if( mod_info_context != null ) {
				if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_info_context.CurrentModName ) ) {
					this.Disable();
					return;
				}
			}

			if( this.UIManager.GetTagsOfState(1).Count > 0 ) {
				if( mod_info_context != null ) {
					if( mod_info_context.FinishButton.Text == "Modify" ) {
						this.Disable();
						return;
					}
				}
				
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
