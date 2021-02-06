using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;


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
		/// <param name="excessTiles">All matched tiles that exceed `maxRadius` or `maxTiles`.</param>
		/// <param name="maxRadius"></param>
		/// <param name="tileQuota"></param>
		/// <returns></returns>
		public static IList<(ushort TileX, ushort TileY)> GetAllContiguousMatchingTiles(
					TilePattern pattern,
					int tileX,
					int tileY,
					out IList<(ushort TileX, ushort TileY)> excessTiles,
					int maxRadius = -1,
					int tileQuota = -1 ) {
			int getCodeFromCoord( int x, int y )
				=> x + (y << 16);

			(ushort, ushort) getCoordFromCode( int code ) => (
				(ushort)((code << 16) >> 16),
				(ushort)(code >> 16)
			);

			//

			TileCollideType collide;
			if( !pattern.Check(tileX, tileY, out collide) ) {
				excessTiles = new (ushort, ushort)[ 0 ];
				return new (ushort, ushort)[ 0 ];
			}

			ISet<int> excessTileCodes = new HashSet<int>();
			ISet<int> unchartedTileCodes = new HashSet<int> { getCodeFromCoord(tileX, tileY) };
			ISet<int> newUnchartedTileCodes = new HashSet<int>();
			ISet<int> chartedTileCodes = new HashSet<int>();

			int maxRadiusSqr = maxRadius * maxRadius;

			do {
				foreach( int unchartedTileCode in unchartedTileCodes ) {
					TileFinderHelpers.GetAllContiguousMatchingTileCodesAt(
						pattern: pattern,
						srcTileX: tileX,
						srcTileY: tileY,
						unchartedTileCode: unchartedTileCode,
						maxRadiusSqr: maxRadiusSqr,
						tileQuota: tileQuota,
						excessTileCodes: ref excessTileCodes,
						chartedTileCodes: ref chartedTileCodes,
						unchartedTileCodes: ref newUnchartedTileCodes
					);
				}

				unchartedTileCodes.Clear();

				foreach( int tileAt in newUnchartedTileCodes ) {
					if( chartedTileCodes.Contains(tileAt) ) {
						continue;
					}
					unchartedTileCodes.Add( tileAt );
				}

				newUnchartedTileCodes.Clear();
			} while( unchartedTileCodes.Count > 0 );

			if( excessTileCodes.Count > 0 ) {
				excessTiles = excessTileCodes
					.SafeSelect( tileAt => getCoordFromCode(tileAt) )
					.ToList();
			} else {
				excessTiles = new (ushort, ushort)[0];
			}

			return chartedTileCodes
				.SafeSelect( tileAt => getCoordFromCode(tileAt) )
				.ToList();
		}


		private static void GetAllContiguousMatchingTileCodesAt(
					TilePattern pattern,
					int srcTileX,
					int srcTileY,
					int unchartedTileCode,
					int maxRadiusSqr,
					int tileQuota,
					ref ISet<int> excessTileCodes,
					ref ISet<int> chartedTileCodes,
					ref ISet<int> unchartedTileCodes ) {
			int getCodeFromCoord( int x, int y )
				=> x + ( y << 16 );

			(ushort, ushort) getCoordFromCode( int code ) => (
				(ushort)( (code << 16) >> 16 ),
				(ushort)( code >> 16 )
			);

			//

			int distX, distY, distSqr;

			bool isAtTileQuota = tileQuota > 0 && chartedTileCodes.Count >= tileQuota;

			(ushort x, ushort y) coord = getCoordFromCode( unchartedTileCode );
			int x = (int)coord.x;
			int y = (int)coord.y;

			bool isOutOfRange = false;

			if( maxRadiusSqr > 0 ) {
				distX = x - srcTileX;
				distY = y - srcTileY;
				distSqr = ( distX * distX ) + ( distY * distY );

				if( distSqr >= maxRadiusSqr ) {
					isOutOfRange = true;
				}
			}

			if( pattern.Check( x, y ) ) {
				if( !isOutOfRange && !isAtTileQuota ) {
					chartedTileCodes.Add( unchartedTileCode );
					unchartedTileCodes.Add( getCodeFromCoord( x, y - 1 ) );
					unchartedTileCodes.Add( getCodeFromCoord( x - 1, y ) );
					unchartedTileCodes.Add( getCodeFromCoord( x + 1, y ) );
					unchartedTileCodes.Add( getCodeFromCoord( x, y + 1 ) );
				} else {
					excessTileCodes.Add( unchartedTileCode );
				}
			}
		}
	}
}
