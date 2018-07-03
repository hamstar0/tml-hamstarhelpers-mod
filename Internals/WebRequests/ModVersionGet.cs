using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.Internals.WebRequests {
	public class ModVersionGet {
		private readonly static object MyLock = new object();
		
		public static string ModVersionUrl { get {
				//return "://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5";
				return "https://script.google.com/macros/s/AKfycbyddPh79y6SwgYqOG-P7Pw0iCbi2tBgVCRvUfP9fJT_xrbHFzo/exec";
		} }


		
		////////////////

		public static void GetLatestKnownVersionAsync( Mod mod, Action<Version> on_success, Action<Exception> on_fail ) {
			Action check = delegate () {
				var mymod = HamstarHelpersMod.Instance;

				try {
					if( mymod.ModVersionGet.ModVersions.ContainsKey( mod.Name ) ) {
						on_success( mymod.ModVersionGet.ModVersions[mod.Name] );
					} else {
						var ke = new KeyNotFoundException( "GetLatestKnownVersion - Unrecognized mod " + mod.Name + " (not found on mod browser)" );
						on_fail( ke );
					}
				} catch( Exception e ) {
					on_fail( e );
				}
			};

			ThreadPool.QueueUserWorkItem( _ => {
				lock( ModVersionGet.MyLock ) {
					var mymod = HamstarHelpersMod.Instance;

					if( mymod.ModVersionGet.ModVersions == null ) {
						ModVersionGet.RetrieveLatestKnownVersionsAsync( ( versions, found ) => {
							if( found ) {
								mymod.ModVersionGet.ModVersions = versions;
							}
							check();
						} );
					} else {
						check();
					}
				}
			} );
			
			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}



		private static void RetrieveLatestKnownVersionsAsync( Action<IDictionary<string, Version>, bool> on_success ) {
			Action<string> on_response = delegate ( string output ) {
				IDictionary<string, Version> mod_versions = new Dictionary<string, Version>();
				bool found = false;

				JObject resp_json = JObject.Parse( output );

				if( resp_json.Count > 0 ) {
					JToken mod_list_token = resp_json.SelectToken( "modlist" );
					if( mod_list_token == null ) {
						throw new NullReferenceException( "No modlist" );
					}

					JToken[] mod_list = mod_list_token.ToArray();

					foreach( JToken mod_entry in mod_list ) {
						JToken mod_name_token = mod_entry.SelectToken( "name" );
						JToken mod_vers_raw_token = mod_entry.SelectToken( "version" );
						if( mod_name_token == null || mod_vers_raw_token == null ) {
							continue;
						}

						string mod_name = mod_name_token.ToObject<string>();
						string mod_vers_raw = mod_vers_raw_token.ToObject<string>();

						Version mod_vers = Version.Parse( mod_vers_raw.Substring( 1 ) );

						mod_versions[ mod_name ] = mod_vers;
					}

					found = true;
				}

				on_success( mod_versions, found );
			};

			Action<Exception, string> on_fail = ( e, output ) => {
				if( e is JsonReaderException ) {
					LogHelpers.Log( "RetrieveLatestKnownVersions - Bad JSON: "+
						(output.Length > 256 ? output.Substring(0, 256) : output) );
				} else if( e is WebException || e is NullReferenceException ) {
					LogHelpers.Log( "RetrieveLatestKnownVersions - " + e.Message );
				} else {
					LogHelpers.Log( "RetrieveLatestKnownVersions - " + e.ToString() );
				}
			};

			NetHelpers.NetHelpers.MakeGetRequestAsync( ModVersionGet.ModVersionUrl, on_response, on_fail );
		}



		////////////////

		private IDictionary<string, Version> ModVersions = null;
	}
}
