using System;
using System.Collections.Generic;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
		/// <summary>
		/// Counts tile types within the player's vicinity (used for checking biomes and stuff).
		/// </summary>
		/// <param name="midTileX"></param>
		/// <param name="midTileY"></param>
		/// <returns></returns>
		[Obsolete("needs work")]
		public static IDictionary<int, int> GetPlayerRangeTilesAt( int midTileX, int midTileY ) {   // TODO
			var tiles = new Dictionary<int, int>();

			float minScreenTileWidth = 50f;
			float minScreenTileHeight = 37.5f;
			float offscreenTiles = 23f;
			//if( Lighting.lightMode < 2 ) {
			//	offscreenTiles = 40;
			//} else {
			//	offscreenTiles = 23;
			//}

			int leftTileX = midTileX - (int)( ( offscreenTiles + minScreenTileWidth ) * 0.5f ) - 1;
			int topTileY = midTileY - (int)( ( offscreenTiles + minScreenTileHeight ) * 0.5f ) - 1;
			int rightTileX = midTileX + (int)( ( offscreenTiles + minScreenTileWidth ) * 0.5f ) + 1;
			int botTileY = midTileY + (int)( ( offscreenTiles + minScreenTileHeight ) * 0.5f ) + 1;
			leftTileX = Utils.Clamp<int>( leftTileX, 5, Main.maxTilesX - 1 );
			topTileY = Utils.Clamp<int>( topTileY, 5, Main.maxTilesY - 1 );
			rightTileX = Utils.Clamp<int>( rightTileX, 5, Main.maxTilesX - 1 );
			botTileY = Utils.Clamp<int>( botTileY, 5, Main.maxTilesY - 1 );

			//int width = lastLightX - firstLightX;
			//int height = lastLightY - firstLightY;
			//
			//int biomeOffsetX = Math.Abs( ( width - Main.zoneX ) / 2 );
			//int biomeOffsetY = Math.Abs( ( height - Main.zoneY ) / 2 );
			// //LogHelpers.Log("firstLightX:"+firstLightX+", firstLightY:"+firstLightY
			// //	+", lastLightX:"+lastLightX+", lastLightY:"+lastLightY
			// //	+", width:"+width+", height:"+height
			// //	+", biomeOffsetX:"+biomeOffsetX+", biomeOffsetY:"+biomeOffsetX);

			Tile tile;
			for( int x = leftTileX; x < rightTileX; x++ ) {
				for( int y = topTileY; y < botTileY; y++ ) {
					tile = Main.tile[x, y];
					if( tile == null || !tile.active() ) { continue; }

					if( !tiles.ContainsKey( tile.type ) ) {
						tiles[tile.type] = 1;
					} else {
						tiles[tile.type] += 1;
					}
				}
			}

			return tiles;
		}


		////////////////

		/// <summary>
		/// Attempts to randomly search within a spherical area (via. `radius`) for any matching tiles (via. `pattern`).
		/// Checks for non-zero tile "brightness" to skip over unloaded tiles.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="radius"></param>
		/// <param name="retries"></param>
		/// <param name="skipDarkness"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if tile match found.</returns>
		public static bool FindNearbyRandomMatch(
					TilePattern pattern,
					int tileX,
					int tileY,
					int radius,
					int retries,
					bool skipDarkness,
					out int foundX, out int foundY ) {
			if( tileX + radius <= 0 || tileX - radius >= Main.mapMaxX ) {
				foundX = 0;
				foundY = 0;
				return false;
			}
			if( tileY + radius <= 0 || tileY - radius >= Main.mapMaxY ) {
				foundX = 0;
				foundY = 0;
				return false;
			}

			int wtf = 0;
			do {
				do {
					foundX = Main.rand.Next( -radius, radius ) + tileX;
				} while( foundX <= 0 || foundX >= Main.mapMaxX );
				do {
					foundY = Main.rand.Next( -radius, radius ) + tileY;
				} while( foundY <= 0 || foundY >= Main.mapMaxY );

				//tile = Main.tile[toX, toY];
				if( wtf++ > retries ) {
					foundX = 0;
					foundY = 0;
					return false;
				}
			} while( !pattern.Check(foundX, foundY) || (!skipDarkness && Lighting.Brightness(foundX, foundX) == 0) );

			return true;
		}
	}
}
