using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace HamstarHelpers.WebRequests {
	public class ModVersionGet {
		public static void GetLatestKnownVersion( Mod mod, Action<Version> on_success, Action<Version> on_fail=null ) {
			var mymod = HamstarHelpersMod.Instance;
			Action check = delegate () {
				if( mymod.ModVersionGet.ModVersions.ContainsKey( mod.Name ) ) {
					on_success( mymod.ModVersionGet.ModVersions[ mod.Name ] );
				} else if( on_fail != null ) {
					on_fail( mod.Version );
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

			NetHelpers.NetHelpers.MakeGetRequestAsync( url, delegate ( string output ) {
				bool found = false;

				try {
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
				} catch( Exception e ) {
					LogHelpers.Log( e.ToString() );
				}

				on_success( mod_versions, found );
			}, delegate( Exception e  ) {
				LogHelpers.Log( e.ToString() );
			} );

			/*HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			try {
				WebResponse resp = request.GetResponse();
				string resp_data;

				using( Stream resp_data_stream = resp.GetResponseStream() ) {
					var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
					resp_data = stream_read.ReadToEnd();
					resp_data_stream.Close();
				}


				JObject resp_json = JObject.Parse( resp_data );

				if( resp_json.Count > 0 ) {
					JToken[] mod_list = resp_json.SelectToken("modlist").ToArray();

					foreach( JToken mod_entry in mod_list ) {
						string mod_name = mod_entry.SelectToken("name").ToObject<string>();
						string mod_vers_raw = mod_entry.SelectToken("version").ToObject<string>();
						Version mod_vers = Version.Parse( mod_vers_raw.Substring(1) );

						mod_versions[mod_name] = mod_vers;
					}

					is_loaded = true;
				}
			} catch( Exception e ) {
				LogHelpers.Log( e.ToString() );
			}
			
			return mod_versions;*/
		}



		////////////////

		private IDictionary<string, Version> ModVersions = null;
	}
}
