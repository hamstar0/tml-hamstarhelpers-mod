using HamstarHelpers.Helpers.NetHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;


namespace HamstarHelpers.Internals.WebRequests {
	public struct PostModTagsData {
		public string modname;
		public string modtags;
	}



	class PostModInfo {
		public static void SubmitModInfo( string mod_name, ISet<string> modtags, Action<string> on_success, Action<Exception, string> on_error, Action on_completion=null ) {
			string url = "http://hamstar.pw/hamstarhelpers/mod_info_submit/";
			var json = new PostModTagsData {
				modname = mod_name,
				modtags = string.Join(",", modtags)
			};

			string json_str = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			Action<String> on_response = ( output ) => {
				JObject resp_json = JObject.Parse( output );
				JToken msg = resp_json.SelectToken( "Msg" );

				if( msg == null ) {
					on_success( "Failure." );
				} else {
					on_success( msg.ToObject<string>() );
				}
			};

			NetHelpers.MakePostRequestAsync( url, json_bytes, on_response, on_error, on_completion );
		}
	}
}
