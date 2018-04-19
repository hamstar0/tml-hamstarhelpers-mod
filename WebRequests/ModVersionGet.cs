using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	public class ModVersionGet {
		private readonly static object MyLock = new object();
		
		public static string ModVersionUrl { get { return "://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5"; } }


		
		////////////////

		public static void GetLatestKnownVersionAsync( Mod mod, Action<Version> on_success, Action<Exception> on_fail ) {
			Action check = delegate () {
				var mymod = HamstarHelpersMod.Instance;

				try {
					if( mymod.ModVersionGet.ModVersions.ContainsKey( mod.Name ) ) {
						on_success( mymod.ModVersionGet.ModVersions[ mod.Name ] );
					} else {
						throw new Exception( "GetLatestKnownVersion - Unrecognized mod" );
					}
				} catch( Exception e ) {
					on_fail( e );
				}
			};

			ThreadPool.QueueUserWorkItem( _ => {
				var mymod = HamstarHelpersMod.Instance;

				lock( ModVersionGet.MyLock ) {
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
					JToken[] mod_list = resp_json.SelectToken( "modlist" ).ToArray();

					foreach( JToken mod_entry in mod_list ) {
						string mod_name = mod_entry.SelectToken( "name" ).ToObject<string>();
						string mod_vers_raw = mod_entry.SelectToken( "version" ).ToObject<string>();
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
						(output.Length>256?output.Substring(0, 256) : output) );
				} else {
					LogHelpers.Log( "RetrieveLatestKnownVersions - " + e.ToString() );
				}
			};

			NetHelpers.NetHelpers.MakeGetRequestAsync( ModVersionGet.ModVersionUrl, on_response, on_fail );
		}



		////////////////

		private IDictionary<string, Version> ModVersions = null;



		////////////////

		[System.Obsolete( "use ModVersionGet.GetLatestKnownVersionAsync", true )]
		public static void GetLatestKnownVersion( Mod mod, Action<Version> on_success, Action<Version> on_fail = null ) {
			ModVersionGet.GetLatestKnownVersionAsync( mod, on_success, delegate ( Exception _ ) {
				if( on_fail != null ) {
					on_fail( mod.Version );
				}
			} );
		}
	}
}
