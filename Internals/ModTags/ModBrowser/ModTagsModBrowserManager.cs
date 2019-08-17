using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base;
using HamstarHelpers.Internals.ModTags.ModBrowser.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser {
	partial class ModTagsModBrowserManager : ModTagsManager {
		public ModTagsModBrowserManager( UIInfoDisplay infoDisplay, MenuUIDefinition menuDef )
				: base( infoDisplay, menuDef, true ) {
			this.TagsUI = new UIModTagsModBrowserInterface( UITheme.Vanilla, this );

			this.TagsUI.RefreshButtonEnableStates();
		}
	}
}
