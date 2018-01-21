using System;
using Terraria;


namespace HamstarHelpers.TileHelpers {
	public static class TileFinderHelpers {
		public static bool HasNearbySolid( int tile_x, int tile_y, int proximity_in_tiles ) {
			int min_x = Math.Max( tile_x - proximity_in_tiles, 0 );
			int max_x = Math.Min( tile_x + proximity_in_tiles, Main.maxTilesX - 1 );
			int min_y = Math.Max( tile_y - proximity_in_tiles, 0 );
			int max_y = Math.Min( tile_y + proximity_in_tiles, Main.maxTilesY - 1 );

			for( int i = min_x; i <= max_x; i++ ) {
				for( int j = min_y; j <= max_y; j++ ) {
					if( TileHelpers.IsSolid( Main.tile[i, j] ) ) {
						return true;
					}
				}
			}

			return false;
		}


		public static bool FindNearbyRandomAirTile( int tile_x, int tile_y, int radius, out int to_x, out int to_y ) {
			Tile tile = null;
			int wtf = 0;
			bool is_blocked = false;
			to_x = 0;
			to_y = 0;

			if( tile_x + radius <= 0 || tile_x - radius >= Main.mapMaxX ) { return false; }
			if( tile_y + radius <= 0 || tile_y - radius >= Main.mapMaxY ) { return false; }

			do {
				do { to_x = Main.rand.Next( -radius, radius ) + tile_x; }
				while( to_x <= 0 || to_x >= Main.mapMaxX );
				do { to_y = Main.rand.Next( -radius, radius ) + tile_y; }
				while( to_y <= 0 || to_y >= Main.mapMaxY );

				//tile = Main.tile[to_x, to_y];
				tile = Framing.GetTileSafely( to_x, to_y );
				if( wtf++ > 100 ) {
					return false;
				}

				is_blocked = TileHelpers.IsSolid( tile, true, true ) ||
					TileWallHelpers.IsDungeon( tile ) ||
					TileHelpers.IsWire( tile ) ||
					tile.lava();
			} while( is_blocked && ((tile != null && tile.type != 0) || Lighting.Brightness( to_x, to_x ) == 0) );

			return true;
		}
	}
}
