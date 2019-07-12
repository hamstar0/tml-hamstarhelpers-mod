using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tiles relative to the world.
	/// </summary>
	public static class TileWorldHelpers {
		/// <summary>
		/// Indicates if a given tiles is within the visible map's boundaries.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <returns></returns>
		public static bool IsWithinMap( int tileX, int tileY ) {
			return (tileX > 41 && tileX < Main.maxTilesX - 42) && (tileY > 41 && tileY < Main.maxTilesY - 42);
		}


		/// <summary>
		/// Gauges the overall light amount within an area.
		/// </summary>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
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
