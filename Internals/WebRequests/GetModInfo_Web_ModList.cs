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
		private static void RetrieveAllModInfoAsync( Action<bool, BasicModInfoDatabase> onCompletion ) {
			Action<Exception, string> onError = (e, jsonStr) => {
				if( e == null ) {
					LogHelpers.Alert( (jsonStr ?? "") + " - Invalid exception?" );
				} else if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + ( jsonStr.Length > 256 ? jsonStr.Substring( 0, 256 ) : jsonStr ) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( "'" + (jsonStr ?? "...") + "' - " + e.Message );
				} else {
					LogHelpers.Alert( "'" + (jsonStr ?? "...") + "' - " + e.ToString() );
				}
			};

			Action<bool, string> onWrappedCompletion = ( success, jsonStr ) => {
				BasicModInfoDatabase modInfoDb;

				if( success ) {
					try {
						success = GetModInfo.HandleModInfoReceipt( jsonStr, out modInfoDb );
					} catch( Exception e ) {
						modInfoDb = new BasicModInfoDatabase();
						onError( e, jsonStr );
					}
				} else {
					modInfoDb = new BasicModInfoDatabase();
				}

				onCompletion( success, modInfoDb );
			};

			WebConnectionHelpers.MakeGetRequestAsync( GetModInfo.ModInfoUrl, e => onError(e, ""), onWrappedCompletion );
		}


		private static bool HandleModInfoReceipt( string jsonStr, out BasicModInfoDatabase modInfoDb ) {
			modInfoDb = new BasicModInfoDatabase();

			JObject respJson = JObject.Parse( jsonStr );
			if( respJson.Count == 0 ) {
				return false;
			}
			
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

				modInfoDb[modName] = new BasicModInfo( modDisplayName, modAuthors, modVers, null, null );    //modDesc, modHomepage
			}

			return true;
		}
	}
}
