using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using System;
using System.Collections.Generic;
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


		/// <summary>
		/// Gets a list of all contiguous tiles matching the given pattern.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxRadius"></param>
		/// <param name="maxTiles"></param>
		/// <returns></returns>
		public static IEnumerable<(ushort TileX, ushort TileY)> GetAllContiguousMatchingTiles(
					TilePattern pattern,
					int tileX,
					int tileY,
					int maxRadius = -1,
					int maxTiles = -1 ) {
			if( !pattern.Check(tileX, tileY) ) {
				return new (ushort, ushort)[ 0 ];
			}

			ISet<int> unchartedTileMap = new HashSet<int>{ tileX + (tileY<<16) };
			ISet<int> newUnchartedTileMap = new HashSet<int>();
			ISet<int> chartedTileMap = new HashSet<int>();

			int maxRadiusSqr = maxRadius * maxRadius;
			int distX, distY, distSqr;

			do {
				foreach( int tileAt in unchartedTileMap ) {
					int x = (tileAt << 16) >> 16;
					int y = tileAt >> 16;

					if( maxTiles > 0 && chartedTileMap.Count >= maxTiles ) {
						break;
					}

					if( maxRadius > 0 ) {
						distX = x - tileX;
						distY = y - tileY;
						distSqr = (distX * distX) + (distY * distY);

						if( distSqr >= maxRadiusSqr ) {
							continue;
						}
					}

					if( pattern.Check(x, y) ) {
						chartedTileMap.Add( tileAt );
						newUnchartedTileMap.Add( x + ((y-1)<<16) );
						newUnchartedTileMap.Add( (x-1) + (y<<16) );
						newUnchartedTileMap.Add( (x+1) + (y<<16) );
						newUnchartedTileMap.Add( x + ((y+1)<<16) );
					}
				}

				if( maxTiles > 0 && chartedTileMap.Count >= maxTiles ) {
					break;
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

			return chartedTileMap
				.SafeSelect(
					tileAt => (
						(ushort)((tileAt<<16)>>16),
						(ushort)(tileAt>>16)
					)
				);
		}
	}
}
