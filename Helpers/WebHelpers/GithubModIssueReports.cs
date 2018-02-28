using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.TmlHelpers.ModHelpers;
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


namespace HamstarHelpers.WebHelpers {
	public struct GithubModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	public static class GithubModIssueReports {
		public static string ReportIssue( Mod mod, string issue_title, string issue_body ) {
			if( !ModMetaDataManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}

			int max_lines = HamstarHelpersMod.Instance.Config.ModIssueReportErrorLogMaxLines;

			IEnumerable<Mod> mods = ModHelpers.GetAllMods();
			string body_info = string.Join( "\n \n", GithubModIssueReports.OutputGameData( mods ).ToArray() );
			string body_errors = string.Join( "\n", GithubModIssueReports.OutputErrorLog( max_lines ).ToArray() );
			
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "In-game: " + issue_title;
			string body = body_info;
			body += "\n \n \n \n" + "Recent error logs:\n```\n" + body_errors + "\n```";
			body += "\n \n" + issue_body;

			var json = new GithubModIssueReportData {
				githubuser = ModMetaDataManager.GetGithubUserName( mod ),
				githubproject = ModMetaDataManager.GetGithubProjectName( mod ),
				title = title,
				body = body
			};
			string json_str = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] json_bytes = Encoding.UTF8.GetBytes( json_str );

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
			request.Method = "POST";
			request.ContentType = "application/json";   //"application/vnd.github.v3+json";
			request.ContentLength = json_bytes.Length;
			request.UserAgent = "tModLoader " + ModLoader.version.ToString();

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
			//JToken data = resp_json.SelectToken( "Data.html_url" );
			JToken msg = resp_json.SelectToken( "Msg" );

			/*if( data != null ) {
				string post_at_url = data.ToObject<string>();
				if( !string.IsNullOrEmpty( post_at_url ) ) {
					SystemHelpers.Start( post_at_url );
				}
			}*/

			if( msg == null ) {
				return "Failure.";
			}
			return msg.ToObject<string>();
		}


		////////////////

		public static IList<string> OutputGameData( IEnumerable<Mod> mods ) {
			var list = new List<string>();

			var mods_list = mods.OrderBy( m => m.Name ).Select( m => m.DisplayName + " " + m.Version.ToString() );
			string[] mods_arr = mods_list.ToArray();
			bool is_day = Main.dayTime;
			double time_of_day = Main.time;
			int half_days = WorldHelpers.WorldHelpers.GetElapsedHalfDays();
			string world_size = WorldHelpers.WorldHelpers.GetSize().ToString();
			string[] world_prog = GithubModIssueReports.OutputWorldProgress().ToArray();
			int active_items = ItemHelpers.ItemHelpers.GetActive().Count;
			int active_npcs = NPCHelpers.NPCHelpers.GetActive().Count;
			string[] player_infos = GithubModIssueReports.OutputCurrentPlayerInfo().ToArray();
			string[] player_equips = GithubModIssueReports.OutputCurrentPlayerEquipment().ToArray();
			int active_players = Main.ActivePlayersCount;
			string netmode = Main.netMode == 0 ? "single-player" : "multiplayer";

			list.Add( "Mods: " + string.Join( ", ", mods_arr ) );
			list.Add( "Is day: " + is_day + ", Time of day/night: " + time_of_day + ", Elapsed half days: "+half_days );  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			list.Add( "World name: " + Main.worldName + ", world size: " + world_size );
			list.Add( "World progress: " + string.Join( ", ", world_prog ) );
			list.Add( "Items on ground: " + active_items + ", Npcs active: " + active_npcs );
			list.Add( "Player info: " + string.Join( ", ", player_infos ) );
			list.Add( "Player equips: " + string.Join( ", ", player_equips ) );
			list.Add( "Player count: " + active_players + " ("+netmode+")" );

			return list;
		}

		public static IList<string> OutputWorldProgress() {
			var list = new List<string>();

			if( NPC.downedBoss1 ) { list.Add( "Eye of Cthulhu killed" ); }
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


		public static IList<string> OutputCurrentPlayerInfo() {
			Player player = Main.LocalPlayer;

			string name = "Name: `" + player.name + "`";
			string gender = "Male: " + player.Male;
			string demon = "Demon Heart: " + player.extraAccessory;
			string difficulty = "Difficulty mode: " + player.difficulty;
			string life = "Life: " + player.statLife + " of " + player.statLifeMax2;
			if( player.statLifeMax != player.statLifeMax2 ) { life += " (" + player.statLifeMax + ")"; }
			string mana = "Mana: " + player.statMana + " of " + player.statManaMax2;
			if( player.statManaMax != player.statManaMax2 ) { life += " (" + player.statManaMax + ")"; }
			string def = "Defense: " + player.statDefense;

			return new List<string> { name, gender, demon, difficulty, life, mana, def };
		}


		public static IList<string> OutputCurrentPlayerEquipment() {
			Player player = Main.LocalPlayer;
			var list = new List<string>();

			for( int i = 0; i < player.armor.Length; i++ ) {
				string output = "";
				Item item = player.armor[i];
				if( item == null || item.IsAir ) { continue; }

				if( i == 0 ) {
					output += "Head: ";
				} else if( i == 1 ) {
					output += "Body: ";
				} else if( i == 2 ) {
					output += "Legs: ";
				} else if( PlayerItemHelpers.IsAccessorySlot( player, i ) ) {
					output += "Accessory: ";
				} else if( PlayerItemHelpers.IsVanitySlot( player, i ) ) {
					output += "Vanity: ";
				}

				output += item.HoverName;

				list.Add( output );
			}

			return list;
		}


		////////////////

		public static IList<string> OutputErrorLog( int max_lines ) {
			if( max_lines > 150 ) { max_lines = 150; }

			IList<string> lines = new List<string>();
			char sep = Path.DirectorySeparatorChar;
			string path = Main.SavePath + sep + "Logs" + sep + "Logs.txt";

			if( !File.Exists( path ) ) {
				return new List<string> { "No error logs available." };
			}

			using( var reader = new StreamReader( path ) ) {
				int size = 1024;
				bool eof = false;

				do {
					lines = new List<string>( max_lines + 25 );
					
					if( reader.BaseStream.Length > size ) {
						reader.BaseStream.Seek( -size, SeekOrigin.End );
						size += 1024;
					} else {
						eof = true;
						reader.BaseStream.Seek( 0, SeekOrigin.Begin );
					}

					string line;
					while( ( line = reader.ReadLine() ) != null ) {
						lines.Add( line );
					}
				} while( lines.Count < max_lines && !eof );
			}

			IList<string> rev_lines = lines.Reverse().Take( 25 ).ToList();
			if( lines.Count > max_lines ) { rev_lines.Add( "..." ); }
			
			return new List<string>( rev_lines.Reverse() );
		}
	}
}
