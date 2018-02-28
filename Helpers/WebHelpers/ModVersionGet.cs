using HamstarHelpers.DebugHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Terraria.ModLoader;


namespace HamstarHelpers.WebHelpers {
	public class ModVersionGet {
		public static Version GetLatestKnownVersion( Mod mod, out bool found ) {
			found = false;
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.ModVersionGet.ModVersions == null ) {
				mymod.ModVersionGet.ModVersions = ModVersionGet.RetrieveLatestKnownVersions( out found );
			} else {
				found = true;
			}

			if( mymod.ModVersionGet.ModVersions.ContainsKey(mod.Name) ) {
				return mymod.ModVersionGet.ModVersions[mod.Name];
			}

			return mod.Version;
			//string username = ModMetaDataManager.GetGithubUserName( mod );
			//string projname = ModMetaDataManager.GetGithubProjectName( mod );
			//string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
		}


		private static IDictionary<string, Version> RetrieveLatestKnownVersions( out bool is_loaded ) {
			IDictionary<string, Version> mod_versions = new Dictionary<string, Version>();
			is_loaded = false;

			string url = "https://script.googleusercontent.com/macros/echo?user_content_key=Owhg1llbbzrzST1eMJvfeO2IxGCHpigWMQZOsv1llpGT7ySYkY8EIxaJk0AVD_8Aegr6CiO9znq24nrES8NyTgg99q5WPQbwm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnBSjTGNl2m1Kws9l1N8jgtgHBs4_KqXHF12fqfuynNZuDJVLqqr8NLJ1-kzKtsTLVrxy_u9Yn2NR&lib=MLDmsgwwdl8rHsa0qIkfykg_ahli_ZfP5";
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
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

			return mod_versions;
		}



		////////////////

		private IDictionary<string, Version> ModVersions = null;
	}
}
