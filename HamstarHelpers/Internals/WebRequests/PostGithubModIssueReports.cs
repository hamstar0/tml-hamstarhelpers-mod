﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Encoding;
using HamstarHelpers.Helpers.Info;
using HamstarHelpers.Helpers.ModHelpers;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.TModLoader.Mods;


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
			if( !ModFeaturesHelpers.HasGithub( mod ) ) {
				throw new ModHelpersException( "Mod is not eligable for submitting issues." );
			}

			int maxLines = ModHelpersConfig.Instance.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModListHelpers.GetAllLoadedModsPreferredOrder();
			string bodyInfo = string.Join( "\n \n", FormattedGameInfoHelpers.GetFormattedGameInfo( mods ) );
			string bodyErrors = string.Join( "\n", GameInfoHelpers.GetErrorLog( maxLines ) );

			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "Reported from in-game: " + issueTitle;
			string body = bodyInfo;
			if (mod.Call("GetInfoStringForBugReport", issueTitle) is string modSpecificInfo && !string.IsNullOrWhiteSpace(modSpecificInfo)) {
				body += "\n \n \n \nInformation appended by " + mod.DisplayName + ":\n" + modSpecificInfo;
			}
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
					string sanitizedOutput = EncodingHelpers.SanitizeForASCII( output );
					JObject respJson = JObject.Parse( sanitizedOutput );
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

			WebConnectionHelpers.MakePostRequestAsync( url, jsonStr, e => wrappedOnError(e, ""), wrappedOnCompletion );
		}
	}
}
