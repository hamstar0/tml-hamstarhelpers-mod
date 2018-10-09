using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal void SubmitTags() {
			if( string.IsNullOrEmpty( this.CurrentModName ) ) {
				this.CurrentModName = MenuModHelper.GetModName( MenuContextService.GetPreviousMenu(), MenuContextService.GetCurrentMenu() )
					?? "";
				if( string.IsNullOrEmpty( this.CurrentModName ) ) {
					throw new Exception( "Invalid mod name." );
				}
			}

			Action<string> on_success = delegate ( string output ) {
				MenuContextBase.InfoDisplay?.SetText( output, Color.Lime );
				ErrorLogger.Log( "Mod info submit result: " + output );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				MenuContextBase.InfoDisplay?.SetText( "Error: " + (string.IsNullOrEmpty(output)?e.Message:output), Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			ISet<string> new_tags = this.GetTagsOfState( 1 );

			// Update snapshot of tags for the given mod (locally)
			if( this.AllModTagsSnapshot != null ) {
				this.AllModTagsSnapshot[this.CurrentModName] = new_tags;
			}

			PostModInfo.SubmitModInfo( this.CurrentModName, new_tags, on_success, on_fail );

			this.FinishButton.Lock();
			this.ResetButton.Lock();
		}
	}
}
