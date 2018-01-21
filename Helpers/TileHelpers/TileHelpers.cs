using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.TileHelpers {
	public static class TileHelpers {
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


		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}


		public static bool IsNotBombable( int tile_x, int tile_y ) {
			//Tile tile = Main.tile[i, j];
			Tile tile = Framing.GetTileSafely( tile_x, tile_y );
			return !TileLoader.CanExplode( tile_x, tile_y ) || TileHelpers.IsNotBombableType( tile );
		}

		public static bool IsNotBombableType( Tile tile ) {
			return Main.tileDungeon[(int)tile.type] ||
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


		[System.Obsolete( "use TileWorldHelpers.GaugeBrightnessWithin", true )]
		public static float GaugeBrightnessWithin( int tile_x, int tile_y, int width, int height ) {
			return TileWorldHelpers.GaugeBrightnessWithin( tile_x, tile_y, width, height );
		}


		[System.Obsolete( "use TileWorldHelpers.DropToGround", true )]
		public static Vector2 DropToGround( Vector2 world_pos ) {
			return TileWorldHelpers.DropToGround( world_pos );
		}


		[System.Obsolete( "use TileWorldHelpers.IsWithinMap", true )]
		public static bool IsWithinMap( int tile_x, int tile_y ) {
			return TileWorldHelpers.IsWithinMap( tile_x, tile_y );
		}


		[System.Obsolete( "use TileWallHelpers.IsDungeon", true )]
		public static bool IsDungeon( Tile tile ) {
			return TileWallHelpers.IsDungeon( tile );
		}


		[System.Obsolete( "use TileFinderHelpers.HasNearbySolid", true )]
		public static bool HasNearbySolid( int tile_x, int tile_y, int proximity_in_tiles ) {
			return TileFinderHelpers.HasNearbySolid( tile_x, tile_y, proximity_in_tiles );
		}


		[System.Obsolete( "use TileFinderHelpers.FindNearbyRandomAirTile", true )]
		public static bool FindNearbyRandomAirTile( int tile_x, int tile_y, int radius, out int to_x, out int to_y ) {
			return TileFinderHelpers.FindNearbyRandomAirTile( tile_x, tile_y, radius, out to_x, out to_y );
		}
	}
}
