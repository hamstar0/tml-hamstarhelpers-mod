using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base {
	abstract partial class ModTagsManager {
		protected static ISet<string> RecentTaggedMods = new HashSet<string>();



		////////////////

		public virtual TagDefinition[] MyTags => ModTagsManager.Tags;
		public UIModTagsPanel TagsUI { get; protected set; }
		public string CurrentModName { get; protected set; }
		public bool CanExcludeTags { get; private set; }
		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; private set; }



		////////////////

		public ModTagsManager( bool canExcludeTags ) {
			this.CanExcludeTags = canExcludeTags;
			//this.TagsUI = new UIModTagsPanel( UITheme.Vanilla, this, uiContext, this.CanExcludeTags );
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
					this.DisableSubmitMode();
					//this.UI.FinishButton.SetModeSubmit();
				}

				this.TagsUI.SetCurrentMod( modName, netModTags );

				return false;
			} );
		}


		////////////////

		public void EnableSubmitMode() {
			this.TagsUI.EnableSubmitMode();
		}

		public void DisableSubmitMode() {
			this.TagsUI.DisableSubmitMode();
		}


		////////////////

		public bool CanEditTags() {
			return this.TagsUI.CanEditTags();
		}

		public bool IsCurrentModRecentlyTagged() {
			return ModTagsManager.RecentTaggedMods.Contains( this.CurrentModName );
		}

		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.TagsUI.GetTagsWithGivenState( state );
		}
	}
}
