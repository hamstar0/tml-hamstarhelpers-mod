using HamstarHelpers.PlayerHelpers;
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


namespace HamstarHelpers.Commands {
	public struct ModIssueReport {
		public string githubuser;
		public string githubproject;
		public string title;
		public string body;
		//public string[] labels;
	}



	public class ModIssueReportCommand : ModCommand {
		public static string ReportIssue( Mod mod, string issue_title, string issue_body ) {
			if( !ModMetaDataManager.HasGithub( mod ) ) {
				throw new Exception( "Mod is not eligable for submitting issues." );
			}
			IEnumerable<Mod> mods = ModMetaDataManager.GetAllMods();

			//string url = "http://localhost:12347/issue_submit/";
			string url = "http://hamstar.pw/hamstarhelpers/issue_submit/";
			string title = "In-game: " + issue_title;
			string body = ModIssueReportCommand.OutputGameData( mods );
			body += "\n \n" + issue_body;

			var json = new ModIssueReport {
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
			string msg = resp_json["Msg"].ToObject<string>();

			return "Issue submit result: " + msg;
		}


		////////////////

		public static string OutputGameData( IEnumerable<Mod> mods ) {
			string data = "Mods: " + string.Join( ", ", mods.Select( m => m.DisplayName + " " + m.Version.ToString() ).ToArray() );
			//string data = mods.Select( m => m.DisplayName + " " + m.Version.ToString() ).Aggregate( ( all, next ) => all + ", " + next );
			data += "\n \n" + "Is day: " + Main.dayTime + ", Time of day/night: " + Main.time + ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			data += "\n \n" + "World name: " + Main.worldName + ", world size: " + WorldHelpers.WorldHelpers.GetSize().ToString();
			data += "\n \n" + "World progress: " + string.Join( ", ", ModIssueReportCommand.OutputWorldProgress().ToArray() );
			data += "\n \n" + "Items on ground: " + ItemHelpers.ItemHelpers.GetActive().Count + ", Npcs active: " + NPCHelpers.NPCHelpers.GetActive().Count;
			data += "\n \n" + "Player info: " + string.Join( ", ", ModIssueReportCommand.OutputCurrentPlayerInfo().ToArray() );
			data += "\n \n" + "Player equips: " + string.Join( ", ", ModIssueReportCommand.OutputCurrentPlayerEquipment().ToArray() );
			if( Main.netMode != 0 ) {
				data += "\n \n" + "Player count: " + Main.ActivePlayersCount;
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

		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "hhmodissuereport"; } }
		public override string Usage { get { return "/hhmodissuereport 4 \"issue title\" \"issue description text\""; } }
		public override string Description { get { return "Reports an issue for a mod (setup to do so with Hamstar's Helpers)."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				throw new UsageException( "Command not available for clients.", Color.Red );
			}

			IList<Mod> mods = ModMetaDataManager.GetAllMods().ToList();
			int mod_idx, body_arg_idx;
			string title = this.ParseIssueTitle( args, out body_arg_idx );
			string body = this.ParseIssueDescription( body_arg_idx, args );

			if( !int.TryParse( args[0], out mod_idx ) ) {
				throw new UsageException( args[0] + " is not an integer", Color.Red );
			}
			if( mod_idx <= 0 || mod_idx > mods.Count ) {
				throw new UsageException( args[0] + " is out of range", Color.Red );
			}
			
			try {
				string output = ModIssueReportCommand.ReportIssue( mods[mod_idx - 1], title, body );
				caller.Reply( output, Color.White );
			} catch( Exception e ) {
				caller.Reply( e.Message, Color.Red );
			}
		}


		////////////////

		private string ParseIssueTitle( string[] args, out int body_arg_idx ) {
			if( args[1][0] != '"' ) {
				throw new UsageException( args[0] + " is not a valid issue report title string", Color.Red );
			}

			string title = args[1].Substring( 1 );
			bool is_termined = false;
			body_arg_idx = 1;

			for( int i=2; i<args.Length - 1; i++ ) {
				int arg_len = args[i].Length;
				if( args[i][arg_len-1] == '"' ) {
					is_termined = true;
					body_arg_idx = i+1;

					title += " " + args[i].Substring( 0, arg_len - 1 );
					break;
				} else {
					title += " " + args[i];
				}
			}

			if( !is_termined ) {
				throw new UsageException( args[0] + " is not a valid issue report title string", Color.Red );
			}

			return title;
		}


		private string ParseIssueDescription( int body_arg_idx, string[] args ) {
			if( args[body_arg_idx][0] != '"' ) {
				throw new UsageException( args[0] + " is not a valid issue report description string", Color.Red );
			}

			string body = args[body_arg_idx].Substring( 1 );
			bool is_termined = false;

			for( int i = body_arg_idx+1; i < args.Length; i++ ) {
				int arg_len = args[i].Length;
				if( args[i][arg_len - 1] == '"' ) {
					is_termined = true;

					body += " " + args[i].Substring( 0, arg_len - 1 );
					break;
				} else {
					body += " " + args[i];
				}
			}

			if( !is_termined ) {
				throw new UsageException( args[0] + " is not a valid issue report description string", Color.Red );
			}

			return body;
		}
	}
}
