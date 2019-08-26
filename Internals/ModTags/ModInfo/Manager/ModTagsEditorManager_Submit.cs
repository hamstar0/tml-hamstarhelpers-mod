using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.Manager {
	partial class ModTagsEditorManager : ModTagsManager {
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

			ModTagsManager.RecentTaggedMods.Add( this.CurrentModName );

			this.TagsUI.RefreshControls();
		}
	}
}
