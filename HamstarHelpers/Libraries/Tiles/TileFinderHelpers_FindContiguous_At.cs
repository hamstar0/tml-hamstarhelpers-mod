using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Libraries.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderLibraries {
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
		public static ISet<(ushort TileX, ushort TileY)> GetAllContiguousMatchingTilesAt(
					TilePattern pattern,
					int tileX,
					int tileY,
					out ISet<(ushort TileX, ushort TileY)> excessTiles,
					int maxRadius = -1,
					int tileQuota = -1 ) {
			if( !pattern.Check(tileX, tileY, out _) ) {
				excessTiles = new HashSet<(ushort, ushort)>();
				return new HashSet<(ushort, ushort)>();
			}

			ISet<(ushort, ushort)> chartedTiles = new HashSet<(ushort, ushort)>();
			ISet<(ushort, ushort)> unchartedTiles = new HashSet<(ushort, ushort)> { ((ushort)tileX, (ushort)tileY) };
			excessTiles = new HashSet<(ushort, ushort)>();

			int maxRadiusSqr = maxRadius > 0
				? maxRadius * maxRadius
				: 0;

			do {
				TileFinderLibraries.IterateChartingContiguousMatchingTiles(
					pattern: pattern,
					srcTileX: tileX,
					srcTileY: tileY,
					maxRadiusSqr: maxRadiusSqr,
					tileQuota: tileQuota,
					excessTiles: ref excessTiles,
					chartedTiles: ref chartedTiles,
					unchartedTiles: ref unchartedTiles
				);
			} while( unchartedTiles.Count > 0 );
			
			return chartedTiles;
		}

		/// @private
		[Obsolete( "use GetAllContiguousMatchingTilesAt", true )]
		public static IList<(ushort TileX, ushort TileY)> GetAllContiguousMatchingTiles(
					TilePattern pattern,
					int tileX,
					int tileY,
					out IList<(ushort TileX, ushort TileY)> excessTiles,
					int maxRadius = -1,
					int tileQuota = -1 ) {
			ISet<(ushort TileX, ushort TileY)> excessTileSet;
			var matches = TileFinderLibraries.GetAllContiguousMatchingTilesAt(
				pattern, tileX, tileY, out excessTileSet, maxRadius, tileQuota
			);

			excessTiles = excessTileSet.ToList();
			return matches.ToList();
		}


		////////////////

		private static void IterateChartingContiguousMatchingTiles(
					TilePattern pattern,
					int srcTileX,
					int srcTileY,
					int maxRadiusSqr,
					int tileQuota,
					ref ISet<(ushort, ushort)> excessTiles,
					ref ISet<(ushort, ushort)> chartedTiles,
					ref ISet<(ushort, ushort)> unchartedTiles ) {
			ISet<(ushort, ushort)> nextToScanTiles = new HashSet<(ushort, ushort)>();

			foreach( (ushort x, ushort y) unchartedTile in unchartedTiles ) {
				TileFinderLibraries.StepChartingTileMatchAt(
					pattern: pattern,
					srcTileX: srcTileX,
					srcTileY: srcTileY,
					unchartedTile: unchartedTile,
					maxRadiusSqr: maxRadiusSqr,
					tileQuota: tileQuota,
					excessTiles: ref excessTiles,
					chartedTiles: ref chartedTiles,
					nextToScanTiles: ref nextToScanTiles
				);
			}

			unchartedTiles.Clear();

			foreach( (ushort, ushort) tileAt in nextToScanTiles ) {
				if( !chartedTiles.Contains(tileAt) && !excessTiles.Contains(tileAt) ) {
					unchartedTiles.Add( tileAt );
				}
			}
		}


		private static void StepChartingTileMatchAt(
					TilePattern pattern,
					int srcTileX,
					int srcTileY,
					(ushort x , ushort y) unchartedTile,
					int maxRadiusSqr,
					int tileQuota,
					ref ISet<(ushort, ushort)> excessTiles,
					ref ISet<(ushort, ushort)> chartedTiles,
					ref ISet<(ushort, ushort)> nextToScanTiles ) {
			int distX, distY, distSqr;

			int x = (int)unchartedTile.x;
			int y = (int)unchartedTile.y;

			bool isAtTileQuota = tileQuota > 0 && chartedTiles.Count >= tileQuota;
			bool isOutOfRange = false;

			if( maxRadiusSqr > 0 ) {
				distX = x - srcTileX;
				distY = y - srcTileY;
				distSqr = (distX * distX) + (distY * distY);

				if( distSqr >= maxRadiusSqr ) {
					isOutOfRange = true;
				}
			}

			if( pattern.Check(x, y) ) {
//LogHelpers.Log( "  x:"+x+", y:"+y+" = "+TileID.GetUniqueKey( Main.tile[x, y].type )+" ? "+isOutOfRange+", "+isAtTileQuota );
				if( !isOutOfRange && !isAtTileQuota ) {
					chartedTiles.Add( unchartedTile );

					if( y > 0 ) {
						nextToScanTiles.Add( ((ushort)x, (ushort)(y - 1)) );
					}
					if( x > 0 ) {
						nextToScanTiles.Add( ((ushort)(x - 1), (ushort)y ) );
					}
					if( x < Main.maxTilesX - 1 ) {
						nextToScanTiles.Add( ((ushort)( x + 1 ), (ushort)y) );
					}
					if( y < Main.maxTilesY - 1 ) {
						nextToScanTiles.Add( ((ushort)x, (ushort)( y + 1 )) );
					}
				} else {
					excessTiles.Add( unchartedTile );
				}
			}
		}
	}
}
