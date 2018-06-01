using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.TileHelpers {
	public static partial class TileHelpers {
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
