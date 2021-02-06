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
		private static int GetCodeFromCoord( int x, int y )
			=> x + ( y << 16 );

		private static (ushort, ushort) GetCoordFromCode( int code ) => (
			(ushort)( ( code << 16 ) >> 16 ),
			(ushort)( code >> 16 )
		);


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
			TileCollideType collide;
			if( !pattern.Check(tileX, tileY, out collide) ) {
				excessTiles = new (ushort, ushort)[ 0 ];
				return new (ushort, ushort)[ 0 ];
			}

			ISet<int> excessTileCodes = new HashSet<int>();
			ISet<int> unchartedTileCodes = new HashSet<int> { TileFinderHelpers.GetCodeFromCoord( tileX, tileY) };
			ISet<int> newUnchartedTileCodes = new HashSet<int>();
			ISet<int> chartedTileCodes = new HashSet<int>();

			int maxRadiusSqr = maxRadius * maxRadius;

			do {
				TileFinderHelpers.IterateChartingContiguousMatchingTilesByCode(
					pattern: pattern,
					srcTileX: tileX,
					srcTileY: tileY,
					maxRadiusSqr: maxRadiusSqr,
					tileQuota: tileQuota,
					excessTileCodes: ref excessTileCodes,
					chartedTileCodes: ref chartedTileCodes,
					unchartedTileCodes: ref unchartedTileCodes
				);
			} while( unchartedTileCodes.Count > 0 );
			
			if( excessTileCodes.Count > 0 ) {
				excessTiles = excessTileCodes
					.SafeSelect( tileAt => TileFinderHelpers.GetCoordFromCode( tileAt) )
					.ToList();
			} else {
				excessTiles = new (ushort, ushort)[0];
			}

			return chartedTileCodes
				.SafeSelect( tileAt => TileFinderHelpers.GetCoordFromCode(tileAt) )
				.ToList();
		}


		////////////////

		private static void IterateChartingContiguousMatchingTilesByCode(
					TilePattern pattern,
					int srcTileX,
					int srcTileY,
					int maxRadiusSqr,
					int tileQuota,
					ref ISet<int> excessTileCodes,
					ref ISet<int> chartedTileCodes,
					ref ISet<int> unchartedTileCodes ) {
			foreach( int unchartedTileCode in unchartedTileCodes ) {
				TileFinderHelpers.GetAllContiguousMatchingTileCodesAt(
					pattern: pattern,
					srcTileX: srcTileX,
					srcTileY: srcTileY,
					unchartedTileCode: unchartedTileCode,
					maxRadiusSqr: maxRadiusSqr,
					tileQuota: tileQuota,
					excessTileCodes: ref excessTileCodes,
					chartedTileCodes: ref chartedTileCodes,
					unchartedTileCodes: ref unchartedTileCodes
				);
			}

			unchartedTileCodes.Clear();

			foreach( int tileAt in unchartedTileCodes ) {
				if( chartedTileCodes.Contains( tileAt ) ) {
					continue;
				}
				unchartedTileCodes.Add( tileAt );
			}

			unchartedTileCodes.Clear();
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
			int distX, distY, distSqr;

			bool isAtTileQuota = tileQuota > 0 && chartedTileCodes.Count >= tileQuota;

			(ushort x, ushort y) coord = TileFinderHelpers.GetCoordFromCode( unchartedTileCode );
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
					unchartedTileCodes.Add( TileFinderHelpers.GetCodeFromCoord( x, y - 1 ) );
					unchartedTileCodes.Add( TileFinderHelpers.GetCodeFromCoord( x - 1, y ) );
					unchartedTileCodes.Add( TileFinderHelpers.GetCodeFromCoord( x + 1, y ) );
					unchartedTileCodes.Add( TileFinderHelpers.GetCodeFromCoord( x, y + 1 ) );
				} else {
					excessTileCodes.Add( unchartedTileCode );
				}
			}
		}
	}
}
