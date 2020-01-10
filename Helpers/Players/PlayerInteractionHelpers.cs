using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player interactions with game things.
	/// </summary>
	public class PlayerInteractionHelpers {
		/// <summary>
		/// Detects a given point exists within the player's reach.
		/// </summary>
		/// <param name="tilePos"></param>
		/// <returns></returns>
		public static bool IsWithinTilePlacementReach( (int tileX, int tileY) tilePos ) {
			return PlayerInteractionHelpers.IsWithinTilePlacementReach( tilePos.tileX << 4, tilePos.tileY << 4 );
		}

		/// <summary>
		/// Detects a given point exists within the player's reach.
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static bool IsWithinTilePlacementReach( Vector2 worldPos ) {
			return PlayerInteractionHelpers.IsWithinTilePlacementReach( (int)worldPos.X, (int)worldPos.Y );
		}

		/// <summary>
		/// Detects a given point exists within the player's reach.
		/// </summary>
		/// <param name="worldPosX"></param>
		/// <param name="worldPosY"></param>
		/// <returns></returns>
		public static bool IsWithinTilePlacementReach( int worldPosX, int worldPosY ) {
			Player plr = Main.LocalPlayer;
			
			int plrLeftTileX = (int)plr.position.X >> 4;
			int plrTopTileY = (int)plr.position.Y >> 4;
			int plrRightTileX = ((int)plr.position.X + plr.width) >> 4;
			int plrBottomTileY = ((int)plr.position.Y + plr.height) >> 4;
			int minTileX = plrLeftTileX - Player.tileRangeX - plr.HeldItem.tileBoost - plr.blockRange;
			int maxTileX = plrRightTileX + Player.tileRangeX + plr.HeldItem.tileBoost + plr.blockRange - 1;
			int minTileY = plrTopTileY - Player.tileRangeY - plr.HeldItem.tileBoost - plr.blockRange;
			int maxTileY = plrBottomTileY + Player.tileRangeY + plr.HeldItem.tileBoost + plr.blockRange - 2;
			int minWldX = minTileX << 4;
			int maxWldX = maxTileX << 4;
			int minWldY = minTileY << 4;
			int maxWldY = maxTileY << 4;

			if( worldPosX > minWldX && worldPosX <= maxWldX ) {
				if( worldPosY > minWldY && worldPosY <= maxWldY ) {
					return true;
				}
			}
			return false;
		}
	}
}
