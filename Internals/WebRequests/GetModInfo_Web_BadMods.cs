using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetModInfo {
		private static void RetrieveBadModsAsync( Action<bool, BadModsDatabase> onCompletion ) {
			Action<Exception, string> onError = ( e, jsonStr ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + ( jsonStr.Length > 256 ? jsonStr.Substring( 0, 256 ) : jsonStr ) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( ( jsonStr ?? "" ) + " - " + e.Message );
				} else {
					LogHelpers.Alert( ( jsonStr ?? "" ) + " - " + e.ToString() );
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
			
			WebConnectionHelpers.MakeGetRequestAsync( GetModInfo.BadModsUrl, onError, onWrappedCompletion );
		}


		private static bool HandleBadModsReceipt( string jsonStr, out BadModsDatabase badModsDb ) {
			badModsDb = new BadModsDatabase();

			JObject respJson = JObject.Parse( jsonStr );

			if( respJson.Count == 0 ) {
				return false;
			}

			badModsDb = respJson.ToObject<BadModsDatabase>();
			if( badModsDb == null ) {
				throw new NullReferenceException( "No bad mods found" );
			}

			return true;
		}
	}
}
