using HamstarHelpers.Commands;
using HamstarHelpers.DebugHelpers;
using HamstarHelpers.WebRequests;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	class ControlPanelLogic {
		public Mod CurrentMod = null;


		////////////////

		public ControlPanelLogic() { }


		public void SetCurrentMod( Mod mod ) {
			this.CurrentMod = mod;
		}


		////////////////

		public void ApplyConfigChanges( HamstarHelpersMod mymod ) {
			ConfigsRefreshCommand.RefreshConfigs();

			string mod_names = string.Join( ", ", mymod.ModMetaDataManager.ConfigMods.Keys.ToArray() );
			string output = "Mod configs reloaded for " + mod_names;

			Main.NewText( output, Color.Yellow );
			ErrorLogger.Log( output );
		}


		public void ReportIssue( Mod mod, string issue_title, string issue_body, Action completion_callback ) {
			GithubModIssueReports.ReportIssue( mod, issue_title, issue_body, delegate(string output) {
				Main.NewText( "Issue submit result: " + output, Color.Yellow );
				ErrorLogger.Log( "Issue submit result: " + output );
			}, delegate( Exception e ) {
				Main.NewText( "Issue submit error: " + e.Message, Color.Red );
				LogHelpers.Log( e.ToString() );
			}, completion_callback );

			/*var worker = new BackgroundWorker();
			worker.DoWork += delegate ( object sender, DoWorkEventArgs args ) {
				try {
					string output = GithubModIssueReports.ReportIssue( mod, issue_title, issue_body );

					Main.NewText( "Issue submit result: " + output, Color.Yellow );
					ErrorLogger.Log( "Issue submit result: " + output );
				} catch( Exception e ) {
					Main.NewText( "Issue submit error: " + e.Message, Color.Red );
					LogHelpers.Log( e.ToString() );
				}
			};
			worker.RunWorkerCompleted += completion_callback;
			worker.RunWorkerAsync();*/
		}
	}
}
