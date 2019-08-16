using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.MenuContext {
	/// @private
	partial class ModTagsModInfoMenuContext : ModTagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );

			string modName = ModMenuHelpers.GetModName( MenuContextService.GetCurrentMenuUI(), ui );

			this.InfoDisplay.SetDefaultText( "" );

			if( modName == null ) {
				LogHelpers.Warn( "Could not load mod tags; no mod found" );
				return;
			}

			this.ResetUIState( modName );
			this.SetCurrentMod( ui, modName );
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.InfoDisplay.SetDefaultText( "" );
		}


		////////////////

		private void ResetUIState( string modName ) {
			if( !ModTagsModInfoMenuContext.RecentTaggedMods.Contains( modName ) ) {
				this.MyManager.MyTagsUI.UnlockFinishButton();
			} else {
				this.MyManager.MyTagsUI.LockFinishButton();
			}

			this.Manager.TagsUI.ResetTagButtons( true );
		}


		////////////////

		private void SetCurrentMod( UIState ui, string modName ) {
			this.MyManager.SetCurrentMod( modName );
			//this.CurrentModName = modName;
		}

		public override void OnTagStateChange( UITagMenuButton tagButton ) {
			this.Manager.TagsUI.RefreshButtonEnableStates();
		}
	}
}
