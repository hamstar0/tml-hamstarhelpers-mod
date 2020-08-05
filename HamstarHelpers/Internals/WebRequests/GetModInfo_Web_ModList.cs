using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Encoding;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Net;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace HamstarHelpers.Internals.WebRequests {
	/// @private
	partial class GetModInfo {
		private static void RetrieveAllModInfoAsync( Action<bool, BasicModInfoDatabase> onCompletion ) {
			Action<Exception, string> onError = (e, jsonStr) => {
				if( e == null ) {
					LogHelpers.Alert( (jsonStr.Trunc(64) ?? "") + " - Invalid exception?" );
				} else if( e is JsonReaderException ) {
					LogHelpers.Alert( "Bad JSON: " + jsonStr.Trunc(256) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Alert( ("'"+jsonStr.Trunc(64)+"'" ?? "...") + " - " + e.Message );
				} else {
					LogHelpers.Alert( ("'"+jsonStr.Trunc(64)+"'" ?? "...") + " - " + e.ToString() );
				}
			};

			Action<bool, string> onWrappedCompletion = ( success, jsonStr ) => {
				BasicModInfoDatabase modInfoDb;

				if( success ) {
					try {
						int skipped = 0;
						success = GetModInfo.HandleModInfoReceipt( jsonStr, out modInfoDb, out skipped );
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


		private static bool HandleModInfoReceipt( string jsonData, out BasicModInfoDatabase modInfoDb, out int skipped ) {
			modInfoDb = new BasicModInfoDatabase();
			skipped = 0;

			string sanitizedJsonData = EncodingHelpers.SanitizeForASCII( jsonData );
			JObject respJson = JObject.Parse( sanitizedJsonData );
			if( respJson.Count == 0 ) {
				return false;
			}

			IEnumerable<JToken> modList;
			string modlistCompressed = (string)respJson["modlist_compressed"];

			// From tModLoader's code:
			if( modlistCompressed != null ) {
				byte[] data = Convert.FromBase64String( modlistCompressed );
				var memStream = new MemoryStream( data );

				using( var zip = new GZipStream(memStream, CompressionMode.Decompress) ) {
					using( var reader = new StreamReader(zip) ) {
						modList = JArray.Parse( reader.ReadToEnd() );
					}
				}
			} else {
				// Fallback if needed.
				modList = (JArray)respJson["modlist"];
			}

			/*JToken modListToken = respJson.SelectToken( "modlist_compressed" );
			if( modListToken == null ) {
				throw new NullReferenceException( "No modlist" );
			}

			JToken[] modList = modListToken.ToArray();*/

			foreach( JToken modEntry in modList ) {
				JToken modNameToken = modEntry.SelectToken( "name" );
				JToken modDisplaynameToken = modEntry.SelectToken( "displayname" );
				JToken modAuthorToken = modEntry.SelectToken( "author" );
				JToken modVersRawToken = modEntry.SelectToken( "version" );
				//JToken modDescRawToken = modEntry.SelectToken( "hasdescription" );
				//JToken modHomepageRawToken = modEntry.SelectToken( "homepage" );

				if( modNameToken == null || modVersRawToken == null || modDisplaynameToken == null || modAuthorToken == null
						/*|| hasDescRawToken == null || modHomepageRawToken == null*/ ) {
					skipped++;
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
