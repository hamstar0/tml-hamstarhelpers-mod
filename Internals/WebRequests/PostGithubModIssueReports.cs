using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Misc;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Mods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.WebRequests {
	public struct GithubModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}


	
	class PostGithubModIssueReports {
		public static void ReportIssue( Mod mod, string issueTitle, string issueBody, Action<string> onSuccess, Action<Exception, string> onError, Action onCompletion=null ) {
			if( !ModFeaturesHelpers.HasGithub( mod ) ) {
				throw new HamstarException( "Mod is not eligable for submitting issues." );
			}

			int maxLines = ModHelpersMod.Instance.Config.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModListHelpers.GetAllLoadedModsPreferredOrder();
			string bodyInfo = string.Join( "\n \n", InfoHelpers.GetGameData( mods ).ToArray() );
			string bodyErrors = string.Join( "\n", InfoHelpers.GetErrorLog( maxLines ).ToArray() );

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
			byte[] jsonBytes = Encoding.UTF8.GetBytes( jsonStr );

			Action<String> onResponse = ( output ) => {
				JObject respJson = JObject.Parse( output );
				//JToken data = respJson.SelectToken( "Data.html_url" );
				JToken msg = respJson.SelectToken( "Msg" );

				/*if( data != null ) {
					string post_at_url = data.ToObject<string>();
					if( !string.IsNullOrEmpty( post_at_url ) ) {
						SystemHelpers.Start( post_at_url );
					}
				}*/

				if( msg == null ) {
					onSuccess( "Failure." );
				} else {
					onSuccess( msg.ToObject<string>() );
				}
			};

			Action<Exception, string> wrappedOnError = ( Exception e, string str ) => {
				LogHelpers.Log( "!ModHelpers.PostGithubModIssueReports.ReportIssue - Failed for POST to "+url+" : " + jsonStr );
				onError( e, str );
			};

			NetHelpers.MakePostRequestAsync( url, jsonBytes, onResponse, wrappedOnError, onCompletion );
		}
	}
}
