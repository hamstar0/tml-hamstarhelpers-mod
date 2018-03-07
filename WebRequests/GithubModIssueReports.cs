using HamstarHelpers.MiscHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.TmlHelpers.ModHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	public struct GithubModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	class GithubModIssueReports {
		public static void ReportIssue( Mod mod, string issue_title, string issue_body, Action<string> on_success, Action<Exception> on_error=null, Action on_completion=null ) {
			if( !ModMetaDataManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}

			int max_lines = HamstarHelpersMod.Instance.Config.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModHelpers.GetAllMods();
			string body_info = string.Join( "\n \n", InfoHelpers.OutputGameData( mods ).ToArray() );
			string body_errors = string.Join( "\n", InfoHelpers.OutputErrorLog( max_lines ).ToArray() );
			
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "In-game: " + issue_title;
			string body = body_info;
			body += "\n \n \n \n" + "Recent error logs:\n```\n" + body_errors + "\n```";
			body += "\n \n" + issue_body;

			var json = new GithubModIssueReportData {
				githubuser = ModMetaDataManager.GetGithubUserName( mod ),
				githubproject = ModMetaDataManager.GetGithubProjectName( mod ),
				title = title,
				body = body
			};
			string json_str = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			NetHelpers.NetHelpers.MakePostRequestAsync( url, json_bytes, delegate ( string output ) {
				JObject resp_json = JObject.Parse( output );
				//JToken data = resp_json.SelectToken( "Data.html_url" );
				JToken msg = resp_json.SelectToken( "Msg" );

				/*if( data != null ) {
					string post_at_url = data.ToObject<string>();
					if( !string.IsNullOrEmpty( post_at_url ) ) {
						SystemHelpers.Start( post_at_url );
					}
				}*/

				if( msg == null ) {
					on_success( "Failure." );
				} else {
					on_success( msg.ToObject<string>() );
				}
			}, on_error, on_completion );
		}
	}
}
