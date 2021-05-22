using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Encoding;
using HamstarHelpers.Libraries.DotNET.Extensions;
using HamstarHelpers.Libraries.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetModInfo {
		private static void RetrieveBadModsAsync( Action<bool, BadModsDatabase> onCompletion ) {
			Action<Exception, string> onError = ( e, jsonStr ) => {
				if( e is JsonReaderException ) {
					LogLibraries.Alert( "Bad JSON: " + jsonStr.Trunc(256) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogLibraries.Alert( ("'"+jsonStr.Trunc(64)+"'" ?? "...") + " - " + e.Message );
				} else {
					LogLibraries.Alert( ("'"+jsonStr.Trunc(64)+"'" ?? "...") + " - " + e.ToString() );
				}
			};

			Action<bool, string> onWrappedCompletion = ( success, jsonStr ) => {
				BadModsDatabase badModsDb;

				if( success ) {
					try {
						success = GetModInfo.HandleBadModsReceipt( jsonStr, out badModsDb );
					} catch( Exception e ) {
						badModsDb = new BadModsDatabase();
						onError( e, jsonStr );
					}
				} else {
					badModsDb = new BadModsDatabase();
				}

				onCompletion( success, badModsDb );
			};
			
			WebConnectionLibraries.MakeGetRequestAsync( GetModInfo.BadModsUrl, e => onError(e, ""), onWrappedCompletion );
		}


		private static bool HandleBadModsReceipt( string jsonData, out BadModsDatabase badModsDb ) {
			badModsDb = new BadModsDatabase();

			string sanitizedJsonData = EncodingLibraries.SanitizeForASCII( jsonData );
			JObject parsedObject = JObject.Parse( sanitizedJsonData );

			if( parsedObject.Count == 0 ) {
				return false;
			}

			//foreach( KeyValuePair<string, JToken> kvp in parsedObject ) {
			//	if( kvp.Key != "update" ) {
			//		object obj = JsonConvert.DeserializeObject( kvp.Value.ToString() );
			//		badModsDb[kvp.Key] = (int)obj;
			//	}
			//}

			IDictionary<string, string> skimmed = JsonLibraries.SkimIncompatibleEntries<BadModsDatabase>( parsedObject );
			if( skimmed.Count > 0 ) {
				LogLibraries.Alert( "Skimmed "+skimmed.Count+" bad entries from input:\n    "+string.Join(", ", skimmed.Keys) );
			}

			badModsDb = parsedObject.ToObject<BadModsDatabase>();
			if( badModsDb == null ) {
				throw new NullReferenceException( "No bad mods found" );
			}

			return true;
		}
	}
}
