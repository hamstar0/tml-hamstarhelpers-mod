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

			this.Manager.TagsUI.ResetTagButtonOnStates( false );
		}
		

		////////////////

		public void RefreshEnableState( bool canResetTags ) {
			//if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( modInfoContext.CurrentModName ) ) {
			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Disable();
				return;
			}

			bool modifiedTagsExist = this.Manager.GetTagsWithGivenState( 1 ).Count > 0
				|| this.Manager.GetTagsWithGivenState( -1 ).Count > 0;

			if( !modifiedTagsExist ) {
				this.Disable();
				return;
			}

			if( canResetTags ) {
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
