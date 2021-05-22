using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;
using HamstarHelpers.Libraries.DotNET.Encoding;
using HamstarHelpers.Libraries.Info;
using HamstarHelpers.Libraries.ModHelpers;
using HamstarHelpers.Libraries.Net;
using HamstarHelpers.Libraries.TModLoader.Mods;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	public struct GithubModIssueReportData {
		/// @private
		public string githubuser;
		/// @private
		public string githubproject;
		/// @private
		public string title;
		/// @private
		public string body;
		//public string[] labels;
	}



	/// @private
	class PostGithubModIssueReports {
		public static void ReportIssue(
					Mod mod,
					string issueTitle,
					string issueBody,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion ) {
			if( !ModFeaturesLibraries.HasGithub( mod ) ) {
				throw new ModHelpersException( "Mod is not eligable for submitting issues." );
			}

			int maxLines = ModHelpersConfig.Instance.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModListLibraries.GetAllLoadedModsPreferredOrder();
			string bodyInfo = string.Join( "\n \n", FormattedGameInfoLibraries.GetFormattedGameInfo( mods ) );
			string bodyErrors = string.Join( "\n", GameInfoLibraries.GetErrorLog( maxLines ) );

			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "Reported from in-game: " + issueTitle;
			string body = bodyInfo;
			body += "\n \n \n \n" + "Recent error logs:\n```\n" + bodyErrors + "\n```";
			body += "\n \n" + issueBody;

			var json = new GithubModIssueReportData {
				githubuser = ModFeaturesLibraries.GetGithubUserName( mod ),
				githubproject = ModFeaturesLibraries.GetGithubProjectName( mod ),
				title = title,
				body = body
			};
			string jsonStr = JsonConvert.SerializeObject( json, Formatting.Indented );

			Action<bool, string> wrappedOnCompletion = ( success, output ) => {
				string processedOutput = "";

				if( success ) {
					string sanitizedOutput = EncodingLibraries.SanitizeForASCII( output );
					JObject respJson = JObject.Parse( sanitizedOutput );
					JToken data = respJson.SelectToken( "Data.html_url" );
					JToken msg = respJson.SelectToken( "Msg" );

					if( data != null ) {
						string issueUrl = data.ToObject<string>();
						if( !string.IsNullOrEmpty( issueUrl ) ) {
							SystemLibraries.OpenUrl( issueUrl );
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
				LogLibraries.Log( "!ModHelpers.PostGithubModIssueReports.ReportIssue - Failed for POST to "+url+" : " + jsonStr );
				onError( e, str );
			};

			WebConnectionLibraries.MakePostRequestAsync( url, jsonStr, e => wrappedOnError(e, ""), wrappedOnCompletion );
		}
	}
}
