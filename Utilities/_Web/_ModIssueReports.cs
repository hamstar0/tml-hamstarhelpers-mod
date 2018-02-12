using HamstarHelpers.WebHelpers;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Web {
	[System.Obsolete( "use WebHelpers.GithubModIssueReportData", true )]
	public struct ModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	[System.Obsolete( "use WebHelpers.GithubModIssueReports", true )]
	public static class ModIssueReports {
		public static string ReportIssue( Mod mod, string issue_title, string issue_body ) {
			return GithubModIssueReports.ReportIssue( mod, issue_title, issue_body );
		}


		////////////////

		public static IList<string> OutputGameData( IEnumerable<Mod> mods ) {
			return GithubModIssueReports.OutputGameData( mods );
		}

		public static IList<string> OutputWorldProgress() {
			return GithubModIssueReports.OutputWorldProgress();
		}


		public static IList<string> OutputCurrentPlayerInfo() {
			return GithubModIssueReports.OutputCurrentPlayerInfo();
		}


		public static IList<string> OutputCurrentPlayerEquipment() {
			return GithubModIssueReports.OutputCurrentPlayerEquipment();
		}


		////////////////

		public static IList<string> OutputErrorLog() {
			return GithubModIssueReports.OutputErrorLog();
		}
	}
}
