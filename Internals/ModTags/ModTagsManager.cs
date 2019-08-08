using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModTagsManager {
		private SessionMenuContext Context;


		////////////////

		public UITagsPanel TagsUI { get; private set; }
		public string CurrentModName { get; private set; }
		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; private set; }



		////////////////

		public ModTagsManager( SessionMenuContext menuContext, bool canDisableTags ) {
			this.Context = menuContext;
			this.TagsUI = new UITagsPanel( menuContext.MyMenuUI, UITheme.Vanilla, this, ModTagsManager.Tags, canDisableTags );
		}


		////////////////

		public void SetCurrentMod( string modName ) {
			CustomLoadHooks.AddHook( GetModTags.TagsReceivedHookValidator, ( args ) => {
				if( !args.Found ) {
					LogHelpers.Warn();
					return false;
				}

				this.AllModTagsSnapshot = args.ModTags;
				//this.AllModTagsSnapshot = args.ModTags;

				ISet<string> netModTags = args.Found && args.ModTags.ContainsKey( modName ) ?
						args.ModTags[modName] :
						new HashSet<string>();
				bool hasNetTags = netModTags.Count > 0;

				//LogHelpers.Log( "SetCurrentMod modname: " + modName + ", modTags: " + string.Join(",", netModTags ) );
				if( hasNetTags ) {
					this.SetInfoTextDefault( "Do these tags look incorrect? If so, modify them." );
					//this.UI.ResetButton.Disable();
					this.TagsUI.DisableResetButton();
				} else {
					this.SetInfoTextDefault( "No tags set for this mod. Why not add some?" );
					this.SetSubmitMode();
					//this.UI.FinishButton.SetModeSubmit();
				}

				this.TagsUI.SetCurrentMod( modName, netModTags );

				return false;
			} );
		}


		////////////////

		public void SetSubmitMode() {
			f
		}

		public void UpdateMode( bool isSubmitMode ) {
			f
		}


		////////////////

		public bool CanEditTags() {
			f
		}

		public bool IsCurrentModRecentlyTagged() {
			f
		}

		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			f
		}


		////////////////

		public void SubmitTags() {
			f
		}
	}
}
