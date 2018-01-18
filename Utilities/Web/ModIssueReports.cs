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


namespace HamstarHelpers.Utilities.Web {
	public struct ModIssueReportData {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	public static class ModIssueReports {
		public static string ReportIssue( Mod mod, string issue_title, string issue_body ) {
			if( !ModMetaDataManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}
			IEnumerable<Mod> mods = ModHelpers.GetAllMods();

			//string url = "http://localhost:12347/issue_submit/";
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "In-game: " + issue_title;
			string body = ModIssueReports.OutputGameData( mods );
			body += "\n \n \n \n" + "Recent error logs:\n" + ModIssueReports.OutputErrorLog();
			body += "\n \n \n \n" + issue_body;

			var json = new ModIssueReportData {
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
			JToken data = resp_json.SelectToken( "Data.html_url" );
			JToken msg = resp_json.SelectToken( "Msg" );

			/*if( data != null ) {
				string post_at_url = data.ToObject<string>();
				if( !string.IsNullOrEmpty( post_at_url ) ) {
					System.Diagnostics.Process.Start( post_at_url );
				}
			}*/

			if( msg != null ) {
				return msg.ToObject<string>();
			}
			return "Failure.";
		}


		////////////////

		public static string OutputGameData( IEnumerable<Mod> mods ) {
			var mods_list = mods.OrderBy( m => m.Name ).Select( m => m.DisplayName + " " + m.Version.ToString() );

			string data = "Mods: " + string.Join( ", ", mods_list.ToArray() );
			data += "\n \n" + "Is day: " + Main.dayTime + ", Time of day/night: " + Main.time;  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			data += "\n \n" + "World name: " + Main.worldName + ", world size: " + WorldHelpers.WorldHelpers.GetSize().ToString();
			data += "\n \n" + "World progress: " + string.Join( ", ", ModIssueReports.OutputWorldProgress().ToArray() );
			data += "\n \n" + "Items on ground: " + ItemHelpers.ItemHelpers.GetActive().Count + ", Npcs active: " + NPCHelpers.NPCHelpers.GetActive().Count;
			data += "\n \n" + "Player info: " + string.Join( ", ", ModIssueReports.OutputCurrentPlayerInfo().ToArray() );
			data += "\n \n" + "Player equips: " + string.Join( ", ", ModIssueReports.OutputCurrentPlayerEquipment().ToArray() );
			if( Main.netMode != 0 ) {
				string netmode = Main.netMode == 0 ? "single-player" : "multiplayer";
				data += "\n \n" + "Player count: " + Main.ActivePlayersCount + " ("+netmode+")";
			}

			return data;
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

		public static string OutputErrorLog() {
			IList<string> lines = new List<string>();
			char sep = Path.DirectorySeparatorChar;
			string path = Main.SavePath + sep + "Logs" + sep + "Logs.txt";

			if( !File.Exists( path ) ) { return "No error logs available."; }

			using( var reader = new StreamReader( path ) ) {
				int size = 1024;
				bool eof = false;

				do {
					lines = new List<string>( 50 );
					
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
				} while( lines.Count < 25 && !eof );
			}

			IList<string> rev_lines = lines.Reverse().Take( 25 ).ToList();
			if( lines.Count > 25 ) { rev_lines.Add( "..." ); }

			string[] lines_arr = rev_lines.Reverse().ToArray();

			return "```\n" + string.Join( "\n", lines_arr.ToArray() ) + "\n```";
		}
	}
}
