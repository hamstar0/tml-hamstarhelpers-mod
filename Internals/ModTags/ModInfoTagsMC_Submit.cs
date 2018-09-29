using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.WebRequests;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		internal void SubmitTags() {
			if( this.ModName == "" ) {
				throw new Exception( "Invalid mod name." );
			}

			Action<string> on_success = delegate ( string output ) {
				this.InfoDisplay.SetText( output );

				MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Submit Success",
					ui => { },
					ui => {
						this.FinishButton.Lock();
						ui.Recalculate();
					}
				);

				ErrorLogger.Log( "Mod info submit result: " + output );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				this.InfoDisplay.SetText( "Error: " + output );

				MenuUI.AddMenuLoader( this.UIName, this.ContextName + " Tag Submit Error",
					ui => { },
					ui => {
						this.FinishButton.Unlock();
						ui.Recalculate();
					}
				);
				
				LogHelpers.Log( e.ToString() );
			};

			PostModInfo.SubmitModInfo( this.ModName, this.GetTagsOfState( 1 ), on_success, on_fail );

			this.FinishButton.Lock();
		}
	}
}
