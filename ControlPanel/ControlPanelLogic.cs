using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.ControlPanel {
	struct ModIssueReport {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	class ControlPanelLogic {
		public Mod CurrentMod = null;


		////////////////

		public ControlPanelLogic() { }

		////////////////

		public IEnumerable<Mod> GetMods() {
			var mods = new LinkedList<Mod>();
			var mod_set = new HashSet<string>();

			mods.AddLast( HamstarHelpersMod.Instance );
			mod_set.Add( HamstarHelpersMod.Instance.Name );

			foreach( var kv in ExtendedModManager.ConfigMods ) {
				if( kv.Key == HamstarHelpersMod.Instance.Name || kv.Value.File == null ) { continue; }
				mods.AddLast( kv.Value );
				mod_set.Add( kv.Value.Name );
			}

			foreach( var mod in ModLoader.LoadedMods ) {
				if( mod_set.Contains( mod.Name ) || mod.File == null ) { continue; }
				mods.AddLast( mod );
			}

			return mods;
		}

		////////////////

		public void SetCurrentMod( Mod mod ) {
			this.CurrentMod = mod;
		}


		////////////////
		
		public void ReportIssue( Mod mod, string issue ) {
			if( !ExtendedModManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}
			IEnumerable<Mod> mods = this.GetMods();
			
			//string url = "http://localhost:12347/issue_submit/";
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "Issue for " + mod.DisplayName + " " + mod.Version.ToString();
			string body = "Mods: "+string.Join(", ", mods.Select( m => m.DisplayName + " " + m.Version.ToString() ).ToArray())
				+ '\n'+" "+'\n' + issue;

			var json = new ModIssueReport {
				githubuser = ExtendedModManager.GetGithubUserName( mod ),
				githubproject = ExtendedModManager.GetGithubProjectName( mod ),
				title = title,
				body = body
			};
			string json_str = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
				request.Method = "POST";
				request.ContentType = "application/json";   //"application/vnd.github.v3+json";
				request.ContentLength = json_bytes.Length;
				request.UserAgent = "tModLoader "+ModLoader.version.ToString();

				using( Stream data_stream = request.GetRequestStream() ) {
					data_stream.Write( json_bytes, 0, json_bytes.Length );
					data_stream.Close();
				}

				WebResponse resp = request.GetResponse();
				byte[] resp_data = new byte[resp.ContentLength];
				Stream resp_data_stream = resp.GetResponseStream();
				resp_data_stream.Read( resp_data, 0, resp_data.Length );
				string resp_str = Encoding.UTF8.GetString( resp_data );
				object obj = JsonConvert.DeserializeObject( resp_str );

				JObject resp_json = JObject.Parse( resp_str );
				string msg = resp_json["Msg"].ToObject<string>();

				Main.NewText( "Issue submit result: "+msg, Color.Yellow );
			} catch( Exception e ) {
				ErrorLogger.Log( "Issue submit error: " + e.ToString() );
			}
		}


		public void ApplyConfigChanges() {
			ExtendedModManager.ReloadAllConfigsFromFile();
		}
	}
}
