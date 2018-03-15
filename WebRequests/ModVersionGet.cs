using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	public class ModVersionGet {
		private readonly static object MyLock = new object();



		public static void GetLatestKnownVersion( Mod mod, Action<Version> on_success, Action<Version> on_fail=null ) {
			var mymod = HamstarHelpersMod.Instance;

			Action check = delegate () {
				lock( ModVersionGet.MyLock ) {
					try {
						if( mymod.ModVersionGet.ModVersions.ContainsKey( mod.Name ) ) {
							on_success( mymod.ModVersionGet.ModVersions[mod.Name] );
						} else if( on_fail != null ) {
							on_fail( mod.Version );
						}
					} catch( Exception e ) {
						LogHelpers.Log( "Could not retrieve version for mod " + mod.DisplayName + ": " + e.ToString() );
						if( on_fail != null ) {
							on_fail( mod.Version );
						}
					}
				}
			};
			
			if( mymod.ModVersionGet.ModVersions == null ) {
				ModVersionGet.RetrieveLatestKnownVersions( delegate( IDictionary<string, Version> versions, bool found ) {
					if( found ) {
						mymod.ModVersionGet.ModVersions = versions;
					}
					check();
				} );
			} else {
				check();
			}
			
			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}



		private static void RetrieveLatestKnownVersions( Action<IDictionary<string, Version>, bool> on_success ) {
			IDictionary<string, Version> mod_versions = new Dictionary<string, Version>();
			string url = "https://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5";

			Action<string> on_response = delegate ( string output ) {
				bool found = false;

				JObject resp_json = JObject.Parse( output );

				if( resp_json.Count > 0 ) {
					JToken[] mod_list = resp_json.SelectToken( "modlist" ).ToArray();

					foreach( JToken mod_entry in mod_list ) {
						string mod_name = mod_entry.SelectToken( "name" ).ToObject<string>();
						string mod_vers_raw = mod_entry.SelectToken( "version" ).ToObject<string>();
						Version mod_vers = Version.Parse( mod_vers_raw.Substring( 1 ) );

						mod_versions[mod_name] = mod_vers;
					}

					found = true;
				}

				on_success( mod_versions, found );
			};
			Action<Exception> on_fail = delegate ( Exception e ) {
				LogHelpers.Log( "RetrieveLatestKnownVersions - " + e.ToString() );
			};

			NetHelpers.NetHelpers.MakeGetRequestAsync( url, on_response, on_fail );
		}



		////////////////

		private IDictionary<string, Version> ModVersions = null;
	}
}
