using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal void SubmitTags() {
			if( this.CurrentModName == "" ) {
				throw new Exception( "Invalid mod name." );
			}

			Action<string> on_success = delegate ( string output ) {
				MenuContextBase.InfoDisplay?.SetText( output, Color.Lime );
				ErrorLogger.Log( "Mod info submit result: " + output );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				MenuContextBase.InfoDisplay?.SetText( "Error: " + (string.IsNullOrEmpty(output)?e.Message:output), Color.Red );
				LogHelpers.Log( e.ToString() );
			};

			PostModInfo.SubmitModInfo( this.CurrentModName, this.GetTagsOfState( 1 ), on_success, on_fail );

			this.FinishButton.Lock();
			this.ResetButton.Lock();
		}
	}
}
