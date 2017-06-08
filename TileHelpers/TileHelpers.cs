using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.TileHelpers {
	public static class TileHelpers {
		public static float GaugeBrightnessWithin( int x, int y, int width, int height ) {
			int i = 0, j = 0;
			float avg = 0f;

			for( i=0; i<width; i++ ) {
				for( j=0; j<height; j++ ) {
					avg += Lighting.Brightness( x + i, y + j );
				}
			}

			return avg / (i * j);
		}


		public static bool IsAir( Tile tile ) {
			return tile == null || !tile.active() /*|| tile.type == 0*/;
		}

		public static bool IsSolid( Tile tile, bool is_platform_solid = false, bool is_actuated_solid = false ) {
			if( tile == null || !tile.active() ) { return false; }

			if( Main.tileSolid[(int)tile.type] ) {  // Solid
				if( !Main.tileSolidTop[(int)tile.type] || is_platform_solid ) {  // Non-platform
					if( !tile.inActive() || is_actuated_solid ) {  // Actuator not active
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsDungeon( Tile tile ) {
			if( tile == null || !tile.active() ) { return false; }

			// Lihzahrd Brick Wall
			if( tile.wall == 87 ) {
				return true;
			}
			// Dungeon Walls
			if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
				return true;
			}
			return false;
		}

		public static bool IsWire( Tile tile ) {
			if( tile == null || !tile.active() ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}


		public static bool FindNearbyRandomAirTile( int world_x, int world_y, int radius, out int to_x, out int to_y ) {
			Tile tile = null;
			int wtf = 0;

			do {
				do { to_x = Main.rand.Next( -radius, radius ) + world_x; }
				while( to_x < 0 || to_x >= Main.mapMaxX );
				do { to_y = Main.rand.Next( -radius, radius ) + world_y; }
				while( to_y < 0 || to_y >= Main.mapMaxY );

				tile = Main.tile[to_x, to_y];
				if( wtf++ > 100 ) {
					return false;
				}
			} while( (TileHelpers.IsSolid(tile, false, false) || TileHelpers.IsDungeon(tile) || TileHelpers.IsWire(tile) || tile.lava())
				&& ((tile != null && tile.type != 0) || Lighting.Brightness(to_x, to_x) == 0) );

			return true;
		}


		public static bool IsNotBombable( int i, int j ) {
			Tile tile = Main.tile[i, j];

			return !TileLoader.CanExplode( i, j ) ||
				Main.tileDungeon[(int)tile.type] ||
				tile.type == 88 ||  // Dresser
				tile.type == 21 ||  // Chest
				tile.type == 26 ||  // Demon Altar
				tile.type == 107 || // Cobalt Ore
				tile.type == 108 || // Mythril Ore
				tile.type == 111 || // Adamantite Ore
				tile.type == 226 || // Lihzahrd Brick
				tile.type == 237 || // Lihzahrd Altar
				tile.type == 221 || // Palladium Ore
				tile.type == 222 || // Orichalcum Ore
				tile.type == 223 || // Titanium Ore
				tile.type == 211 || // Chlorophyte Ore
				tile.type == 404 || // Desert Fossil
				(!Main.hardMode && tile.type == 58);    // Hellstone Ore
		}
	}
}
