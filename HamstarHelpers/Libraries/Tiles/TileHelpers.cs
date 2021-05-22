using HamstarHelpers.Libraries.Debug;
using Terraria;


namespace HamstarHelpers.Libraries.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileLibraries {
		/// <summary>
		/// Reports if a tile is exactly identical to another tile.
		/// </summary>
		/// <param name="tile1"></param>
		/// <param name="tile2"></param>
		/// <returns></returns>
		public static bool IsEqual( Tile tile1, Tile tile2 ) {
			return tile1.type == tile2.type
				&& tile1.wall == tile2.wall
				&& tile1.frameY == tile2.frameY
				&& tile1.frameX == tile2.frameX
				&& tile1.bTileHeader == tile2.bTileHeader
				&& tile1.bTileHeader3 == tile2.bTileHeader3
				&& tile1.bTileHeader2 == tile2.bTileHeader2
				&& tile1.sTileHeader == tile2.sTileHeader
				&& tile1.liquid == tile2.liquid;
		}


		/// <summary>
		/// Indicates if a given tile is "air" (including no walls).
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isWireAir"></param>
		/// <param name="isLiquidAir"></param>
		/// <returns></returns>
		public static bool IsAir( Tile tile, bool isWireAir = false, bool isLiquidAir = false ) {
			if( tile == null ) {
				return true;
			}
			if( tile.active() || tile.wall > 0 ) {/*|| tile.type == 0*/
				return false;
			}
			if( !isWireAir && TileLibraries.IsWire(tile) ) {
				return false;
			}
			if( !isLiquidAir && tile.liquid != 0 ) {
				return false;
			}

			return true;
		}

		
		/// <summary>
		/// Indicates if a given tile is "solid".
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="isPlatformSolid"></param>
		/// <param name="isActuatedSolid"></param>
		/// <returns></returns>
		public static bool IsSolid( Tile tile, bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			if( TileLibraries.IsAir(tile) ) { return false; }
			if( !Main.tileSolid[tile.type] || !tile.active() ) { return false; }

			if( !isPlatformSolid ) {
				bool isTopSolid = Main.tileSolidTop[tile.type];
				if( isTopSolid ) {
					return false;
				}
			}

			if( !isActuatedSolid && tile.inActive() ) {
				return false;
			}

			return true;
		}


		/// <summary>
		/// Indicates if a given tile has wires.
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		public static bool IsWire( Tile tile ) {
			if( tile == null /*|| !tile.active()*/ ) { return false; }
			return tile.wire() || tile.wire2() || tile.wire3() || tile.wire4();
		}
	}
}
