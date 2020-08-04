using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Buffs;


namespace HamstarHelpers.Services.Cheats {
	public partial class PlayerCheats {
		public static void TeleportTo( Player player, int tileX, int tileY ) {
			f
		}


		////////////////

		internal static void UpdateCheats( Player player, CheatModeType cheatFlags ) {
			//if( (cheatFlags & CheatModeType.BilboMode) != 0 ) {
			//}
			if( (cheatFlags & CheatModeType.GodMode) != 0 ) {
				PlayerCheats.UpdateCheatGodMode( player );
			}
			//if( (cheatFlags & CheatModeType.MDKMode) != 0 ) {
			//}
			if( (cheatFlags & CheatModeType.FlyMode) != 0 ) {
				PlayerCheats.UpdateCheatFlyMode( player );
			}
		}

		////

		private static void UpdateCheatGodMode( Player player ) {
			int buffType = ModContent.BuffType<DegreelessnessBuff>();
			int buffIdx = player.FindBuffIndex( buffType );
			if( buffIdx == -1 ) {
				player.AddBuff( buffType, 2 );
			} else {
				player.buffTime[ buffIdx ] = 2;
			}
		}

		private static void UpdateCheatFlyMode( Player player ) {
			if( player.wingTime > 0f ) {
				player.wingTime = 1f;
			} else if( player.rocketTime > 0 ) {
				player.rocketTime = 1;
			}
		}


		////////////////

		internal static void ModifyDrawLayers( List<PlayerLayer> layers, CheatModeType cheatFlags ) {
			if( (cheatFlags & CheatModeType.BilboMode) != 0 ) {
				foreach( PlayerLayer layer in layers ) {
					layer.visible = false;
				}
			}
		}
	}
}
