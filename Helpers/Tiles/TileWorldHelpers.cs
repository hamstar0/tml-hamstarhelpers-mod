using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/** <summary>Assorted static "helper" functions pertaining to tiles relative to the world.</summary> */
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
	}
}
