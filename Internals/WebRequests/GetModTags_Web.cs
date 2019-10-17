using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetModTags {
		private static void RetrieveAllModTagsAsync( Action<bool, ModTagsDatabase> onCompletion ) {
			Action<Exception, string> onError = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + (output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( ("'"+output+"'" ?? "...") + " - " + e.Message );
				} else {
					LogHelpers.Alert( ("'"+output+"'" ?? "...") + " - " + e.ToString() );
				}
			};

			Action<bool, string> onWrappedCompletion = ( success, jsonStr ) => {
				ModTagsDatabase modTagSet;

				if( success ) {
					try {
						success = GetModTags.HandleModTagsReceipt( jsonStr, out modTagSet );
					} catch( Exception e ) {
						modTagSet = new ModTagsDatabase();
						onError( e, jsonStr );
					}
				} else {
					modTagSet = new ModTagsDatabase();
				}

				onCompletion( success, modTagSet );
			};

			WebConnectionHelpers.MakeGetRequestAsync( GetModTags.ModTagsUrl, e => onError(e, ""), onWrappedCompletion );
		}


		private static bool HandleModTagsReceipt( string jsonData, out ModTagsDatabase modTagsDb ) {
			bool found = false;
			modTagsDb = new ModTagsDatabase();

			JObject respJson = JObject.Parse( jsonData );

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

					modTagsDb[ modName ] = new HashSet<string>( modTags );
				}
				found = true;
			}

			return found;
		}
	}
}
