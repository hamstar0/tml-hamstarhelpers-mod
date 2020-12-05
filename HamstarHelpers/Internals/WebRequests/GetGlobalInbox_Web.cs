using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Encoding;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetGlobalInbox {
		private static void RetrieveGlobalInboxAsync( Action<bool, IDictionary<string, string>> onCompletion ) {
			Action<Exception, string> onError = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + output.Trunc(64) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( (output.Trunc(64) ?? "...") + " - " + e.Message );
				} else {
					LogHelpers.Alert( (output.Trunc(64) ?? "...") + " - " + e.ToString() );
				}
			};

			Action<bool, string> onWrappedCompletion = ( success, jsonStr ) => {
				IDictionary<string, string> globalInboxSet;

				if( success ) {
					try {
						success = GetGlobalInbox.HandleGlobalInboxReceipt( jsonStr, out globalInboxSet );
					} catch( Exception e ) {
						globalInboxSet = new Dictionary<string, string>();
						onError( e, jsonStr );
					}
				} else {
					globalInboxSet = new Dictionary<string, string>();
				}

				onCompletion( success, globalInboxSet );
			};

			//

			WebConnectionHelpers.MakeGetRequestAsync( GetGlobalInbox.GlobalInboxUrl, e => onError(e, ""), onWrappedCompletion );
		}


		private static bool HandleGlobalInboxReceipt( string jsonData, out IDictionary<string, string> globalInboxDb ) {
			bool found = false;
			globalInboxDb = new Dictionary<string, string>();

			string sanitizedJsonData = EncodingHelpers.SanitizeForASCII( jsonData );
			JObject respJson = JObject.Parse( sanitizedJsonData );

			if( respJson.Count > 0 ) {
				JToken inboxToken = respJson.SelectToken( "inbox" );
				if( inboxToken == null ) {
					throw new NullReferenceException( "No inbox: " + string.Join( ",", respJson.Properties() ) );
				}

				JToken[] inboxList = inboxToken.ToArray();

				foreach( JToken msgEntry in inboxList ) {
					JToken msgTitleToken = msgEntry.SelectToken( "Title" );
					JToken msgToken = msgEntry.SelectToken( "Message" );
					if( msgTitleToken == null || msgToken == null ) {
						continue;
					}

					string msgTitle = msgTitleToken.ToObject<string>();
					string msg = msgToken.ToObject<string>();

					globalInboxDb[msgTitle] = msg;
				}
				found = true;
			}

			return found;
		}
	}
}
