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
		/// Returns a screen position of a given world position as if projected onto the overlay map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Vector2 worldPosition ) {    //Main.mapStyle == 2
			return HUDMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)worldPosition.X, (int)worldPosition.Y, 0, 0 ) );
		}

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the overlay map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Rectangle worldPosition ) {    //Main.mapStyle == 2
			float mapScale = Main.mapOverlayScale;
			var scrSize = UIHelpers.GetScreenSize();

			float offscrLitX = 10f * mapScale;
			float offscrLitY = 10f * mapScale;

			float scrWrldPosMidX = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
			float scrWrldPosMidY = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			scrWrldPosMidX *= mapScale;
			scrWrldPosMidY *= mapScale;
			float mapX = -scrWrldPosMidX + (float)(Main.screenWidth / 2);
			float mapY = -scrWrldPosMidY + (float)(Main.screenHeight / 2);

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
		/// Gets the scaled dimensions of a given width and height as if projectected onto the overlay map.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Vector2 GetSizeOnOverlayMap( float width, float height ) {    //Main.mapStyle == 2
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;

			Vector2 mapBasePos = HUDMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).Item1;
			Vector2 mapNewPos = HUDMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).Item1;

			return mapNewPos - mapBasePos;
		}
	}
}
