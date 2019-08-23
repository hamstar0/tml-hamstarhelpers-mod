using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Info;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.TModLoader.Mods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	public struct GithubModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	/// @private
	class PostGithubModIssueReports {
		public static void ReportIssue( Mod mod, string issueTitle, string issueBody,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion ) {
			if( !ModFeaturesHelpers.HasGithub( mod ) ) {
				throw new ModHelpersException( "Mod is not eligable for submitting issues." );
			}

			int maxLines = ModHelpersMod.Instance.Config.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModListHelpers.GetAllLoadedModsPreferredOrder();
			string bodyInfo = string.Join( "\n \n", FormattedGameInfoHelpers.GetFormattedGameInfo( mods ).ToArray() );
			string bodyErrors = string.Join( "\n", GameInfoHelpers.GetErrorLog( maxLines ).ToArray() );

			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "Reported from in-game: " + issueTitle;
			string body = bodyInfo;
			body += "\n \n \n \n" + "Recent error logs:\n```\n" + bodyErrors + "\n```";
			body += "\n \n" + issueBody;

			var json = new GithubModIssueReportData {
				githubuser = ModFeaturesHelpers.GetGithubUserName( mod ),
				githubproject = ModFeaturesHelpers.GetGithubProjectName( mod ),
				title = title,
				body = body
			};
			string jsonStr = JsonConvert.SerializeObject( json, Formatting.Indented );

			Action<bool, string> wrappedOnCompletion = ( success, output ) => {
				string processedOutput = "";

				if( success ) {
					JObject respJson = JObject.Parse( output );
					JToken data = respJson.SelectToken( "Data.html_url" );
					JToken msg = respJson.SelectToken( "Msg" );

					if( data != null ) {
						string issueUrl = data.ToObject<string>();
						if( !string.IsNullOrEmpty( issueUrl ) ) {
							SystemHelpers.OpenUrl( issueUrl );
						}
					}

					success = msg != null;
					if( success ) {
						processedOutput = msg.ToObject<string>();
					} else {
						processedOutput = "Failure.";
					}
				}

				onCompletion( success, processedOutput );
			};

			Action<Exception, string> wrappedOnError = ( Exception e, string str ) => {
				LogHelpers.Log( "!ModHelpers.PostGithubModIssueReports.ReportIssue - Failed for POST to "+url+" : " + jsonStr );
				onError( e, str );
			};

			WebConnectionHelpers.MakePostRequestAsync( url, jsonStr, wrappedOnError, wrappedOnCompletion );
		}
	}
}
