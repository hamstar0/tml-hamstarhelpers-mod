using HamstarHelpers.TmlHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Web {
	public class ModVersionGet {
		public static Version GetLatestKnownVersion( Mod mod, out bool found ) {
			found = false;

			if( !ModMetaDataManager.HasGithub( mod ) ) {
				return mod.Version;
			}

			string username = ModMetaDataManager.GetGithubUserName( mod );
			string projname = ModMetaDataManager.GetGithubProjectName( mod );
			string url = "https://api.github.com/repos/" + username + "/" + projname + "/releases";
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "GET";
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

			WebResponse resp = request.GetResponse();
			string resp_data;

			using( Stream resp_data_stream = resp.GetResponseStream() ) {
				var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
				resp_data = stream_read.ReadToEnd();
				resp_data_stream.Close();
			}
			
			try {
				JArray resp_json = JArray.Parse( resp_data );
				if( resp_json.Count == 0 ) {
					return mod.Version;
				}

				JToken tag_name_tok = resp_json[0].SelectToken( "tag_name" );
				if( tag_name_tok == null ) {
					return mod.Version;
				}

				string tag_name = tag_name_tok.ToObject<string>();
				var vers = Version.Parse( tag_name.Substring(1) );

				found = true;

				return vers;
			} catch( Exception _ ) {
				return mod.Version;
			}
		}
	}
}
