using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
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
		/// Traces downwards from a given tile coordinate to the nearest floor, and then measures the contiguous width.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxFallRange">Max distance to trace downwards to a floor before giving up.</param>
		/// <param name="floorX">Leftmost tile of the contiguous floor.</param>
		/// <param name="floorY">Last matching tile before hitting the floor.</param>
		/// <returns></returns>
		public static int GetFloorWidth( TilePattern pattern, int tileX, int tileY, int maxFallRange, out int floorX, out int floorY ) {
			floorY = tileY;

			while( pattern.Check(tileX, floorY) ) {
				floorY++;

				if( (floorY - tileY) >= maxFallRange ) {
					floorX = tileX;
					return 0;
				}
			}
			floorY--;

			int rightWidth = 1;
			while( pattern.Check(tileX+rightWidth, floorY) && !pattern.Check(tileX+rightWidth, floorY+1) ) {
				rightWidth++;
			}

			int leftWidth = 0;
			while( pattern.Check(tileX-leftWidth, floorY) && !pattern.Check(tileX-leftWidth, floorY+1) ) {
				leftWidth++;
			}

			floorX = tileX - leftWidth;
			return rightWidth + leftWidth;
		}

		/// <summary>
		/// Traces upwards from a given tile coordinate to the nearest ceiling, and then measures the contiguous width.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxRiseRange">Max distance to trace upwards to a ceiling before giving up.</param>
		/// <param name="ceilX">Leftmost tile of the contiguous ceiling.</param>
		/// <param name="ceilY">Last matching tile before hitting the ceiling.</param>
		/// <returns></returns>
		public static int GetCeilingWidth( TilePattern pattern, int tileX, int tileY, int maxRiseRange, out int ceilX, out int ceilY ) {
			ceilY = tileY;

			while( pattern.Check(tileX, ceilY) ) {
				ceilY--;

				if( (tileY - ceilY) >= maxRiseRange ) {
					ceilX = tileX;
					return 0;
				}
			}
			ceilY++;

			int rightWidth = 1;
			while( pattern.Check(tileX+rightWidth, ceilY) && !pattern.Check(tileX+rightWidth, ceilY-1) ) {
				rightWidth++;
			}

			int leftWidth = 0;
			while( pattern.Check(tileX-leftWidth, ceilY) && !pattern.Check(tileX-leftWidth, ceilY-1) ) {
				leftWidth++;
			}

			ceilX = tileX - leftWidth;
			return rightWidth + leftWidth;
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
	}
}
