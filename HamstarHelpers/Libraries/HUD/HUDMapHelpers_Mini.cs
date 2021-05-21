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
		/// Gets the scale (zoom) of the mini map.
		/// </summary>
		/// <returns></returns>
		public static float GetMiniMapScale() {
			return Main.mapMinimapScale;
		}


		////////////////

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the mini-map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static (Vector2 ScreenPosition, bool IsOnScreen) GetMiniMapPositionAsScreenPosition( Vector2 worldPosition ) {    //Main.mapStyle == 1
			return HUDMapHelpers.GetMiniMapPositionAsScreenPosition( new Rectangle( (int)worldPosition.X, (int)worldPosition.Y, 0, 0 ) );
		}

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the mini-map.
		/// </summary>
		/// <param name="worldArea"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static (Vector2 ScreenPosition, bool IsOnScreen) GetMiniMapPositionAsScreenPosition( Rectangle worldArea ) {    //Main.mapStyle == 1
			float mapScale = Main.mapMinimapScale;

			float wldScreenPosX = ( Main.screenPosition.X + (float)( Main.screenWidth / 2 ) ) / 16f;
			float wldScreenPosY = ( Main.screenPosition.Y + (float)( Main.screenHeight / 2 ) ) / 16f;
			float minimapWidScaled = (float)Main.miniMapWidth / mapScale;
			float minimapHeiScaled = (float)Main.miniMapHeight / mapScale;
			float minimapWorldX = (float)( (int)wldScreenPosX ) - minimapWidScaled * 0.5f;
			float minimapWorldY = (float)( (int)wldScreenPosY ) - minimapHeiScaled * 0.5f;
			float floatRemainderX = ( wldScreenPosX - (float)( (int)wldScreenPosX ) ) * mapScale;
			float floatRemainderY = ( wldScreenPosY - (float)( (int)wldScreenPosY ) ) * mapScale;

			float originX = worldArea.X + (float)( worldArea.Width * 0.5f );
			float originY = worldArea.Y + (float)( worldArea.Height * 0.5f );
			float originXRelativeToMap = ( ( originX / 16f ) - minimapWorldX ) * mapScale;
			float originYRelativeToMap = ( ( originY / 16f ) - minimapWorldY ) * mapScale;
			float originXScreenPos = originXRelativeToMap + (float)Main.miniMapX;
			float originYScreenPos = originYRelativeToMap + (float)Main.miniMapY;
			originYScreenPos -= 2f * mapScale / 5f;

			var scrPos = new Vector2( originXScreenPos - floatRemainderX, originYScreenPos - floatRemainderY );
			bool isOnscreen = originXScreenPos > (float)( Main.miniMapX + 12 ) &&
					originXScreenPos < (float)( Main.miniMapX + Main.miniMapWidth - 16 ) &&
					originYScreenPos > (float)( Main.miniMapY + 10 ) &&
					originYScreenPos < (float)( Main.miniMapY + Main.miniMapHeight - 14 );

			return ( scrPos, isOnscreen );
		}


		////////////////

		/// <summary>
		/// Gets the scaled dimensions of a given width and height as if projectected onto the mini-map.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Vector2 GetSizeOnMinimap( float width, float height ) {   //Main.mapStyle == 1
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;
			
			Vector2 mapBasePos = HUDMapHelpers.GetMiniMapPositionAsScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).ScreenPosition;
			Vector2 mapNewPos = HUDMapHelpers.GetMiniMapPositionAsScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).ScreenPosition;

			return mapNewPos - mapBasePos;
		}
	}
}
