using Microsoft.Xna.Framework;
using System;
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




	public class PlayerCheats {
		public static void ToggleBilboMode( Player player ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClient( player.whoAmI, CheatModeType.BilboMode );
			} else {
			}
		}

		public static void ToggleDegreelessnessMode( Player player ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClient( player.whoAmI, CheatModeType.GodMode );
			} else {
			}
		}

		public static void ToggleMDKMode( Player player ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClient( player.whoAmI, CheatModeType.MDKMode );
			} else {
			}
		}

		public static void ToggleGhostMode( Player player ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerCheatModeProtocol.SendToClient( player.whoAmI, CheatModeType.FlyMode );
			} else {
			}
		}

		public static void TeleportTo( Player player, int tileX, int tileY ) {
			f
		}
	}
}
