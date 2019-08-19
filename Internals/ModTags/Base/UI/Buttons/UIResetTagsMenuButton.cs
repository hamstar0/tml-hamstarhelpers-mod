using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.Base.UI.Buttons {
	/// @private
	partial class UIResetTagsMenuButton : UIMenuButton {
		public static float ButtonWidth { get; private set; } = 98f;
		public static float ButtonHeight { get; private set; } = 24f;



		////////////////

		private readonly ModTagsManager Manager;

		public bool IsLocked { get; private set; }



		////////////////

		public UIResetTagsMenuButton( UITheme theme, ModTagsManager manager, float xCenterOffset, float y )
				: base( theme,
					"Reset Tags",
					UIResetTagsMenuButton.ButtonWidth,
					UIResetTagsMenuButton.ButtonHeight,
					xCenterOffset,	//Old value: -196f, 172f
					y,
					0.36f,
					true ) {
			this.Manager = manager;
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			this.Manager.TagsUI.ResetTagButtons( false );
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.RefreshEnableState();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.RefreshEnableState();
		}
		

		////////////////

		public void RefreshEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			//if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( modInfoContext.CurrentModName ) ) {
			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Disable();
				return;
			}

			if( this.Manager.GetTagsWithGivenState(1).Count > 0 || this.Manager.GetTagsWithGivenState(-1).Count > 0 ) {
				//modInfoContext.FinishButton.Text == UIEditModeMenuButton.ModifyModeText
				if( this.Manager.CanEditTags() ) {
					this.Disable();
					return;
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
