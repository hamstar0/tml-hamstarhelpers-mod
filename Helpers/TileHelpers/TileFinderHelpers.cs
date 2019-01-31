using System;
using System.Collections.Generic;
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


		////

		public static IDictionary<int, int> GetPlayerRangeTilesAt( int tileX, int tileY ) {
			var screenRangeTiles = new Dictionary<int, int>();

			float minScreenWidHalfX = 50f * 0.5f;
			float minScreenHeiHalfY = 37.5f * 0.5f;
			int offscreenTiles = 23;
			/*if( Lighting.lightMode < 2 ) {
				offscreenTiles = 40;
			} else {
				offscreenTiles = 23;
			}*/

			int firstLightX = (int)(((float)tileX - minScreenWidHalfX) / 16f) - 1;
			int firstLightY = (int)(((float)tileY - minScreenHeiHalfY) / 16f) - 1;
			int lastLightX = (int)(((float)tileX + minScreenWidHalfX) / 16f) + 2;
			int lastLightY = (int)(((float)tileY + minScreenHeiHalfY) / 16f) + 2;
			firstLightX = Utils.Clamp<int>( firstLightX, 0, Main.maxTilesX - 1 );
			firstLightY = Utils.Clamp<int>( firstLightY, 0, Main.maxTilesX - 1 );
			lastLightX = Utils.Clamp<int>( lastLightX, 0, Main.maxTilesY - 1 );
			lastLightY = Utils.Clamp<int>( lastLightY, 0, Main.maxTilesY - 1 );

			int width = offscreenTiles + (lastLightX - firstLightX);
			int height = offscreenTiles + (lastLightY - firstLightY);

			int biomeOffsetX = ( width - Main.zoneX ) / 2;
			int biomeOffsetY = ( height - Main.zoneY ) / 2;

			Tile tile;
			for( int x = firstLightX + biomeOffsetX; x < lastLightX - biomeOffsetX; x++ ) {
				for( int y = firstLightY + biomeOffsetY; y < lastLightY - biomeOffsetY; y++ ) {
					tile = Main.tile[x, y];

					if( !screenRangeTiles.ContainsKey(tile.type) ) {
						screenRangeTiles[ tile.type ] = 1;
					} else {
						screenRangeTiles[ tile.type ] += 1;
					}
				}
			}

			return screenRangeTiles;
		}
	}
}
