using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.MenuContext;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Services.UI.Menus;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.MenuContext {
	/// @private
	partial class ModTagsModBrowserMenuContext : ModTagsMenuContextBase {
		public static void Initialize( bool onModLoad ) {
			if( ModHelpersMod.Instance.Config.DisableModTags ) { return; }

			if( !onModLoad ) {
				var ctx = new ModTagsModBrowserMenuContext( MenuUIDefinition.UIModBrowser, "ModHelpers: Mod Browser" );
				MenuContextService.AddMenuContext( ctx );
			}
		}



		////////////////

		protected ModTagsModBrowserMenuContext( MenuUIDefinition menuDef, string contextName )
				: base( menuDef, contextName ) {
			this.Manager = new ModTagsModBrowserManager( this.InfoDisplay );
		}


		////////////////

		public override void OnTagStateChange( UITagMenuButton tagButton ) {
			this.ApplyModsFilter();

			//this.ResetButton.UpdateEnableState();
			this.Manager.TagsUI.RefreshButtonEnableStates();
		}
	}
}
