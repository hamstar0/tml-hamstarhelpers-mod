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
			int plrReachX = Player.tileRangeX << 4;
			int plrReachY = Player.tileRangeY << 4;

			int minX = (int)plr.position.X - plrReachX;
			int maxX = ((int)plr.position.X + plr.width) - plrReachX;
			int minY = (int)plr.position.Y - plrReachY;
			int maxY = ((int)plr.position.Y + plr.height) - plrReachY;

			if( worldPosX > minX && worldPosX <= maxX ) {
				if( worldPosY > minY && worldPosY <= maxY ) {
					return true;
				}
			}
			return false;
		}
	}
}
