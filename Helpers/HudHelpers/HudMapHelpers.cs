using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.HudHelpers {
	public static partial class HudMapHelpers {
		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Vector2 position ) {    //Main.mapFullscreen
			return HudMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)position.X, (int)position.Y, 0, 0 ) );
		}

		public static Tuple<Vector2, bool> GetFullMapScreenPosition( Rectangle position ) {    //Main.mapFullscreen
			float mapScale = Main.mapFullscreenScale / Main.UIScale;
			var scrSize = UIHelpers.UIHelpers.GetScreenSize();

			float offscrLitX = 10f * mapScale;
			float offscrLitY = 10f * mapScale;

			float mapFullscrX = Main.mapFullscreenPos.X * mapScale;
			float mapFullscrY = Main.mapFullscreenPos.Y * mapScale;
			float mapX = -mapFullscrX + (float)(Main.screenWidth / 2);
			float mapY = -mapFullscrY + (float)(Main.screenHeight / 2);

			float originMidX = (position.X / 16f) * mapScale;
			float originMidY = (position.Y / 16f) * mapScale;

			originMidX += mapX;
			originMidY += mapY;

			var scrPos = new Vector2( originMidX, originMidY );
			bool isOnscreen = originMidX >= 0 &&
				originMidY >= 0 &&
				originMidX < scrSize.Item1 &&
				originMidY < scrSize.Item2;

			return Tuple.Create( scrPos, isOnscreen );
		}


		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Vector2 position ) {    //Main.mapStyle == 2
			return HudMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)position.X, (int)position.Y, 0, 0 ) );
		}

		public static Tuple<Vector2, bool> GetOverlayMapScreenPosition( Rectangle position ) {    //Main.mapStyle == 2
			float mapScale = Main.mapOverlayScale;
			var scrSize = UIHelpers.UIHelpers.GetScreenSize();

			float offscrLitX = 10f * mapScale;
			float offscrLitY = 10f * mapScale;

			float scrWrldPosMidX = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
			float scrWrldPosMidY = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			scrWrldPosMidX *= mapScale;
			scrWrldPosMidY *= mapScale;
			float mapX = -scrWrldPosMidX + (float)(Main.screenWidth / 2);
			float mapY = -scrWrldPosMidY + (float)(Main.screenHeight / 2);

			float originMidX = (position.X / 16f) * mapScale;
			float originMidY = (position.Y / 16f) * mapScale;

			originMidX += mapX;
			originMidY += mapY;

			var scrPos = new Vector2( originMidX, originMidY );
			bool isOnscreen = originMidX >= 0 &&
				originMidY >= 0 &&
				originMidX < scrSize.Item1 &&
				originMidY < scrSize.Item2;

			return Tuple.Create( scrPos, isOnscreen );
		}


		public static Tuple<Vector2, bool> GetMiniMapScreenPosition( Vector2 position ) {    //Main.mapStyle == 1
			return HudMapHelpers.GetMiniMapScreenPosition( new Rectangle( (int)position.X, (int)position.Y, 0, 0 ) );
		}

		public static Tuple<Vector2, bool> GetMiniMapScreenPosition( Rectangle position ) {    //Main.mapStyle == 1
			float mapScale = Main.mapMinimapScale;

			float wldScreenPosX = ( Main.screenPosition.X + (float)( Main.screenWidth / 2 ) ) / 16f;
			float wldScreenPosY = ( Main.screenPosition.Y + (float)( Main.screenHeight / 2 ) ) / 16f;
			float minimapWidScaled = (float)Main.miniMapWidth / mapScale;
			float minimapHeiScaled = (float)Main.miniMapHeight / mapScale;
			float minimapWorldX = (float)( (int)wldScreenPosX ) - minimapWidScaled * 0.5f;
			float minimapWorldY = (float)( (int)wldScreenPosY ) - minimapHeiScaled * 0.5f;
			float floatRemainderX = ( wldScreenPosX - (float)( (int)wldScreenPosX ) ) * mapScale;
			float floatRemainderY = ( wldScreenPosY - (float)( (int)wldScreenPosY ) ) * mapScale;

			float originX = position.X + (float)( position.Width * 0.5f );
			float originY = position.Y + (float)( position.Height * 0.5f );
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

			return Tuple.Create( scrPos, isOnscreen );
		}


		////////////////

		public static Vector2 GetSizeOnFullMap( float width, float height ) {   //Main.mapFullscreen 
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;

			Vector2 mapBasePos = HudMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).Item1;
			Vector2 mapNewPos = HudMapHelpers.GetFullMapScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).Item1;

			return mapNewPos - mapBasePos;
		}

		public static Vector2 GetSizeOnOverlayMap( float width, float height ) {    //Main.mapStyle == 2
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;

			Vector2 mapBasePos = HudMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).Item1;
			Vector2 mapNewPos = HudMapHelpers.GetOverlayMapScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).Item1;

			return mapNewPos - mapBasePos;
		}

		public static Vector2 GetSizeOnMinimap( float width, float height ) {   //Main.mapStyle == 1
			float baseX = Main.screenPosition.X;
			float baseY = Main.screenPosition.Y;
			
			Vector2 mapBasePos = HudMapHelpers.GetMiniMapScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).Item1;
			Vector2 mapNewPos = HudMapHelpers.GetMiniMapScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).Item1;

			return mapNewPos - mapBasePos;
		}
	}
}
