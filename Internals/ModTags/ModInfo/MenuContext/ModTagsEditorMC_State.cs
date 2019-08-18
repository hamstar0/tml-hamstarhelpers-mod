using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Services.UI.Menus;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.MenuContext {
	/// @private
	partial class ModTagsEditorMenuContext : ModTagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );

			UIState prevUi = MenuContextService.GetCurrentMenuUI();
			if( prevUi == ui ) {
				prevUi = MenuContextService.GetPreviousMenuUI();
			}

			string modName = ModMenuHelpers.GetModName( prevUi, ui );

			this.InfoDisplay.SetDefaultText( "" );

			if( modName == null ) {
				this.MyManager.MyTagsUI.ResetUIState( false );

				LogHelpers.Warn( "Could not load mod tags; no mod found" );
				return;
			}

			bool isRecentlyTagged = ModTagsEditorMenuContext.RecentTaggedMods.Contains( modName );

			this.MyManager.MyTagsUI.ResetUIState( isRecentlyTagged );
			this.MyManager.SetCurrentModAsync( modName );
		}


		public override void Hide( UIState ui ) {
			base.Hide( ui );

			this.Manager.TagsUI.UnsetCategory();
		}
	}
}
