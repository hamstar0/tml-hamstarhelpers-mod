using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static partial class TileHelpers {
		public static bool IsAir( Tile tile ) {
			return tile == null || (!tile.active() && tile.wall == 0) /*|| tile.type == 0*/;
		}

		
		public static bool IsSolid( Tile tile, bool is_platform_solid = false, bool is_actuated_solid = false ) {
			if( tile == null || !tile.active() ) { return false; }
			if( !Main.tileSolid[ tile.type ] ) { return false; }

			bool is_top_solid = Main.tileSolidTop[ tile.type ];
			bool is_passable = tile.inActive();

			if( !is_platform_solid && is_top_solid ) { return false; }
			if( !is_actuated_solid && is_passable ) { return false; }
			
			return true;
		}


		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}


		public static bool IsNotVanillaBombable( int tile_x, int tile_y ) {
			Tile tile = Framing.GetTileSafely( tile_x, tile_y );
			return !TileLoader.CanExplode( tile_x, tile_y ) || TileHelpers.IsNotVanillaBombableType( tile );
		}

		public static bool IsNotVanillaBombableType( Tile tile ) {
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


		public static bool PlaceTile( int x, int y, int tile_type, int place_style=0, bool muted=false, bool forced=false, int plr_who=-1 ) {
			if( !WorldGen.PlaceTile( x, y, tile_type, muted, forced, plr_who, place_style ) ) {
				return false;
			}

			NetMessage.SendData( MessageID.TileChange, -1, -1, null, 1, (float)x, (float)y, (float)tile_type, place_style, 0, 0 );

			if( Main.netMode == 1 ) {
				if( tile_type == TileID.Chairs ) {
					NetMessage.SendTileSquare( -1, x - 1, y - 1, 3, TileChangeType.None );
				} else if( tile_type == TileID.Beds || tile_type == TileID.Bathtubs ) {
					NetMessage.SendTileSquare( -1, x, y, 5, TileChangeType.None );
				}
			}

			return true;
		}
	}
}
