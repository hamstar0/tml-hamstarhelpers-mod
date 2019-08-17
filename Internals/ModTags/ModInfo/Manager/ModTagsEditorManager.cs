using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.ModTags.ModInfo.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
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

			this.TagsUI.RefreshButtonEnableStates();
		}


		////////////////

		public void SetCurrentMod( string modName ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				if( !args.Found ) {
					LogHelpers.Warn();
					return false;
				}

				this.SetCurrentModAsync( modName, args.Found, args.ModTags );
				return false;
			} );
		}


		private void SetCurrentModAsync( string modName, bool found, IDictionary<string, ISet<string>> modTags ) {
			this.AllModTagsSnapshot = modTags;

			ISet<string> netModTags = found && modTags.ContainsKey( modName ) ?
					modTags[modName] :
					new HashSet<string>();
			bool hasNetTags = netModTags.Count > 0;

			//LogHelpers.Log( "SetCurrentMod modname: " + modName + ", modTags: " + string.Join(",", netModTags ) );
			if( hasNetTags ) {
				this.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
				//this.UI.ResetButton.Disable();
				this.TagsUI.DisableResetButton();
			} else {
				this.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
				this.DisableSubmitMode();
				//this.UI.FinishButton.SetModeSubmit();
			}

			this.MyTagsUI.SetCurrentMod( modName, netModTags );
		}


		////////////////

		public void EnableSubmitMode() {
			this.MyTagsUI.EnableSubmitOption();
		}

		public void DisableSubmitMode() {
			this.MyTagsUI.DisableSubmitOption();
		}


		////////////////

		public override bool CanEditTags() {
			return this.MyTagsUI.CanEditTags();
		}


		////////////////

		public override void OnSetTagState( string tagName, int state ) {
		}
	}
}
