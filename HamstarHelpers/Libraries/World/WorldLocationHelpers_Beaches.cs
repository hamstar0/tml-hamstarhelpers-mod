using System;
using Terraria;


namespace HamstarHelpers.Libraries.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to locating things in the world.
	/// </summary>
	public partial class WorldLocationLibraries {
		private static bool CheckColumnForBeach( int x, out int atX, out int atY ) {
			int y;

			for( y = WorldLibraries.SkyLayerBottomTileY; y < WorldLibraries.SurfaceLayerBottomTileY; y++ ) {
				Tile tile = Framing.GetTileSafely( x, y );
				if( tile == null || !tile.active() ) {
					continue;
				}
				if( Main.tile[x, y - 1].liquid != 0 ) {
					break;
				}

				atX = x;
				atY = y;
				return true;
			}

			atX = x;
			atY = y;
			return false;
		}

		////

		/// <summary>
		/// Gets the first (sand) tile of the eastern beach.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool GetEastBeach( out int tileX, out int tileY ) {
			int reach = 40;//340;

			for( int x = reach; x < Main.dungeonX; x++ ) {
				if( WorldLocationLibraries.CheckColumnForBeach( x, out tileX, out tileY ) ) {
					return true;
				}
			}

			tileX = tileY = 0;
			return false;
		}

		/// <summary>
		/// Gets the first (sand) tile of the western beach.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool GetWestBeach( out int tileX, out int tileY ) {
			int reach = 40;//340;
			int max = (Main.maxTilesX - reach) - Main.dungeonX;

			for( int x = Main.maxTilesX - reach; x > Main.dungeonX; x-- ) {
				if( WorldLocationLibraries.CheckColumnForBeach( x, out tileX, out tileY ) ) {
					return true;
				}
			}

			tileX = tileY = 0;
			return false;
		}
	}
}
