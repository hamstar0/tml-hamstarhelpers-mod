using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.NetHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	partial class GetModInfo {
		private static void RetrieveBadModsAsync( Action<IDictionary<string, bool>, bool> onSuccess ) {
			Action<Exception, string> onFail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + ( output.Length > 256 ? output.Substring( 0, 256 ) : output ) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( ( output ?? "" ) + " - " + e.Message );
				} else {
					LogHelpers.Alert( ( output ?? "" ) + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, bool>, bool> onCompletion = ( responseVal, success ) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, bool>();
				}

				onSuccess( responseVal, success );
			};
			
			NetHelpers.MakeGetRequestAsync( GetModInfo.BadModsUrl, GetModInfo.HandleBadModsReceipt, onFail, onCompletion );
		}


		private static Tuple<IDictionary<string, bool>, bool> HandleBadModsReceipt( string output ) {
			IDictionary<string, bool> badMods = new Dictionary<string, bool>();

			JObject respJson = JObject.Parse( output );
			bool found = respJson.Count > 0;

			if( found ) {
				badMods = respJson.ToObject<Dictionary<string, bool>>();
				if( badMods == null ) {
					throw new NullReferenceException( "No bad mods found" );
				}
			}

			return Tuple.Create( badMods, found );
		}
	}
}
