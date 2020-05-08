using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Tiles {
	/// <summary>
	/// Represents a tile that works like a standard corruption/crimson/jungle bramble, but cannot be removed by melee weapons,
	/// and may support additional custom behavior.
	/// </summary>
	public partial class CursedBrambleTile : ModTile {
		/// <summary>
		/// Attempts to remove a random bramble within a given radius of a given tile.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="radius"></param>
		/// <returns>`true` if a bramble was found and removed.</returns>
		public static bool ErodeRandomBrambleWithinRadius( int tileX, int tileY, int radius ) {
			int randX = TmlHelpers.SafelyGetRand().Next( radius * 2 );
			int randY = TmlHelpers.SafelyGetRand().Next( radius * 2 );
			int randTileX = tileX + ( randX - radius );
			int randTileY = tileY + ( randY - radius );

			Tile tile = Framing.GetTileSafely( randTileX, randTileY );
			if( !tile.active() || tile.type != ModContent.TileType<CursedBrambleTile>() ) {
				return false;
			}

			TileHelpers.KillTileSynced( tileX, tileY, false, false );
			return true;
		}
	}
}
