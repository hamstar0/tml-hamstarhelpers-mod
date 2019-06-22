using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/** @private */
	partial class GetModTags {
		private static void RetrieveAllModTagsAsync( Action<IDictionary<string, ISet<string>>, bool> onCompletion ) {
			Action<Exception, string> onGetFail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + (output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( (output ?? "...") + " - " + e.Message );
				} else {
					LogHelpers.Alert( (output ?? "...") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, ISet<string>>, bool> onGetCompletion = ( responseVal, found ) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, ISet<string>>();
				}

				onCompletion( responseVal, found );
			};

			NetPlayHelpers.MakeGetRequestAsync( GetModTags.ModTagsUrl, GetModTags.HandleModTagsReceipt, onGetFail, onGetCompletion );
		}


		private static Tuple<IDictionary<string, ISet<string>>, bool> HandleModTagsReceipt( string output ) {
			bool found = false;
			IDictionary<string, ISet<string>> modTagSet = new Dictionary<string, ISet<string>>();

			JObject respJson = JObject.Parse( output );

			if( respJson.Count > 0 ) {
				JToken tagListToken = respJson.SelectToken( "modlist" );
				if( tagListToken == null ) {
					throw new NullReferenceException( "No modlist: " + string.Join( ",", respJson.Properties() ) );
				}

				JToken[] tagList = tagListToken.ToArray();

				foreach( JToken tagEntry in tagList ) {
					JToken modNameToken = tagEntry.SelectToken( "ModName" );
					JToken modTagsToken = tagEntry.SelectToken( "ModTags" );
					if( modNameToken == null || modTagsToken == null ) {
						continue;
					}

					string modName = modNameToken.ToObject<string>();
					string modTagsRaw = modTagsToken.ToObject<string>();
					string[] modTags = modTagsRaw.Split( ',' );

					modTagSet[modName] = new HashSet<string>( modTags );
				}
				found = true;
			}

			return Tuple.Create( modTagSet, found );
		}
	}
}
