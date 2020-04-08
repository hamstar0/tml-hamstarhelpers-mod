using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HUD {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the HUD map.
	/// </summary>
	public partial class HUDMapHelpers {
		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Vector2 worldPosition ) {    //Main.mapFullscreen
			return HUDMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)worldPosition.X, (int)worldPosition.Y, 0, 0 ) );
		}

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Rectangle worldPosition ) {    //Main.mapFullscreen
			float mapScale = Main.mapFullscreenScale / Main.UIScale;
			var scrSize = UIHelpers.GetScreenSize();

			//float offscrLitX = 10f * mapScale;
			//float offscrLitY = 10f * mapScale;

			float mapFullscrX = Main.mapFullscreenPos.X * mapScale;
			float mapFullscrY = Main.mapFullscreenPos.Y * mapScale;
			float mapX = -mapFullscrX + (float)(Main.screenWidth / 2);
			float mapY = -mapFullscrY + (float)(Main.screenHeight / 2);

			float originMidX = (worldPosition.X / 16f) * mapScale;
			float originMidY = (worldPosition.Y / 16f) * mapScale;

			originMidX += mapX;
			originMidY += mapY;

			var scrPos = new Vector2( originMidX, originMidY );
			bool isOnscreen = originMidX >= 0 &&
				originMidY >= 0 &&
				originMidX < scrSize.Item1 &&
				originMidY < scrSize.Item2;

			return Tuple.Create( scrPos, isOnscreen );
		}


		////////////////

		/// <summary>
		/// Gets the scaled dimensions of a given width and height as if projectected onto the full screen map.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Vector2 GetSizeOnFullMap( float width, float height ) {   //Main.mapFullscreen 
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;

			Vector2 mapBasePos = HUDMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).Item1;
			Vector2 mapNewPos = HUDMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).Item1;

			return mapNewPos - mapBasePos;
		}


		////////////////

		/// <summary>
		/// Outputs the world tile position of a given screen position in the fullscreen map.
		/// </summary>
		/// <param name="scrPosX"></param>
		/// <param name="scrPosY"></param>
		/// <param name="tilePos"></param>
		/// <returns>`true` if the tile is within world bounds.</returns>
		public static bool GetFullscreenMapTileOfScreenPosition( int scrPosX, int scrPosY, out (int x, int y) tilePos ) {
			float mapScale = Main.mapFullscreenScale;
			float minX = 10f;
			float minY = 10f;
			float maxX = (float)( Main.maxTilesX - 10 );
			float maxY = (float)( Main.maxTilesY - 10 );

			float mapPosX = Main.mapFullscreenPos.X * mapScale;
			float mapPosY = Main.mapFullscreenPos.Y * mapScale;

			float scrOriginX = (float)(Main.screenWidth / 2) - mapPosX;
			scrOriginX += minX * mapScale;
			float scrOriginY = (float)(Main.screenHeight / 2) - mapPosY;
			scrOriginY += minX * mapScale;

			int tileX = (int)(((float)scrPosX - scrOriginX) / mapScale + minX);
			int tileY = (int)(((float)scrPosY - scrOriginY) / mapScale + minY);

			tilePos = (tileX, tileY);
			return tileX >= minX
				&& tileX < maxX
				&& tileY >= minY
				&& tileY < maxY;
		}
	}
}
