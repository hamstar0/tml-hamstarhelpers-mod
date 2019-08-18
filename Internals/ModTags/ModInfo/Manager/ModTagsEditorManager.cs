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

			this.TagsUI.RefreshButtonEnableStates();
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

			ISet<string> tagsOfMod = found && tagsPerMod.ContainsKey( modName ) ?
					tagsPerMod[modName] :
					new HashSet<string>();
			bool hasExistingTags = tagsOfMod.Count > 0;

			//LogHelpers.Log( "SetCurrentMod modname: " + modName + ", modTags: " + string.Join(",", netModTags ) );
			if( hasExistingTags ) {
				this.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
				//this.UI.ResetButton.Disable();
				this.TagsUI.DisableResetButton();
			} else {
				this.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
				this.DisableSubmitMode();
				//this.UI.FinishButton.SetModeSubmit();
			}

			this.MyTagsUI.SetCurrentMod( modName, tagsOfMod );
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
