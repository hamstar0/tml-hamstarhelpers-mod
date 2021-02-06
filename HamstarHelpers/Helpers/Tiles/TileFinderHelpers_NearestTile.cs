using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
		/// <summary>
		/// Attempts to find an area of a given matching tile pattern within a larger area. Search begins from the
		/// center.
		/// </summary>
		/// <param name="tileType"></param>
		/// <param name="within"></param>
		/// <param name="patternWidth"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindNearbyAreaFromCenter(
					TilePattern tileType,
					Rectangle within,
					int patternWidth,
					int height,
					out int foundX,
					out int foundY ) {
			int midX = within.X + ( within.Width / 2 ) - ( patternWidth / 2 );
			int midY = within.Y + ( within.Height / 2 ) - ( height / 2 );
			int maxX = within.X + within.Width - patternWidth;
			int maxY = within.Y + within.Height - height;
			int currMinX = midX;
			int currMaxX = midX;
			int currMinY = midY;
			int currMaxY = midY;

			int i = midX;
			int j = midY;
			int turn = 0;

			while( !( currMinX == within.X && currMaxX == maxX && currMinY == within.Y && currMaxY == maxY ) ) {
				if( tileType.CheckArea( i, j, patternWidth, height ) ) {   // TODO Optimize currMin and currMax from data from CheckArea
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


		////

		/// <summary>
		/// Gets the nearest matching tile to a given point.
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
				double radMinSqr = radius - 8;
				double radMaxSqr = radius + 8;
				radMinSqr *= radMinSqr;
				radMaxSqr *= radMaxSqr;

				for( double inX = -radius; inX <= radius; inX += 16 ) {
					for( double inY = -radius; inY <= radius; inY += 16 ) {
						double distSqr = ( inX * inX ) + ( inY * inY );
						if( distSqr < radMinSqr || distSqr > radMaxSqr ) {
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
		/// Gets the nearest matching tile to a given tile.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="pattern"></param>
		/// <param name="maxTileRadius"></param>
		/// <returns>Tile position of match. `null` if none.</returns>
		public static (int TileX, int TileY)? GetNearestTile(
				int tileX,
				int tileY,
				TilePattern pattern,
				int maxTileRadius = Int32.MaxValue ) {
			if( pattern.Check(tileX, tileY) ) {
				return ( tileX, tileY );
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
