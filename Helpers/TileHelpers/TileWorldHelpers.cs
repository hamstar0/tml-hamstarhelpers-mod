using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.TileHelpers {
	public static class TileWorldHelpers {
		public static bool IsWithinMap( int tile_x, int tile_y ) {
			return (tile_x > 41 && tile_x < Main.maxTilesX - 42) && (tile_y > 41 && tile_y < Main.maxTilesY - 42);
		}


		public static float GaugeBrightnessWithin( int tile_x, int tile_y, int width, int height ) {
			int i = 0, j = 0;
			float avg = 0f;

			for( i = 0; i < width; i++ ) {
				for( j = 0; j < height; j++ ) {
					avg += Lighting.Brightness( tile_x + i, tile_y + j );
				}
			}

			return avg / (i * j);
		}


		public static Vector2 DropToGround( Vector2 world_pos ) {
			int x = (int)world_pos.X / 16;
			int y = (int)world_pos.Y / 16;

			do {
				y++;
			} while( y <= (Main.maxTilesY - 42) && !TileHelpers.IsSolid( Framing.GetTileSafely( x, y ) ) );
			y--;

			return new Vector2( world_pos.X, y * 16 );
		}
	}
}
