using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Services.UI.Menus;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModBrowserTagsMenuContext : TagsMenuContextBase {
		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModBrowserTagsMenuContext();
				MenuContextService.AddMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Browser", ctx );
			}
		}



		////////////////

		private ModBrowserTagsMenuContext() : base( true ) {
		}


		////////////////

		public override void OnTagStateChange( UITagButton tagButton ) {
			this.ApplyModsFilter();

			//this.ResetButton.UpdateEnableState();
			this.Manager.TagsUI.RefreshButtonEnableStates();
		}
	}
}
