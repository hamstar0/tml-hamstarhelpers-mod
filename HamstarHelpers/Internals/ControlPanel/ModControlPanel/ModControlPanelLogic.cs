using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Internals.WebRequests;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	class ModControlPanelLogic {
		public Mod CurrentMod = null;



		////////////////

		public ModControlPanelLogic() { }

		////////////////

		public void SetCurrentMod( Mod mod ) {
			this.CurrentMod = mod;
		}


		////////////////

		public void ReportIssue( Mod mod, string issueTitle, string issueBody, Action onCompletion ) {
			Action<bool, string> wrappedOnCompletion = ( success, output ) => {
				if( success ) {
					Main.NewText( "Issue submit result: " + output, Color.Yellow );
					LogLibraries.Log( "Issue submit result: " + output );
				}
				onCompletion();
			};

			Action<Exception, string> onError = ( e, output ) => {
				Main.NewText( "Issue submit error: " + e.Message, Color.Red );
				LogLibraries.Log( e.ToString() );
			};

			PostGithubModIssueReports.ReportIssue( mod, issueTitle, issueBody, onError, wrappedOnCompletion );
		}
	}
}
