using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader.Menus;
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
				this.CurrentModName = ModMenuLibraries.GetModName( prevMenuUI, currMenuUI ) ?? "";

				if( string.IsNullOrEmpty( this.CurrentModName ) ) {
					throw new ModHelpersException( "Invalid mod name." );
				}
			}

			Action<Exception, string> onError = ( e, output ) => {
				if( !string.IsNullOrEmpty( output ) ) {
					this.SetInfoText( "Error (output): " + output, Color.Red );
				} else {
					this.SetInfoText( "Error (exception): " + e.Message, Color.Red );
				}
				LogLibraries.Log( e.ToString() );
			};

			ISet<string> newTags = this.GetTagsWithGivenState( 1 );

			// Update snapshot of tags for the given mod (locally)
			if( this.AllModTagsSnapshot != null ) {
				this.AllModTagsSnapshot[ this.CurrentModName ] = newTags;
			}

			PostModInfo.SubmitModInfo( this.CurrentModName, newTags, onError, this.PostSubmitTags );

			ModTagsManager.RecentTaggedMods.Add( this.CurrentModName );
		}


		private void PostSubmitTags( bool success, string output ) {
			if( success ) {
				this.SetInfoText( output, Color.Lime );
				LogLibraries.Log( "Mod info submit result: " + output );
			}

			this.TagsUI.RefreshControls();
		}
	}
}
