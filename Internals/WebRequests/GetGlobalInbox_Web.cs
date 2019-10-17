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
	partial class GetGlobalInbox {
		private static void RetrieveGlobalInboxAsync( Action<bool, IDictionary<string, string>> onCompletion ) {
			Action<Exception, string> onError = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + (output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( (output ?? "...") + " - " + e.Message );
				} else {
					LogHelpers.Alert( (output ?? "...") + " - " + e.ToString() );
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

			WebConnectionHelpers.MakeGetRequestAsync( GetGlobalInbox.GlobalInboxUrl, e => onError(e, ""), onWrappedCompletion );
		}


		private static bool HandleGlobalInboxReceipt( string jsonData, out IDictionary<string, string> globalInboxDb ) {
			bool found = false;
			globalInboxDb = new Dictionary<string, string>();

			JObject respJson = JObject.Parse( jsonData );

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
