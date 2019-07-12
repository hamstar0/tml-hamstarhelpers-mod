using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
		/// <summary>
		/// Indicates if a given tile is "air".
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		public static bool IsAir( Tile tile ) {
			return tile == null || (!tile.active() && tile.wall == 0) /*|| tile.type == 0*/;
		}

		
		/// <summary>
		/// Indicates if a given tile is "solid".
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isPlatformSolid"></param>
		/// <param name="isActuatedSolid"></param>
		/// <returns></returns>
		public static bool IsSolid( Tile tile, bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			if( tile == null || !tile.active() ) { return false; }
			if( !Main.tileSolid[ tile.type ] ) { return false; }

			bool isTopSolid = Main.tileSolidTop[ tile.type ];
			bool isPassable = tile.inActive();

			if( !isPlatformSolid && isTopSolid ) { return false; }
			if( !isActuatedSolid && isPassable ) { return false; }
			
			return true;
		}


		/// <summary>
		/// Indicates if a given tile has wires.
		/// </summary>
		/// <param name="tile"></param>
		/// <returns>A bit flag set representing which wire types exist (of the 4). If 0, no wires set.</returns>
		public static byte IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return 0; }
			return (byte)((tile.wire() ? 1 : 0)
				+ (tile.wire2() ? 2 : 0)
				+ (tile.wire3() ? 4 : 0)
				+ (tile.wire4() ? 8 : 0));
		}


		/// <summary>
		/// Indicates if a given tile cannot be destroyed by vanilla explosives.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsNotVanillaBombable( int tileX, int tileY ) {
			Tile tile = Framing.GetTileSafely( tileX, tileY );
			return !TileLoader.CanExplode( tileX, tileY ) || TileHelpers.IsNotVanillaBombableType( tile.type );
		}

		/// <summary>
		/// Indicates if a given tile type cannot be destroyed by vanilla explosives.
		/// </summary>
		/// <param name="tileType"></param>
		/// <returns></returns>
		public static bool IsNotVanillaBombableType( int tileType ) {
			return Main.tileDungeon[tileType] ||
				tileType == TileID.Dressers ||
				tileType == TileID.Containers ||
				tileType == TileID.DemonAltar ||
				tileType == TileID.Cobalt ||
				tileType == TileID.Mythril ||
				tileType == TileID.Adamantite||
				tileType == TileID.LihzahrdBrick ||
				tileType == TileID.LihzahrdAltar ||
				tileType == TileID.Palladium ||
				tileType == TileID.Orichalcum ||
				tileType == TileID.Titanium ||
				tileType == TileID.Chlorophyte ||
				tileType == TileID.DesertFossil ||
				(!Main.hardMode && tileType == TileID.Hellstone);
		}


		/// <summary>
		/// Places a given tile of a given type. Synced.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="tileType"></param>
		/// <param name="placeStyle"></param>
		/// <param name="muted"></param>
		/// <param name="forced"></param>
		/// <param name="plrWho"></param>
		/// <returns></returns>
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
