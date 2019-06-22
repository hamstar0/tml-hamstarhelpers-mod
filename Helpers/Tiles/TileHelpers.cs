using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/** <summary>Assorted static "helper" functions pertaining to tiles.</summary> */
	public static partial class TileHelpers {
		public static bool IsAir( Tile tile ) {
			return tile == null || (!tile.active() && tile.wall == 0) /*|| tile.type == 0*/;
		}

		
		public static bool IsSolid( Tile tile, bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			if( tile == null || !tile.active() ) { return false; }
			if( !Main.tileSolid[ tile.type ] ) { return false; }

			bool isTopSolid = Main.tileSolidTop[ tile.type ];
			bool isPassable = tile.inActive();

			if( !isPlatformSolid && isTopSolid ) { return false; }
			if( !isActuatedSolid && isPassable ) { return false; }
			
			return true;
		}


		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}


		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			return !TileLoader.CanExplode( tileX, tileY ) || TileHelpers.IsNotVanillaBombableType( tile );
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


		public static bool PlaceTile( int tileX, int tileY, int tileType, int placeStyle=0, bool muted=false, bool forced=false, int plrWho=-1 ) {
			if( !WorldGen.PlaceTile( tileX, tileY, tileType, muted, forced, plrWho, placeStyle ) ) {
				return false;
			}

			NetMessage.SendData( MessageID.TileChange, -1, -1, null, 1, (float)tileX, (float)tileY, (float)tileType, placeStyle, 0, 0 );

			if( Main.netMode == 1 ) {
				if( tileType == TileID.Chairs ) {
					NetMessage.SendTileSquare( -1, tileX - 1, tileY - 1, 3, TileChangeType.None );
				} else if( tileType == TileID.Beds || tileType == TileID.Bathtubs ) {
					NetMessage.SendTileSquare( -1, tileX, tileY, 5, TileChangeType.None );
				}
			}

			return true;
		}
	}
}
