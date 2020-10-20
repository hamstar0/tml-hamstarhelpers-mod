using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using Microsoft.Xna.Framework;

namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
		/// <summary>
		/// Finds the top left tile of a given area by a given pattern. Assumes the area is square (checks left first).
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
			bool foundX = false, foundY = false;
			
			do {
				if( !pattern.Check( tileX - i, tileY ) ) {
					foundX = true;
					i--;
					break;
				}
			} while( i++ < maxDistance );

			do {
				if( !pattern.Check( tileX - i, tileY - j ) ) {
					foundY = true;
					j--;
					break;
				}
			} while( j++ < maxDistance );

			coords = (TileX: tileX - i, TileY: tileY - j);
			return foundX && foundY;
		}


		////////////////

		/// <summary>
		/// Traces downwards from a given tile coordinate to the nearest floor, and then measures the contiguous width.
		/// </summary>
		/// <param name="nonFloorPattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxFallRange">Max distance to trace downwards to a floor before giving up.</param>
		/// <param name="floorX">Leftmost tile of the contiguous floor.</param>
		/// <param name="floorY">Last matching tile before hitting the floor.</param>
		/// <returns></returns>
		public static int GetFloorWidth(
					TilePattern nonFloorPattern,
					int tileX,
					int tileY,
					int maxFallRange,
					out int floorX,
					out int floorY ) {
			floorY = tileY;

			while( nonFloorPattern.Check(tileX, floorY) ) {
				floorY++;

				if( (floorY - tileY) >= maxFallRange ) {
					floorX = tileX;
					return 0;//TileFinderHelpers.GetHorizontalWidthAt( nonFloorPattern, tileX, out floorX, --floorY );
				}
			}
			floorY--;

			return TileFinderHelpers.GetHorizontalWidthAt( nonFloorPattern, tileX, out floorX, floorY );
		}

		/// <summary>
		/// Traces upwards from a given tile coordinate to the nearest ceiling, and then measures the contiguous width.
		/// </summary>
		/// <param name="nonCeilingPattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxRiseRange">Max distance to trace upwards to a ceiling before giving up.</param>
		/// <param name="ceilX">Leftmost tile of the contiguous ceiling.</param>
		/// <param name="ceilY">Last matching tile before hitting the ceiling.</param>
		/// <returns></returns>
		public static int GetCeilingWidth( TilePattern nonCeilingPattern, int tileX, int tileY, int maxRiseRange, out int ceilX, out int ceilY ) {
			ceilY = tileY;

			while( nonCeilingPattern.Check( tileX, ceilY ) ) {
				ceilY--;

				if( ( tileY - ceilY ) >= maxRiseRange ) {
					ceilX = tileX;
					return 0;//TileFinderHelpers.GetHorizontalWidthAt( nonCeilingPattern, tileX, out ceilX, ++ceilY );
				}
			}
			ceilY++;

			return TileFinderHelpers.GetHorizontalWidthAt( nonCeilingPattern, tileX, out ceilX, ceilY );
		}


		////

		private static int GetHorizontalWidthAt( TilePattern pattern, int tileX, out int floorX, int floorY ) {
			int rightWidth = 1;
			while( pattern.Check(tileX + rightWidth, floorY) && !pattern.Check(tileX + rightWidth, floorY + 1) ) {
				rightWidth++;
			}

			int leftWidth = 0;
			while( pattern.Check(tileX - leftWidth, floorY) && !pattern.Check(tileX - leftWidth, floorY + 1) ) {
				leftWidth++;
			}

			floorX = tileX - leftWidth;
			return (rightWidth - 1) + leftWidth;
		}


		////////////////

		/// <summary>
		/// Gets a list of all contiguous tiles matching the given pattern.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="unclosedTiles">All matched tiles that exceed `maxRadius` or `maxTile`, just beyond the outer edges of the result's matches.</param>
		/// <param name="maxRadius"></param>
		/// <param name="maxTiles"></param>
		/// <returns></returns>
		public static IList<(ushort TileX, ushort TileY)> GetAllContiguousMatchingTiles(
					TilePattern pattern,
					int tileX,
					int tileY,
					out IList<(ushort TileX, ushort TileY)> unclosedTiles,
					int maxRadius = -1,
					int maxTiles = -1 ) {
			int getCodeFromCoord( int x, int y ) => x + (y << 16);

			(ushort, ushort) getCoordFromCode( int code ) => (
				(ushort)((code << 16) >> 16),
				(ushort)(code >> 16)
			);

			////

			TileCollideType collide;
			if( !pattern.Check(tileX, tileY, out collide) ) {
				unclosedTiles = new (ushort, ushort)[0];
				return new (ushort, ushort)[ 0 ];
			}

			ISet<int> edgeTileMap = new HashSet<int>();
			ISet<int> unchartedTileMap = new HashSet<int>{ getCodeFromCoord(tileX, tileY) };
			ISet<int> newUnchartedTileMap = new HashSet<int>();
			ISet<int> chartedTileMap = new HashSet<int>();

			int maxRadiusSqr = maxRadius * maxRadius;
			int distX, distY, distSqr;
			(ushort x, ushort y) coord;

			do {
				bool isMaxTiles = false;

				foreach( int tileAt in unchartedTileMap ) {
					bool isMaxRadius = false;

					coord = getCoordFromCode( tileAt );
					int x = (int)coord.x;
					int y = (int)coord.y;

					if( maxTiles > 0 && chartedTileMap.Count >= maxTiles ) {
						isMaxTiles = true;
					}

					if( maxRadius > 0 ) {
						distX = x - tileX;
						distY = y - tileY;
						distSqr = (distX * distX) + (distY * distY);

						if( distSqr >= maxRadiusSqr ) {
							isMaxRadius = true;
						}
					}

					if( pattern.Check(x, y) ) {
						if( !isMaxRadius && !isMaxTiles ) {
							chartedTileMap.Add( tileAt );
							newUnchartedTileMap.Add( getCodeFromCoord(x, y - 1) );
							newUnchartedTileMap.Add( getCodeFromCoord(x - 1, y) );
							newUnchartedTileMap.Add( getCodeFromCoord(x + 1, y) );
							newUnchartedTileMap.Add( getCodeFromCoord(x, y + 1) );
						} else {
							edgeTileMap.Add( tileAt );
						}
					}
				}

				unchartedTileMap.Clear();

				foreach( int tileAt in newUnchartedTileMap ) {
					if( chartedTileMap.Contains(tileAt) ) {
						continue;
					}
					unchartedTileMap.Add( tileAt );
				}

				newUnchartedTileMap.Clear();
			} while( unchartedTileMap.Count > 0 );

			if( edgeTileMap.Count > 0 ) {
				unclosedTiles = edgeTileMap
					.SafeSelect( tileAt => getCoordFromCode(tileAt) )
					.ToList();
			} else {
				unclosedTiles = new (ushort, ushort)[0];
			}

			return chartedTileMap
				.SafeSelect( tileAt => getCoordFromCode(tileAt) )
				.ToList();
		}


		/// <summary>
		/// Scans the entire world to find the largest encompassing box of the given tile pattern. Leaves a 1 tile
		/// padding around map edges.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="minimumMatchingNeighbors"></param>
		/// <returns></returns>
		public static Rectangle? FindBoxForAllOf( TilePattern pattern, int minimumMatchingNeighbors = 1 ) {
			int countNeighbors( int x, int y ) {
				if( minimumMatchingNeighbors == 0 ) {
					return 0;
				}

				int count = 0;

				if( pattern.Check( x - 1, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y - 1 ) ) {
					count++;
				}
				if( pattern.Check( x - 1, y ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y ) ) {
					count++;
				}
				if( pattern.Check( x - 1, y + 1 ) ) {
					count++;
				}
				if( pattern.Check( x, y + 1 ) ) {
					count++;
				}
				if( pattern.Check( x + 1, y + 1 ) ) {
					count++;
				}

				return count;
			}

			//

			var area = new Rectangle( -1, -1, 0, 0 );
			int maxX = Main.maxTilesX - 1;
			int maxY = Main.maxTilesY - 1;

			for( int x=1; x<maxX; x++ ) {
				for( int y=1; y<maxY; y++ ) {
					if( !pattern.Check(x, y) ) {
						continue;
					}

					int neighbors = countNeighbors( x, y );
					if( neighbors < minimumMatchingNeighbors ) {
						continue;
					}

					if( area.X == -1 ) {
						area.X = x;
						area.Y = y;
					} else {
						if( (x - area.X) > area.Width ) {
							area.Width = (x - area.X);
						}
						if( (y - area.Y) > area.Height ) {
							area.Height = (y - area.Y);
						}
					}
				}
			}

			return area.X == -1
				? null
				: new Rectangle?(area);
		}
	}
}
