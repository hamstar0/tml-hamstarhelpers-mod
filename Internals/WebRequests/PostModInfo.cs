using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	public struct PostModTagsData {
		/// <summary></summary>
		public string modname;
		/// <summary></summary>
		public string modtags;
	}



	/// @private
	class PostModInfo {
		public static void SubmitModInfo( string modName, ISet<string> modTags,
					Action<Exception, string> onError,
					Action<bool, string> onCompletion=null ) {
			string url = "http://hamstar.pw/hamstarhelpers/mod_info_submit/";
			var json = new PostModTagsData {
				modname = modName,
				modtags = string.Join(",", modTags)
			};

			string jsonStr = JsonConvert.SerializeObject( json, Formatting.Indented );

			Action<bool, string> wrappedOnCompletion = ( success, output ) => {
				string processedOutput = "";

				if( success ) {
					JObject respJson = JObject.Parse( output );
					JToken msg = respJson.SelectToken( "Msg" );

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
				LogHelpers.Log( "!ModHelpers.PostModInfo.SubmitModInfo - Failed for post: "+jsonStr );
				onError( e, str );
			};

			WebConnectionHelpers.MakePostRequestAsync( url, jsonStr, wrappedOnError, wrappedOnCompletion );
		}
	}
}
