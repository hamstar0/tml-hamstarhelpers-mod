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
		/// <summary>
		/// Attempts to find an area of contiguous matching tiles within a larger area.
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="within"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="foundX">Returns found X tile coordinate.</param>
		/// <param name="foundY">Returns found Y tile coordinate.</param>
		/// <returns>`true` if matching area found.</returns>
		public static bool FindAreaMatchWithin( TilePattern pattern, Rectangle within, int width, int height,
					out int foundX, out int foundY ) {
			int maxX = within.X + within.Width - width;
			int maxY = within.Y + within.Height - height;

			for( int i = within.X; i < maxX; i++ ) {
				for( int j = within.Y; j < maxY; j++ ) {
					if( pattern.CheckArea( i, j, width, height ) ) {
						foundX = i;
						foundY = j;
						return true;
					}
				}
			}

			foundX = foundY = 0;
			return false;
		}


		/// <summary>
		/// Gets all tiles of a given pattern within a given area.
		/// </summary>
		/// <param name="worldRect"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static IList<(ushort TileY, ushort TileX)> GetTileMatchesInWorldRectangle( Rectangle worldRect, TilePattern pattern ) {
			return TileFinderHelpers.GetTileMatchesInWorldRectangle( worldRect, pattern, null );
		}

		/// <summary>
		/// Gets all tiles of a given pattern within a given area. Calls a function for each.
		/// </summary>
		/// <param name="worldRect"></param>
		/// <param name="pattern"></param>
		/// <param name="forEach">Performs an action for each tile. 3rd bool parameter indicates a match. Returned bool to indicate a match.</param>
		/// <returns></returns>
		public static IList<(ushort TileY, ushort TileX)> GetTileMatchesInWorldRectangle(
				Rectangle worldRect,
				TilePattern pattern,
				Func<int, int, bool, bool> forEach ) {
			int projWldRight = worldRect.X + worldRect.Width;
			int projWldBottom = worldRect.Y + worldRect.Height;

			var hits = new List<(ushort, ushort)>();

			for( int i = (worldRect.X >> 4); (i << 4) <= projWldRight; i++ ) {
				if( i < 0 || i > Main.maxTilesX - 1 ) { continue; }

				for( int j = (worldRect.Y >> 4); (j << 4) <= projWldBottom; j++ ) {
					if( j < 0 || j > Main.maxTilesY - 1 ) { continue; }

					Tile tile = Main.tile[i, j];

					//if( TileHelpers.IsAir( tile ) ) { continue; }

					bool isMatch = pattern.Check( i, j );
					isMatch = forEach?.Invoke( i, j, isMatch ) ?? isMatch;

					if( isMatch ) { continue; }

					hits.Add( ((ushort)i, (ushort)j) );
				}
			}

			return hits;
		}
	}
}
