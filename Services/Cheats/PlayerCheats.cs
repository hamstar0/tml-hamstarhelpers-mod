using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Internals.NetProtocols;


namespace HamstarHelpers.Services.Cheats {
	/// <summary></summary>
	public enum CheatModeType {
		/// <summary>
		/// Invisibility.
		/// </summary>
		BilboMode = 1,
		/// <summary>
		/// Invincibility.
		/// </summary>
		GodMode = 2,
		/// <summary>
		/// All weapons do extreme damage.
		/// </summary>
		MDKMode = 4,
		/// <summary>
		/// Noclip, speed, and flight.
		/// </summary>
		FlyMode = 8,
	}




	/// <summary>
	/// Provides APIs for toggling or applying player cheat effects.
	/// </summary>
	public partial class PlayerCheats {
		/// <summary>
		/// Toggles a given set of cheats.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="cheatFlags"></param>
		public static void ToggleCheats( Player player, CheatModeType cheatFlags ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClients( player, cheatFlags );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				PlayerCheatModeProtocol.BroadcastFromClient( cheatFlags );
			}
			
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			CheatModeType currCheats = myplayer.Logic.ActiveCheats;
			
			if( (currCheats & cheatFlags) == cheatFlags ) {
				currCheats = (CheatModeType)((int)currCheats - (int)cheatFlags );
			} else {
				currCheats = currCheats & cheatFlags;
			}
			myplayer.Logic.SetCheats( currCheats );
		}


		////////////////

		/// <summary>
		/// Attempts to parse cheat flags from a textual string list of cheat names.
		/// </summary>
		/// <param name="cheats"></param>
		/// <param name="cheatFlags"></param>
		/// <returns>`true` if all cheats are valid.</returns>
		public static bool TryParseCheatFlags( string[] cheats, out CheatModeType cheatFlags ) {
			cheatFlags = 0;
			foreach( string cheat in cheats ) {
				switch( cheat ) {
				case "bilbo":
					cheatFlags |= CheatModeType.BilboMode;
					break;
				case "god":
					cheatFlags |= CheatModeType.GodMode;
					break;
				case "mdk":
					cheatFlags |= CheatModeType.MDKMode;
					break;
				case "fly":
					cheatFlags |= CheatModeType.FlyMode;
					break;
				default:
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Gets a string list representation of a player's active cheats.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static IList<string> OutputActiveCheats( Player player ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			return PlayerCheats.OutputCheatFlags( myplayer.Logic.ActiveCheats );
		}

		/// <summary>
		/// Gets a string list representation of a given set of cheats.
		/// </summary>
		/// <param name="cheatFlags"></param>
		/// <returns></returns>
		public static IList<string> OutputCheatFlags( CheatModeType cheatFlags ) {
			var output = new List<string>();

			if( (cheatFlags & CheatModeType.BilboMode) != 0 ) {
				output.Add( "bilbo" );
			}
			if( (cheatFlags & CheatModeType.GodMode) != 0 ) {
				output.Add( "god" );
			}
			if( (cheatFlags & CheatModeType.MDKMode) != 0 ) {
				output.Add( "mdk" );
			}
			if( (cheatFlags & CheatModeType.FlyMode) != 0 ) {
				output.Add( "fly" );
			}

			return output;
		}
	}
}
