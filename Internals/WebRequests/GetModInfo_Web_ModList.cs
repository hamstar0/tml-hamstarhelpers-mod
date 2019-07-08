using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.TModLoader.Mods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetModInfo {
		private static void RetrieveAllModInfoAsync( Action<IDictionary<string, BasicModInfo>, bool> onSuccess ) {
			Action<Exception, string> onFail = (e, output) => {
				if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + ( output.Length > 256 ? output.Substring( 0, 256 ) : output ) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( (output ?? "") + " - " + e.Message );
				} else {
					LogHelpers.Alert( (output ?? "") + " - " + e.ToString() );
				}
			};

			Action<IDictionary<string, BasicModInfo>, bool> onCompletion = (responseVal, success) => {
				if( responseVal == null ) {
					responseVal = new Dictionary<string, BasicModInfo>();
				}
				onSuccess( responseVal, success );
			};

			WebConnectionHelpers.MakeGetRequestAsync( GetModInfo.ModInfoUrl, GetModInfo.HandleModInfoReceipt, onFail, onCompletion );
		}


		private static Tuple<IDictionary<string, BasicModInfo>, bool> HandleModInfoReceipt( string output ) {
			IDictionary<string, BasicModInfo> modInfos = new Dictionary<string, BasicModInfo>();

			JObject respJson = JObject.Parse( output );
			bool found = respJson.Count > 0;

			if( found ) {
				JToken modListToken = respJson.SelectToken( "modlist" );
				if( modListToken == null ) {
					throw new NullReferenceException( "No modlist" );
				}

				JToken[] modList = modListToken.ToArray();

				foreach( JToken modEntry in modList ) {
					JToken modNameToken = modEntry.SelectToken( "name" );
					JToken modDisplaynameToken = modEntry.SelectToken( "displayname" );
					JToken modAuthorToken = modEntry.SelectToken( "author" );
					JToken modVersRawToken = modEntry.SelectToken( "version" );
					//JToken modDescRawToken = modEntry.SelectToken( "hasdescription" );
					//JToken modHomepageRawToken = modEntry.SelectToken( "homepage" );

					if( modNameToken == null || modVersRawToken == null || modDisplaynameToken == null || modAuthorToken == null
							/*|| hasDescRawToken == null || modHomepageRawToken == null*/ ) {
						continue;
					}

					string modName = modNameToken.ToObject<string>();
					string modDisplayName = modDisplaynameToken.ToObject<string>();
					string modVersRaw = modVersRawToken.ToObject<string>();
					//string modDesc = modDescRawToken?.ToObject<string>() ?? null;
					//string modHomepage = modHomepageRawToken.ToObject<string>();

					Version modVers = Version.Parse( modVersRaw.Substring( 1 ) );
					IEnumerable<string> modAuthors = modAuthorToken.ToObject<string>()
						.Split( ',' )
						.SafeSelect( a => a.Trim() );

					modInfos[modName] = new BasicModInfo( modDisplayName, modAuthors, modVers, null, null );    //modDesc, modHomepage
				}
			}

			return Tuple.Create( modInfos, found );
		}
	}
}
