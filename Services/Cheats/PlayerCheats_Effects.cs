using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Buffs;
using HamstarHelpers.Helpers.Players;


namespace HamstarHelpers.Services.Cheats {
	/// <summary>
	/// Provides APIs for toggling or applying player cheat effects.
	/// </summary>
	public partial class PlayerCheats {
		/// <summary>
		/// Teleports a player to a given tile location. Negative tile values loop from the opposite respective side of
		/// the world.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		public static void TeleportTo( Player player, int tileX, int tileY ) {
			if( tileX < 0 ) {
				tileX = Main.maxTilesX - tileX;
			}
			if( tileY < 0 ) {
				tileY = Main.maxTilesY - tileY;
			}

			PlayerWarpHelpers.Teleport( player, new Vector2(tileX*16, tileY*16) );
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

		internal static void OnHit( ref int damage, CheatModeType cheatFlags ) {
			if( (cheatFlags & CheatModeType.MDKMode) != 0 ) {
				damage = 100000000;//0
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
