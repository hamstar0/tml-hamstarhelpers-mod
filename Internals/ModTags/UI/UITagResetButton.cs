using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagResetButton : UIMenuButton {
		private readonly TagsMenuContextBase MenuContext;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagResetButton( TagsMenuContextBase menu_context )
				: base( UITheme.Vanilla, "Reset Tags", 72f, 28f, 200f, 172f, 0.36f, true ) {
			this.MenuContext = menu_context;

			this.RecalculatePos();
			this.UpdateEnableState();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			this.MenuContext.ResetTagButtons();
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

			var mod_info_context = this.MenuContext as ModInfoTagsMenuContext;

			if( mod_info_context != null ) {
				if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_info_context.CurrentModName ) ) {
					this.Disable();
					return;
				}
			}

			if( this.MenuContext.GetTagsOfState(1).Count > 0 ) {
				if( mod_info_context != null ) {
					if( mod_info_context.FinishButton.Text == "Modify Tags" ) {
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
