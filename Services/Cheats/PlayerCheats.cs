using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


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




	public partial class PlayerCheats {
		public static void ToggleCheats( Player player, CheatModeType cheats ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClient( player.whoAmI, cheats );
			} else {
				var myplayer = player.GetModPlayer<ModHelpersPlayer>();
				myplayer.Logic.ToggleCheats( cheats );
			}
		}

		public static bool TryGetCheatFlags( string[] cheats, out CheatModeType cheatFlags ) {
			cheatFlags = 0;
			foreach( string cheat in cheats ) {
				switch( cheat ) {
				case "bilbo":
					cheatFlags = (CheatModeType)((int)cheatFlags + (int)CheatModeType.BilboMode);
					break;
				case "god":
					cheatFlags = (CheatModeType)((int)cheatFlags + (int)CheatModeType.GodMode);
					break;
				case "mdk":
					cheatFlags = (CheatModeType)((int)cheatFlags + (int)CheatModeType.MDKMode);
					break;
				case "fly":
					cheatFlags = (CheatModeType)((int)cheatFlags + (int)CheatModeType.FlyMode);
					break;
				default:
					return false;
				}
			}
			return true;
		}

		public static IList<string> OutputActiveCheats( Player player ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			return PlayerCheats.OutputCheatFlags( myplayer.Logic.ActiveCheats );
		}

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
