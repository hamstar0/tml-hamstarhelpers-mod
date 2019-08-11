using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.UI;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModTagsManager {
		private static ISet<string> RecentTaggedMods = new HashSet<string>();



		////////////////

		public UIModTagsPanel TagsUI { get; private set; }
		public bool CanExcludeTags { get; private set; }
		public string CurrentModName { get; private set; }
		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; private set; }



		////////////////

		public ModTagsManager( UIState uiContext, bool canExcludeTags ) {
			this.CanExcludeTags = canExcludeTags;
			this.TagsUI = new UIModTagsPanel( UITheme.Vanilla, this, uiContext, ModTagsManager.Tags, this.CanExcludeTags );
		}


		public void OnMenuContextualize() {
			this.TagsUI.ApplyMenuContext();
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


		////////////////

		public void SubmitTags() {
			if( string.IsNullOrEmpty( this.CurrentModName ) ) {
				UIState prevMenuUI = MenuContextService.GetPreviousMenuUI();
				UIState currMenuUI = MenuContextService.GetCurrentMenuUI();
				this.CurrentModName = ModMenuHelpers.GetModName( prevMenuUI, currMenuUI ) ?? "";
				if( string.IsNullOrEmpty( this.CurrentModName ) ) {
					throw new ModHelpersException( "Invalid mod name." );
				}
			}

			Action<Exception, string> onError = ( e, output ) => {
				this.SetInfoText( "Error: " + ( string.IsNullOrEmpty( output ) ? e.Message : output ), Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			Action<bool, string> onCompletion = ( success, output ) => {
				if( success ) {
					this.SetInfoText( output, Color.Lime );
					LogHelpers.Log( "Mod info submit result: " + output );
				}
			};

			ISet<string> newTags = this.GetTagsWithGivenState( 1 );

			// Update snapshot of tags for the given mod (locally)
			if( this.AllModTagsSnapshot != null ) {
				this.AllModTagsSnapshot[this.CurrentModName] = newTags;
			}

			PostModInfo.SubmitModInfo( this.CurrentModName, newTags, onError, onCompletion );
			
			this.TagsUI.LockFinishButton();
			this.TagsUI.LockResetButton();

			ModTagsManager.RecentTaggedMods.Add( this.CurrentModName );
		}
	}
}
