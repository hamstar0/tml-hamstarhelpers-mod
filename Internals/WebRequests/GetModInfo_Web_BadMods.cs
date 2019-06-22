using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/** @private */
	partial class GetModInfo {
		private static void RetrieveBadModsAsync( Action<IDictionary<string, int>, bool> onSuccess ) {
			Action<Exception, string> onFail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + ( output.Length > 256 ? output.Substring( 0, 256 ) : output ) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( ( output ?? "" ) + " - " + e.Message );
				} else {
					LogHelpers.Alert( ( output ?? "" ) + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, int>, bool> onCompletion = ( responseVal, success ) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, int>();
				}

				onSuccess( responseVal, success );
			};
			
			NetPlayHelpers.MakeGetRequestAsync( GetModInfo.BadModsUrl, GetModInfo.HandleBadModsReceipt, onFail, onCompletion );
		}


		private static Tuple<IDictionary<string, int>, bool> HandleBadModsReceipt( string output ) {
			IDictionary<string, int> badMods = new Dictionary<string, int>();

			JObject respJson = JObject.Parse( output );
			bool found = respJson.Count > 0;

			if( found ) {
				badMods = respJson.ToObject<Dictionary<string, int>>();
				if( badMods == null ) {
					throw new NullReferenceException( "No bad mods found" );
				}
			}

			return Tuple.Create( badMods, found );
		}
	}
}
