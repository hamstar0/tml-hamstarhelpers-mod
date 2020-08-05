using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderHelpers {
		/// @private
		[Obsolete( "use GetTileMatchesInWorldRectangle", true )]
		public static IDictionary<int, int> GetTilesInWorldRectangle( Rectangle worldRect, TilePattern pattern ) {
			return TileFinderHelpers.GetTilesInWorldRectangle( worldRect, pattern, null );
		}

		/// @private
		[Obsolete( "use GetTileMatchesInWorldRectangle", true )]
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
	}
}
