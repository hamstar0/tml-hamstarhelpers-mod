using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using System;
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
	}
}
