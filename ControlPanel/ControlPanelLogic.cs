using HamstarHelpers.Commands;
using Microsoft.Xna.Framework;
using System;
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


		public void ReportIssue( Mod mod, string issue_title, string issue_body ) {
			try {
				string output = ModIssueReportCommand.ReportIssue( mod, issue_title, issue_body );

				Main.NewText( "Issue submit result: " + output, Color.Yellow );
				ErrorLogger.Log( "Issue submit result: " + output );
			} catch(Exception e ) {
				Main.NewText( "Issue submit error: "+e.ToString(), Color.Red );
				throw e;
			}
		}
	}
}
