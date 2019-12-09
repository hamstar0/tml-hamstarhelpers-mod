using HamstarHelpers.Classes.Tiles.TilePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles.Draw {
	/// <summary>
	/// Assorted static "helper" functions pertaining to fill-'drawing' tiles into the world.
	/// </summary>
	public class TileDrawFillHelpers {
		/// <summary>
		/// Fills an area (of the given pattern) with the given tile type.
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="fillDiagonal"></param>
		/// <param name="prePlace">Return `null` to skip tile placing.</param>
		/// <returns></returns>
		public static ISet<(int TileX, int TileY)> Fill(
					TilePattern filter,
					int tileX,
					int tileY,
					bool fillDiagonal,
					Func<int, int, TileDrawDefinition> prePlace ) {
			var filled = new HashSet<(int, int)>();
			var unfilled = new HashSet<(int, int)> { (tileX, tileY) };

			do {
				var unfilledCopy = unfilled.ToArray();
				unfilled.Clear();

				foreach( (int x, int y) in unfilledCopy ) {
					if( filter.Check( x, y ) ) {
						if( prePlace(x, y)?.Place(x, y) ?? false ) {
							filled.Add( (x, y) );
						}
					}

					if( !filled.Contains( (x, y - 1) ) ) {
						unfilled.Add( (x, y - 1) );
					}
					if( !filled.Contains( (x - 1, y) ) ) {
						unfilled.Add( (x - 1, y) );
					}
					if( !filled.Contains( (x + 1, y) ) ) {
						unfilled.Add( (x + 1, y) );
					}
					if( !filled.Contains( (x, y + 1) ) ) {
						unfilled.Add( (x, y + 1) );
					}

					if( fillDiagonal ) {
						if( !filled.Contains( (x - 1, y - 1) ) ) {
							unfilled.Add( (x - 1, y - 1) );
						}
						if( !filled.Contains( (x + 1, y - 1) ) ) {
							unfilled.Add( (x + 1, y - 1) );
						}
						if( !filled.Contains( (x - 1, y + 1) ) ) {
							unfilled.Add( (x - 1, y + 1) );
						}
						if( !filled.Contains( (x + 1, y + 1) ) ) {
							unfilled.Add( (x + 1, y + 1) );
						}
					}
				}
			} while( unfilled.Count > 0 );

			return filled;
		}
	}
}
