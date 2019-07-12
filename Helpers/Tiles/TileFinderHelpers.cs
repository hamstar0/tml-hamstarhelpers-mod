using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public class TileFinderHelpers {
		/// <summary>
		/// Counts tile types within the player's vicinity (used for checking biomes and stuff).
		/// </summary>
		/// <param name="midTileX"></param>
		/// <param name="midTileY"></param>
		/// <returns></returns>
		[Obsolete("needs work")]
		public static IDictionary<int, int> GetPlayerRangeTilesAt( int midTileX, int midTileY ) {	// TODO
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
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="radius"></param>
		/// <param name="retries"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if tile match found.</returns>
		public static bool FindNearbyRandomMatch( TileType pattern, int tileX, int tileY, int radius, int retries,
					out int foundX, out int foundY ) {
			int wtf = 0;

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

			do {
				do { foundX = Main.rand.Next( -radius, radius ) + tileX; }
				while( foundX <= 0 || foundX >= Main.mapMaxX );
				do { foundY = Main.rand.Next( -radius, radius ) + tileY; }
				while( foundY <= 0 || foundY >= Main.mapMaxY );

				//tile = Main.tile[toX, toY];
				if( wtf++ > retries ) {
					foundX = 0;
					foundY = 0;
					return false;
				}
			} while( !pattern.Check( foundX, foundY ) || Lighting.Brightness( foundX, foundX ) == 0 );

			return true;
		}


		/// <summary>
		/// Attempts to find an area within an area of a given matching tile pattern.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="within"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindAreaMatchWithin( TileType pattern, Rectangle within, int width, int height,
					out int foundX, out int foundY ) {
			int maxX = within.X + within.Width - width;
			int maxY = within.Y + within.Height - height;

			for( int i = within.X; i < maxX; i++ ) {
				for( int j = within.Y; j < maxY; j++ ) {
					if( pattern.CheckArea( i, j, width, height ) ) {
						foundX = i;
						foundY = j;
						return true;
					}
				}
			}

			foundX = foundY = 0;
			return false;
		}


		/// <summary>
		/// Attempts to find an area within an area of a given matching tile pattern. Search begins from the center.
		/// </summary>
		/// <param name="tileType"></param>
		/// <param name="within"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindNearbyAreaFromCenter( TileType tileType, Rectangle within, int width, int height,
					out int foundX, out int foundY ) {
			int midX = within.X + ( within.Width / 2 ) - ( width / 2 );
			int midY = within.Y + ( within.Height / 2 ) - ( height / 2 );
			int maxX = within.X + within.Width - width;
			int maxY = within.Y + within.Height - height;
			int currMinX = midX;
			int currMaxX = midX;
			int currMinY = midY;
			int currMaxY = midY;

			int i = midX;
			int j = midY;
			int turn = 0;

			while( !( currMinX == within.X && currMaxX == maxX && currMinY == within.Y && currMaxY == maxY ) ) {
				if( tileType.CheckArea( i, j, width, height ) ) {   // TODO Optimize currMin and currMax from data from CheckArea
					foundX = i;
					foundY = j;
					return true;
				}

				switch( turn ) {
				case 0:
					if( i < currMaxX ) {
						i++;
					} else {
						if( j < currMaxY ) { j++; }
						turn++;
					}
					break;
				case 1:
					if( j < currMaxY ) {
						j++;
					} else {
						if( i > currMinX ) { i--; }
						turn++;
					}
					break;
				case 2:
					if( i > currMinX ) {
						i--;
					} else {
						if( j > currMinY ) { j--; }
						turn++;
					}
					break;
				case 3:
					if( j > currMinY ) {
						j--;
					} else {
						if( i < currMaxX ) { i--; }
						turn = 0;

						if( currMinX > within.X ) { currMinX--; }
						if( currMinY > within.Y ) { currMinY--; }
						if( currMaxX < maxX ) { currMaxX++; }
						if( currMaxY < maxY ) { currMaxY++; }
					}
					break;
				}
			}

			foundX = foundY = 0;
			return false;
		}
	}
}
