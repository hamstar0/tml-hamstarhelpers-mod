using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal void SubmitTags() {
			if( this.CurrentModName == "" ) {
				throw new Exception( "Invalid mod name." );
			}

			Action<string> on_success = delegate ( string output ) {
				this.InfoDisplay.SetText( output );
				ErrorLogger.Log( "Mod info submit result: " + output );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				this.InfoDisplay.SetText( "Error: " + output );
				LogHelpers.Log( e.ToString() );
			};

			PostModInfo.SubmitModInfo( this.CurrentModName, this.GetTagsOfState( 1 ), on_success, on_fail );

			this.FinishButton.Lock();
			this.ResetButton.Lock();
		}
	}
}
