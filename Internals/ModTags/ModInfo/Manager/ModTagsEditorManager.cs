using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.ModTags.ModInfo.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.Manager {
	partial class ModTagsEditorManager : ModTagsManager {
		public UIModTagsEditorInterface MyTagsUI => (UIModTagsEditorInterface)this.TagsUI;



		////////////////

		public ModTagsEditorManager( UIInfoDisplay infoDisplay, MenuUIDefinition menuDef, UIState _uiModInfo )
				: base( infoDisplay, menuDef, false ) {
			this.TagsUI = new UIModTagsEditorInterface( UITheme.Vanilla, this, _uiModInfo );

			this.TagsUI.RefreshControls();
		}


		////////////////

		public void SetCurrentModAsync( string modName ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				if( !args.Found ) {
					LogHelpers.Warn();
					return false;
				}

				this.SetCurrentMod( modName, args.Found, args.ModTags );
				return false;
			} );
		}


		private void SetCurrentMod( string modName, bool found, IDictionary<string, ISet<string>> tagsPerMod ) {
			this.AllModTagsSnapshot = tagsPerMod;
			this.CurrentModName = modName;

			ISet<string> tagsOfMod = found && tagsPerMod.ContainsKey( modName ) ?
					tagsPerMod[modName] :
					new HashSet<string>();

			this.MyTagsUI.SetTagsForCurrentMod( modName, tagsOfMod );
			this.MyTagsUI.RefreshControls();
		}


		////////////////

		public override void OnTagStateChange( string tagName, int state ) {
		}
	}
}
