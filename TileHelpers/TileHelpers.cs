using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
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
				bool is_top_solid = Main.tileSolidTop[(int)tile.type];
				bool is_passable = tile.inActive();

				if( !is_platform_solid && is_top_solid ) { return false; }
				if( !is_actuated_solid && is_passable ) { return false; }
				return true;
			}
			return false;
		}

		public static bool IsDungeon( Tile tile ) {
			if( tile == null ) { return false; }

			// Lihzahrd Brick Wall
			//if( tile.wall == 87 ) {
			if( tile.wall == (ushort)WallID.LihzahrdBrickUnsafe /*|| tile.wall == (ushort)WallID.LihzahrdBrick*/ ) {
				return true;
			}
			// Dungeon Walls
			//if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
			if( tile.wall == (ushort)WallID.BlueDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonSlabUnsafe ||
				tile.wall == (ushort)WallID.BlueDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonTileUnsafe ||
				tile.wall == (ushort)WallID.BlueDungeonUnsafe ||
				tile.wall == (ushort)WallID.GreenDungeonUnsafe ||
				tile.wall == (ushort)WallID.PinkDungeonUnsafe ) {
				return true;
			}
			return false;
		}

		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}


		public static bool FindNearbyRandomAirTile( int world_x, int world_y, int radius, out int to_x, out int to_y ) {
			Tile tile = null;
			int wtf = 0;
			bool is_blocked = false;
			to_x = 0;
			to_y = 0;

			if( world_x + radius <= 0 || world_x - radius >= Main.mapMaxX ) { return false; }
			if( world_y + radius <= 0 || world_y - radius >= Main.mapMaxY ) { return false; }

			do {
				do { to_x = Main.rand.Next( -radius, radius ) + world_x; }
				while( to_x <= 0 || to_x >= Main.mapMaxX );
				do { to_y = Main.rand.Next( -radius, radius ) + world_y; }
				while( to_y <= 0 || to_y >= Main.mapMaxY );

				//tile = Main.tile[to_x, to_y];
				tile = Framing.GetTileSafely( to_x, to_y );
				if( wtf++ > 100 ) {
					return false;
				}

				is_blocked = TileHelpers.IsSolid( tile, true, true ) ||
					TileHelpers.IsDungeon( tile ) ||
					TileHelpers.IsWire( tile ) ||
					tile.lava();
			} while( is_blocked && ((tile != null && tile.type != 0) || Lighting.Brightness(to_x, to_x) == 0) );

			return true;
		}


		public static bool IsNotBombable( int world_x, int world_y ) {
			//Tile tile = Main.tile[i, j];
			Tile tile = Framing.GetTileSafely( world_x, world_y );

			return !TileLoader.CanExplode( world_x, world_y ) ||
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


		public static bool IsWithinMap( int tile_x, int tile_y ) {
			return (tile_x > 41 && tile_x < Main.maxTilesX - 42) && (tile_y > 41 && tile_y < Main.maxTilesY - 42);
		}


		public static Vector2 DropToGround( Vector2 world_pos ) {
			int x = (int)world_pos.X / 16;
			int y = (int)world_pos.Y / 16;

			do {
				y++;
			} while( (y * 16) <= (Main.maxTilesY - 42) && !TileHelpers.IsSolid( Framing.GetTileSafely(x, y) ) );
			y--;

			return new Vector2( world_pos.X, y * 16 );
		}


		public static bool HasNearbySolid( int tile_x, int tile_y, int proximity_in_tiles ) {
			int min_x = Math.Max( tile_x - proximity_in_tiles, 0 );
			int max_x = Math.Min( tile_x + proximity_in_tiles, Main.maxTilesX - 1 );
			int min_y = Math.Max( tile_y - proximity_in_tiles, 0 );
			int max_y = Math.Min( tile_y + proximity_in_tiles, Main.maxTilesY - 1 );

			for( int i = min_x; i <= max_x; i++ ) {
				for( int j = min_y; j <= max_y; j++ ) {
					if( TileHelpers.IsSolid(Main.tile[i, j]) ) {
						return true;
					}
				}
			}

			return false;
		}
	}
}
