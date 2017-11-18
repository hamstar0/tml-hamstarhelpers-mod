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
		
		public void ReportIssue( Mod mod, string issue_title, string issue_body ) {
			if( !ExtendedModManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}
			IEnumerable<Mod> mods = this.GetMods();
			
			//string url = "http://localhost:12347/issue_submit/";
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "In-game report: " + issue_title;
			string body = this.OutputGameData( mods );
			body += "\n \n" + issue_body;

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
				string resp_data;

				using( Stream resp_data_stream = resp.GetResponseStream() ) {
					var stream_read = new StreamReader( resp_data_stream, Encoding.UTF8 );
					resp_data = stream_read.ReadToEnd();
					resp_data_stream.Close();
				}
				
				JObject resp_json = JObject.Parse( resp_data );
				string msg = resp_json["Msg"].ToObject<string>();

				Main.NewText( "Issue submit result: "+msg, Color.Yellow );
				ErrorLogger.Log( "Issue submit result: " + msg );
			} catch( Exception e ) {
				ErrorLogger.Log( "Issue submit error: " + e.ToString() );
				Main.NewText( "Issue submit error: "+e.ToString(), Color.Red );
			}
		}


		public void ApplyConfigChanges() {
			foreach( var kv in ExtendedModManager.ConfigMods ) {
				ExtendedModManager.ReloadConfigFromFile( kv.Value );
			}

			string mod_names = string.Join( ", ", ExtendedModManager.ConfigMods.Keys.ToArray() );

			Main.NewText( "Mod configs reloaded for " + mod_names, Color.Yellow );
			ErrorLogger.Log( "Mod configs reloaded for " + mod_names );
		}


		////////////////

		public string OutputGameData( IEnumerable<Mod> mods ) {
			string data = "Mods: " + string.Join( ", ", mods.Select( m => m.DisplayName + " " + m.Version.ToString() ).ToArray() );
			//string data = mods.Select( m => m.DisplayName + " " + m.Version.ToString() ).Aggregate( ( all, next ) => all + ", " + next );
			data += "\n \n" + "Is day: " + Main.dayTime + ", Time of day/night: " + Main.time + ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			data += "\n \n" + "World name: " + Main.worldName + ", world size: " + WorldHelpers.WorldHelpers.GetSize().ToString();
			data += "\n \n" + "World progress: " + string.Join( ", ", this.OutputWorldProgress().ToArray() );
			data += "\n \n" + "Items on ground: " + ItemHelpers.ItemHelpers.GetActive().Count + ", Npcs active: " + NPCHelpers.NPCHelpers.GetActive().Count;
			if( Main.netMode != 0 ) {
				data += "\n \n" + "Player count: " + Main.ActivePlayersCount;
			}

			return data;
		}
		
		public IList<string> OutputWorldProgress() {
			var list = new List<string>();

			if( NPC.downedBoss1 ) { list.Add("Eye of Cthulhu killed"); }
			if( NPC.downedSlimeKing ) { list.Add( "King Slime killed" ); }
			if( NPC.downedQueenBee ) { list.Add( "Queen Bee killed" ); }
			if( NPC.downedBoss2 && !WorldGen.crimson ) { list.Add( "Eater of Worlds killed" ); }
			if( NPC.downedBoss2 && WorldGen.crimson ) { list.Add( "Brain of Cthulhu killed" ); }
			if( NPC.downedBoss3 ) { list.Add( "Skeletron killed" ); }
			if( Main.hardMode ) { list.Add( "Wall of Flesh killed" ); }
			if( NPC.downedMechBoss1 ) { list.Add( "Destroyer killed" ); }
			if( NPC.downedMechBoss2 ) { list.Add( "Twins killed" ); }
			if( NPC.downedMechBoss3 ) { list.Add( "Skeletron Prime killed" ); }
			if( NPC.downedPlantBoss ) { list.Add( "Plantera killed" ); }
			if( NPC.downedGolemBoss ) { list.Add( "Golem killed" ); }
			if( NPC.downedFishron ) { list.Add( "Duke Fishron killed" ); }
			if( NPC.downedAncientCultist ) { list.Add( "Ancient Cultist killed" ); }
			if( NPC.downedMoonlord ) { list.Add( "Moon Lord killed" ); }

			return list;
		}
	}
}
