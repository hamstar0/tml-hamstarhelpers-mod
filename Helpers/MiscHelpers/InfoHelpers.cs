using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.NPCHelpers;
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

			IList<string> outList = new List<string>();

			if( ( flags & VanillaEventFlag.Goblins ) > 0 ) {
				flags -= VanillaEventFlag.Goblins;
				outList.Add( "Goblins" );
			}
			if( ( flags & VanillaEventFlag.FrostLegion ) > 0 ) {
				flags -= VanillaEventFlag.FrostLegion;
				outList.Add( "Frost Legion" );
			}
			if( ( flags & VanillaEventFlag.Pirates ) > 0 ) {
				flags -= VanillaEventFlag.Pirates;
				outList.Add( "Pirates" );
			}
			if( ( flags & VanillaEventFlag.Martians ) > 0 ) {
				flags -= VanillaEventFlag.Martians;
				outList.Add( "Martians" );
			}
			if( ( flags & VanillaEventFlag.BloodMoon ) > 0 ) {
				flags -= VanillaEventFlag.BloodMoon;
				outList.Add( "Blood Moon" );
			}
			if( ( flags & VanillaEventFlag.SlimeRain ) > 0 ) {
				flags -= VanillaEventFlag.SlimeRain;
				outList.Add( "Slime Rain" );
			}
			if( ( flags & VanillaEventFlag.Sandstorm ) > 0 ) {
				flags -= VanillaEventFlag.Sandstorm;
				outList.Add( "Sandstorm" );
			}
			if( ( flags & VanillaEventFlag.SolarEclipse ) > 0 ) {
				flags -= VanillaEventFlag.SolarEclipse;
				outList.Add( "Solar Eclipse" );
			}
			if( ( flags & VanillaEventFlag.PumpkinMoon ) > 0 ) {
				flags -= VanillaEventFlag.PumpkinMoon;
				outList.Add( "Pumpkin Moon" );
			}
			if( ( flags & VanillaEventFlag.FrostMoon ) > 0 ) {
				flags -= VanillaEventFlag.FrostMoon;
				outList.Add( "Frost Moon" );
			}
			if( ( flags & VanillaEventFlag.LunarApocalypse ) > 0 ) {
				flags -= VanillaEventFlag.LunarApocalypse;
				outList.Add( "Lunar Apocalypse" );
			}

			if( outList.Count == 0 ) {
				outList.Add( "Normal" );
			}

			return outList;
		}


		public static IList<string> GetGameData( IEnumerable<Mod> mods ) {
			var list = new List<string>();

			var modsList = mods.OrderBy( m => m.Name ).Select( m => m.DisplayName + " " + m.Version.ToString() );
			string[] modsArr = modsList.ToArray();
			bool isDay = Main.dayTime;
			double timeOfDay = Main.time;
			int halfDays = WorldHelpers.WorldStateHelpers.GetElapsedHalfDays();
			string worldSize = WorldHelpers.WorldHelpers.GetSize().ToString();
			string[] worldProg = InfoHelpers.GetWorldProgress().ToArray();
			int activeItems = ItemHelpers.ItemHelpers.GetActive().Count;
			int activeNpcs = NPCHelpers.NPCHelpers.GetActive().Count;
			string[] playerInfos = InfoHelpers.GetCurrentPlayerInfo().ToArray();
			string[] playerEquips = InfoHelpers.GetCurrentPlayerEquipment().ToArray();
			int activePlayers = Main.ActivePlayersCount;
			string netmode = Main.netMode == 0 ? "single-player" : "multiplayer";
			bool autopause = Main.autoPause;
			bool autosave = Main.autoSave;
			int lighting = Lighting.lightMode;
			int lightingThreads = Lighting.LightingThreads;
			int frameSkipMode = Main.FrameSkipMode;
			bool isMaximized = Main.screenMaximized;
			int windowWid = Main.screenWidth;
			int windowHei = Main.screenHeight;
			int qualityStyle = Main.qaStyle;
			bool bgOn = Main.BackgroundEnabled;
			bool childSafe = !ChildSafety.Disabled;
			float gameZoom = Main.GameZoomTarget;
			float uiZoom = Main.UIScale;

			list.Add( InfoHelpers.RenderModTable( modsArr ) );
			list.Add( "Is day: " + isDay + ", Time of day/night: " + timeOfDay + ", Elapsed half days: " + halfDays );  //+ ", Total time (seconds): " + Main._drawInterfaceGameTime.TotalGameTime.Seconds;
			list.Add( "World name: " + Main.worldName + ", world size: " + worldSize );
			list.Add( "World progress: " + (worldProg.Length > 0 ? string.Join(", ", worldProg) : "none") );
			list.Add( "Items on ground: " + activeItems + ", Npcs active: " + activeNpcs );
			list.Add( "Player info: " + string.Join( ", ", playerInfos ) );
			list.Add( "Player equips: " + (playerEquips.Length > 0 ? string.Join(", ", playerEquips) : "none" ) );
			list.Add( "Player count: " + activePlayers + " (" + netmode + ")" );
			list.Add( "Autopause: " + autopause );
			list.Add( "Autosave: " + autosave );
			list.Add( "Lighting mode: " + lighting );
			list.Add( "Lighting threads: " + lightingThreads );
			list.Add( "Frame skip mode: " + frameSkipMode );
			list.Add( "Is screen maximized: " + isMaximized );
			list.Add( "Screen resolution: " + windowWid + " " + windowHei );
			list.Add( "Quality style: " + qualityStyle );
			list.Add( "Background on: " + bgOn );
			list.Add( "Child safety: " + childSafe );
			list.Add( "Game zoom: " + gameZoom );
			list.Add( "UI zoom: " + uiZoom );

			return list;
		}

		public static string RenderModTable( string[] mods ) {
			mods = mods.Select( m => m.Replace( "|", "\\|" ) ).ToArray();
			
			int len = mods.Length;
			string output = "| Mods:  | - | - |";
			output += "\n| :--- | :--- | :--- |";

			for( int i=0; i<len; i++ ) {
				output += '\n';
				output += "| " + mods[i] + " | " + (++i<len ? mods[i] : "-") + " | " + (++i<len ? mods[i] : "-") + " |";
			}

			return output;
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
				throw new HamstarException("Server-side only.");
			}

			return ModHelpersMod.Instance.ServerInfo.AveragePing;
		}


		////////////////

		public static IList<string> GetErrorLog( int maxLines ) {
			if( maxLines > 150 ) { maxLines = 150; }

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
					lines = new List<string>( maxLines + 25 );

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
				} while( lines.Count < maxLines && !eof );
			}

			IList<string> revLines = lines.Reverse().Take( 25 ).ToList();
			if( lines.Count > maxLines ) { revLines.Add( "..." ); }

			return new List<string>( revLines.Reverse() );
		}
	}
}
