using HamstarHelpers.Helpers.Debug;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles.
	/// </summary>
	public partial class TileHelpers {
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
			if( !isWireAir && TileHelpers.IsWire(tile) ) {
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
			if( TileHelpers.IsAir(tile) ) { return false; }
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
