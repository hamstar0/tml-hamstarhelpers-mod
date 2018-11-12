﻿using HamstarHelpers.Helpers.NPCHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public static partial class InfoHelpers {
		public static string GetVanillaProgress() {
			if( NPC.downedMoonlord ) {
				return "Post Moon Lord";
			}
			if( NPC.LunarApocalypseIsUp ) {
				return "Lunar apocalypse";
			}
			if( NPC.downedGolemBoss ) {
				return "Post Golem";
			}
			if( NPC.downedPlantBoss ) {
				return "Post Plantera";
			}
			if( NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 ) {
				return "Post Mech Bosses";
			}
			if( Main.hardMode ) {
				return "Hard mode";
			}
			if( NPC.downedBoss2 ) {
				if( WorldGen.crimson ) {
					return "Post Brain of Cthulhu";
				} else {
					return "Post Eater of Worlds";
				}
			}
			if( NPC.downedBoss3 ) {
				return "Post Skeletron";
			}
			if( NPC.downedQueenBee ) {
				return "Post Queen Bee";
			}
			if( NPC.downedBoss1 || NPC.downedSlimeKing ) {
				return "Boss killing begun";
			}
			if( NPC.downedGoblins ) {
				return "Goblins killed";
			}
			return "Start of game";
		}


		public static IList<string> GetCurrentVanillaEvents() {
			VanillaEventFlag flags = NPCInvasionHelpers.GetCurrentEventTypeSet();

			IList<string> out_list = new List<string>();

			if( ( flags & VanillaEventFlag.Goblins ) > 0 ) {
				flags -= VanillaEventFlag.Goblins;
				out_list.Add( "Goblins" );
			}
			if( ( flags & VanillaEventFlag.FrostLegion ) > 0 ) {
				flags -= VanillaEventFlag.FrostLegion;
				out_list.Add( "Frost Legion" );
			}
			if( ( flags & VanillaEventFlag.Pirates ) > 0 ) {
				flags -= VanillaEventFlag.Pirates;
				out_list.Add( "Pirates" );
			}
			if( ( flags & VanillaEventFlag.Martians ) > 0 ) {
				flags -= VanillaEventFlag.Martians;
				out_list.Add( "Martians" );
			}
			if( ( flags & VanillaEventFlag.BloodMoon ) > 0 ) {
				flags -= VanillaEventFlag.BloodMoon;
				out_list.Add( "Blood Moon" );
			}
			if( ( flags & VanillaEventFlag.SlimeRain ) > 0 ) {
				flags -= VanillaEventFlag.SlimeRain;
				out_list.Add( "Slime Rain" );
			}
			if( ( flags & VanillaEventFlag.Sandstorm ) > 0 ) {
				flags -= VanillaEventFlag.Sandstorm;
				out_list.Add( "Sandstorm" );
			}
			if( ( flags & VanillaEventFlag.SolarEclipse ) > 0 ) {
				flags -= VanillaEventFlag.SolarEclipse;
				out_list.Add( "Solar Eclipse" );
			}
			if( ( flags & VanillaEventFlag.PumpkinMoon ) > 0 ) {
				flags -= VanillaEventFlag.PumpkinMoon;
				out_list.Add( "Pumpkin Moon" );
			}
			if( ( flags & VanillaEventFlag.FrostMoon ) > 0 ) {
				flags -= VanillaEventFlag.FrostMoon;
				out_list.Add( "Frost Moon" );
			}
			if( ( flags & VanillaEventFlag.LunarApocalypse ) > 0 ) {
				flags -= VanillaEventFlag.LunarApocalypse;
				out_list.Add( "Lunar Apocalypse" );
			}

			if( out_list.Count == 0 ) {
				out_list.Add( "Normal" );
			}

			return out_list;
		}


		public static IList<string> GetGameData( IEnumerable<Mod> mods ) {
			var list = new List<string>();

			var mods_list = mods.OrderBy( m => m.Name ).Select( m => m.DisplayName + " " + m.Version.ToString() );
			string[] mods_arr = mods_list.ToArray();
			bool is_day = Main.dayTime;
			double time_of_day = Main.time;
			int half_days = WorldHelpers.WorldStateHelpers.GetElapsedHalfDays();
			string world_size = WorldHelpers.WorldHelpers.GetSize().ToString();
			string[] world_prog = InfoHelpers.GetWorldProgress().ToArray();
			int active_items = ItemHelpers.ItemHelpers.GetActive().Count;
			int active_npcs = NPCHelpers.NPCHelpers.GetActive().Count;
			string[] player_infos = InfoHelpers.GetCurrentPlayerInfo().ToArray();
			string[] player_equips = InfoHelpers.GetCurrentPlayerEquipment().ToArray();
			int active_players = Main.ActivePlayersCount;
			string netmode = Main.netMode == 0 ? "single-player" : "multiplayer";
			bool autopause = Main.autoPause;
			bool autosave = Main.autoSave;
			int lighting = Lighting.lightMode;
			int lighting_threads = Lighting.LightingThreads;
			int frame_skip_mode = Main.FrameSkipMode;
			bool is_maximized = Main.screenMaximized;
			int window_wid = Main.screenWidth;
			int window_hei = Main.screenHeight;
			int quality_style = Main.qaStyle;
			bool bg_on = Main.BackgroundEnabled;
			bool child_safe = !ChildSafety.Disabled;
			float game_zoom = Main.GameZoomTarget;
			float ui_zoom = Main.UIScale;

			list.Add( "Mods: " + string.Join( ", ", mods_arr ) );
			list.Add( "Is day: " + is_day + ", Time of day/night: " + time_of_day + ", Elapsed half days: " + half_days );  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			list.Add( "World name: " + Main.worldName + ", world size: " + world_size );
			list.Add( "World progress: " + string.Join( ", ", world_prog ) );
			list.Add( "Items on ground: " + active_items + ", Npcs active: " + active_npcs );
			list.Add( "Player info: " + string.Join( ", ", player_infos ) );
			list.Add( "Player equips: " + string.Join( ", ", player_equips ) );
			list.Add( "Player count: " + active_players + " (" + netmode + ")" );
			list.Add( "Autopause: " + autopause );
			list.Add( "Autosave: " + autosave );
			list.Add( "Lighting mode: " + lighting );
			list.Add( "Lighting threads: " + lighting_threads );
			list.Add( "Frame skip mode: " + frame_skip_mode );
			list.Add( "Is screen maximized: " + is_maximized );
			list.Add( "Screen resolution: " + window_wid+" "+window_hei );
			list.Add( "Quality style: " + quality_style );
			list.Add( "Background on: " + bg_on );
			list.Add( "Child safety: " + child_safe );
			list.Add( "Game zoom: " + game_zoom );
			list.Add( "UI zoom: " + ui_zoom );

			return list;
		}

		public static IList<string> GetWorldProgress() {
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


		public static IList<string> GetCurrentPlayerInfo() {
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


		public static IList<string> GetCurrentPlayerEquipment() {
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

		public static int GetAveragePingOfServer() {
			if( Main.netMode != 2 ) {
				throw new Exception("Server-side only.");
			}

			return ModHelpersMod.Instance.ServerInfo.AveragePing;
		}


		////////////////

		public static IList<string> GetErrorLog( int max_lines ) {
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
