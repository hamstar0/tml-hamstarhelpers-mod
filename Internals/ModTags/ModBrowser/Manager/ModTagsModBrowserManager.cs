using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.ModTags.ModBrowser.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.Manager {
	/// @private
	partial class ModTagsModBrowserManager : ModTagsManager {
		public ModTagsModBrowserManager( UIInfoDisplay infoDisplay, MenuUIDefinition menuDef )
				: base( infoDisplay, menuDef, true ) {
			this.TagsUI = new UIModTagsModBrowserInterface( UITheme.Vanilla, this );

			this.TagsUI.RefreshButtonEnableStates();
		}


		////////////////

		public override void OnSetTagState( string tagName, int state ) {
			this.ApplyModsFilter();
		}


		////////////////

		private void ApplyDefaultFiltersAsync( UIState modBrowserUi ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				Timers.SetTimer( "ModBrowserDefaultTagStates", 15, () => {
					bool isLoading;
					if( !ReflectionHelpers.Get( modBrowserUi, "loading", out isLoading ) ) {
						LogHelpers.Warn( "ModBrowserTagsMenuContext - No 'loading'." );
						return false;
					}

					if( isLoading ) {
						return true;
					} else {
						//UITagButton button = this.TagButtons["Misleading Info"];
						//button.SetTagState( -1 );
						return false;
					}
				} );
				return true;
			} );
		}
	}
}
