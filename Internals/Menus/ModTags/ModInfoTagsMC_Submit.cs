using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.UI.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	/// @private
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal void SubmitTags() {
			if( string.IsNullOrEmpty( this.CurrentModName ) ) {
				this.CurrentModName = ModMenuHelpers.GetModName( MenuContextService.GetPreviousMenuUI(), MenuContextService.GetCurrentMenuUI() )
					?? "";
				if( string.IsNullOrEmpty( this.CurrentModName ) ) {
					throw new ModHelpersException( "Invalid mod name." );
				}
			}

			Action<bool, string> onCompletion = ( success, output ) => {
				if( success ) {
					this.InfoDisplay?.SetText( output, Color.Lime );
					LogHelpers.Log( "Mod info submit result: " + output );
				}
			};

			Action<Exception, string> onError = ( e, output ) => {
				this.InfoDisplay?.SetText( "Error: " + (string.IsNullOrEmpty(output)?e.Message:output), Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			ISet<string> newTags = this.GetTagsOfState( 1 );

			// Update snapshot of tags for the given mod (locally)
			if( this.AllModTagsSnapshot != null ) {
				this.AllModTagsSnapshot[this.CurrentModName] = newTags;
			}

			PostModInfo.SubmitModInfo( this.CurrentModName, newTags, onError, onCompletion );

			this.FinishButton.Lock();
			this.ResetButton.Lock();
		}
	}
}
