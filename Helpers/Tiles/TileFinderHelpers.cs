﻿using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


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


		/// <summary>
		/// Finds the top left tile of a given pattern.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxDistance"></param>
		/// <param name="coords"></param>
		/// <returns>Returns tile coordinates, or else `null` if not found within the given max amounts.</returns>
		public static bool FindTopLeftOfSquare(
				TilePattern pattern,
				int tileX,
				int tileY,
				int maxDistance,
				out (int TileX, int TileY) coords ) {
			if( !pattern.Check(tileX, tileY) ) {
				coords = (tileX, tileY);
				return false;
			}

			int i = 1, j = 1;
			int maxX = 0, maxY = 0;
			bool foundX = false, foundY = false;

			do {
				if( !pattern.Check( tileX - i, tileY ) ) {
					foundX = true;
					maxX = i - 1;
					break;
				}
			} while( i++ < maxDistance );

			do {
				if( !pattern.Check( tileX - maxX, tileY - j ) ) {
					foundY = true;
					maxY = j - 1;
					break;
				}
			} while( j++ < maxDistance );

			coords = ( TileX: tileX - maxX, TileY: tileY - maxY);
			return foundX && foundY;
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
		public static bool FindNearbyRandomMatch( TilePattern pattern, int tileX, int tileY,
					int radius,
					int retries,
					bool skipDarkness,
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
			} while( !pattern.Check(foundX, foundY) || (!skipDarkness && Lighting.Brightness(foundX, foundX) == 0) );

			return true;
		}


		/// <summary>
		/// Attempts to find an area of contiguous matching tiles within a larger area.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="within"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindAreaMatchWithin( TilePattern pattern, Rectangle within, int width, int height,
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
		/// Attempts to find an area of a given matching tile pattern within a larger area. Search begins from the
		/// center.
		/// </summary>
		/// <param name="tileType"></param>
		/// <param name="within"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindNearbyAreaFromCenter( TilePattern tileType, Rectangle within, int width, int height,
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


		/// <summary>
		/// Gets all tiles of a given pattern within a given area.
		/// </summary>
		/// <param name="worldRect"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static IDictionary<int, int> GetTilesInWorldRectangle( Rectangle worldRect, TilePattern pattern ) {
			return TileFinderHelpers.GetTilesInWorldRectangle( worldRect, pattern, null );
		}

		/// <summary>
		/// Gets all tiles of a given pattern within a given area. Calls a function for each.
		/// </summary>
		/// <param name="worldRect"></param>
		/// <param name="pattern"></param>
		/// <param name="forEach">Performs an action for each tile. 3rd bool parameter indicates a match. Returned bool to indicate a match.</param>
		/// <returns></returns>
		public static IDictionary<int, int> GetTilesInWorldRectangle(
				Rectangle worldRect,
				TilePattern pattern,
				Func<int, int, bool, bool> forEach ) {
			int projWldRight = worldRect.X + worldRect.Width;
			int projWldBottom = worldRect.Y + worldRect.Height;

			IDictionary<int, int> hits = new Dictionary<int, int>();

			for( int i = (worldRect.X >> 4); (i << 4) <= projWldRight; i++ ) {
				if( i < 0 || i > Main.maxTilesX - 1 ) { continue; }

				for( int j = (worldRect.Y >> 4); (j << 4) <= projWldBottom; j++ ) {
					if( j < 0 || j > Main.maxTilesY - 1 ) { continue; }

					Tile tile = Main.tile[i, j];

					//if( TileHelpers.IsAir( tile ) ) { continue; }

					bool isMatch = pattern.Check( i, j );
					isMatch = forEach?.Invoke( i, j, isMatch ) ?? isMatch;

					if( isMatch ) { continue; }

					hits[i] = j;
				}
			}

			return hits;
		}


		/// <summary>
		/// Gets the nearest matching tile to a given point (as a world coordinate).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <param name="pattern"></param>
		/// <param name="maxWorldRadius"></param>
		/// <returns>Tile position of match. `null` if none.</returns>
		public static Point? GetNearestTile( Vector2 worldPos, TilePattern pattern, int maxWorldRadius = Int32.MaxValue ) {
			int midWldX = (int)Math.Round( worldPos.X );
			int midWldY = (int)Math.Round( worldPos.Y );
			int tileX = midWldX >> 4;
			int tileY = midWldY >> 4;

			if( pattern.Check( tileX, tileY ) ) {
				return new Point( tileX, tileY );
			}

			int max = Math.Max( Main.maxTilesX - 1, Main.maxTilesY - 1 ) * 16;
			max = Math.Min( maxWorldRadius, max );

			for( int radius = 16; radius < max; radius += 16 ) {
				double radMin = radius - 8;
				double radMax = radius + 8;
				radMin *= radMin;
				radMax *= radMax;

				for( double inX = -radius; inX <= radius; inX += 16 ) {
					for( double inY = -radius; inY <= radius; inY += 16 ) {
						double distSqr = ( inX * inX ) + ( inY * inY );
						if( distSqr < radMin || distSqr > radMax ) {
							continue;
						}

						tileX = ( midWldX + (int)inX ) >> 4;
						if( tileX < 0 || tileX >= ( Main.maxTilesX - 1 ) ) {
							continue;
						}

						tileY = ( midWldY + (int)inY ) >> 4;
						if( tileY < 0 || tileY >= ( Main.maxTilesY - 1 ) ) {
							continue;
						}

						if( pattern.Check( tileX, tileY ) ) {
							return new Point( tileX, tileY );
						}
					}
				}
			}

			return null;
		}


		/// <summary>
		/// Gets the nearest matching tile to a given point.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="pattern"></param>
		/// <param name="maxTileRadius"></param>
		/// <returns>Tile position of match. `null` if none.</returns>
		public static (int TileX, int TileY)? GetNearestTile( int tileX, int tileY, TilePattern pattern, int maxTileRadius = Int32.MaxValue ) {
			if( pattern.Check(tileX, tileY) ) {
				return (tileX, tileY);
			}

			int midTileX = tileX;
			int midTileY = tileY;
			int max = Math.Max( Main.maxTilesX - 1, Main.maxTilesY - 1 );
			max = Math.Min( maxTileRadius, max );

			for( double radius = 1; radius < max; radius += 1 ) {
				double radMinSqr = radius - 0.5d;
				double radMaxSqr = radius + 0.5d;
				radMinSqr *= radMinSqr;
				radMaxSqr *= radMaxSqr;

				for( double inTileX = -radius; inTileX <= radius; inTileX += 1 ) {
					for( double inTileY = -radius; inTileY <= radius; inTileY += 1 ) {
						double distSqr = (inTileX * inTileX) + (inTileY * inTileY);
						if( distSqr < radMinSqr || distSqr > radMaxSqr ) {
							continue;
						}

						tileX = midTileX + (int)inTileX;
						if( tileX < 0 || tileX >= (Main.maxTilesX - 1) ) {
							continue;
						}

						tileY = (midTileY + (int)inTileY) >> 4;
						if( tileY < 0 || tileY >= (Main.maxTilesY - 1) ) {
							continue;
						}

						if( pattern.Check(tileX, tileY) ) {
							return (tileX, tileY);
						}
					}
				}
			}

			return null;
		}
	}
}
