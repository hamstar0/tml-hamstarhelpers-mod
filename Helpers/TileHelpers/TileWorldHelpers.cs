using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.TileHelpers {
	public static class TileWorldHelpers {
		public static bool IsWithinMap( int tileX, int tileY ) {
			return (tileX > 41 && tileX < Main.maxTilesX - 42) && (tileY > 41 && tileY < Main.maxTilesY - 42);
		}


		public static float GaugeBrightnessWithin( int tileX, int tileY, int width, int height ) {
			int i = 0, j = 0;
			float avg = 0f;

			for( i = 0; i < width; i++ ) {
				for( j = 0; j < height; j++ ) {
					avg += Lighting.Brightness( tileX + i, tileY + j );
				}
			}

			return avg / (i * j);
		}


		public static Vector2 DropToGround( Vector2 worldPos ) {
			int x = (int)worldPos.X / 16;
			int y = (int)worldPos.Y / 16;

			do {
				y++;
			} while( y <= (Main.maxTilesY - 42) && !TileHelpers.IsSolid( Framing.GetTileSafely(x, y) ) );
			y--;

			return new Vector2( worldPos.X, y * 16 );
		}
	}
}
