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
		public static void SubmitModInfo( string modName, ISet<string> modTags, Action<string> onSuccess, Action<Exception, string> onError, Action onCompletion=null ) {
			string url = "http://hamstar.pw/hamstarhelpers/modInfo_submit/";
			var json = new PostModTagsData {
				modname = modName,
				modtags = string.Join(",", modTags)
			};

			string jsonStr = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] jsonBytes = Encoding.UTF8.GetBytes( jsonStr );

			Action<String> onResponse = ( output ) => {
				JObject respJson = JObject.Parse( output );
				JToken msg = respJson.SelectToken( "Msg" );

				if( msg == null ) {
					onSuccess( "Failure." );
				} else {
					onSuccess( msg.ToObject<string>() );
				}
			};

			NetHelpers.MakePostRequestAsync( url, jsonBytes, onResponse, onError, onCompletion );
		}
	}
}
