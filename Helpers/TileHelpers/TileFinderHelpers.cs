using System;
using Terraria;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileFinderHelpers {
		public static bool HasNearbySolid( int tileX, int tileY, int proximityInTiles ) {
			int minX = Math.Max( tileX - proximityInTiles, 0 );
			int maxX = Math.Min( tileX + proximityInTiles, Main.maxTilesX - 1 );
			int minY = Math.Max( tileY - proximityInTiles, 0 );
			int maxY = Math.Min( tileY + proximityInTiles, Main.maxTilesY - 1 );

			for( int i = minX; i <= maxX; i++ ) {
				for( int j = minY; j <= maxY; j++ ) {
					if( TileHelpers.IsSolid( Main.tile[i, j] ) ) {
						return true;
					}
				}
			}

			return false;
		}


		public static bool FindNearbyRandomAirTile( int tileX, int tileY, int radius, out int toX, out int toY ) {
			Tile tile = null;
			int wtf = 0;
			bool isBlocked = false;
			toX = 0;
			toY = 0;

			if( tileX + radius <= 0 || tileX - radius >= Main.mapMaxX ) { return false; }
			if( tileY + radius <= 0 || tileY - radius >= Main.mapMaxY ) { return false; }

			do {
				do { toX = Main.rand.Next( -radius, radius ) + tileX; }
				while( toX <= 0 || toX >= Main.mapMaxX );
				do { toY = Main.rand.Next( -radius, radius ) + tileY; }
				while( toY <= 0 || toY >= Main.mapMaxY );

				//tile = Main.tile[toX, toY];
				tile = Framing.GetTileSafely( toX, toY );
				if( wtf++ > 100 ) {
					return false;
				}

				isBlocked = TileHelpers.IsSolid( tile, true, true ) ||
					TileWallHelpers.IsDungeon( tile ) ||
					TileHelpers.IsWire( tile ) ||
					tile.lava();
			} while( isBlocked && ((tile != null && tile.type != 0) || Lighting.Brightness( toX, toX ) == 0) );

			return true;
		}
	}
}
