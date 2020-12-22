using HamstarHelpers.Helpers.Debug;
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
		/// Gets the scale (zoom) of the full screen map.
		/// </summary>
		/// <returns></returns>
		public static float GetFullMapScale() {
			return Main.mapFullscreenScale / Main.UIScale;
		}


		////////////////

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static (Vector2 ScreenPosition, bool IsOnScreen) GetFullMapPositionAsScreenPosition( Vector2 worldPosition ) {    //Main.mapFullscreen
			return HUDMapHelpers.GetFullMapPositionAsScreenPosition(
				new Rectangle( (int)worldPosition.X, (int)worldPosition.Y, 0, 0 )
			);
		}

		/// <summary>
		/// Returns a screen position of a given world position as if projected onto the fullscreen map.
		/// </summary>
		/// <param name="worldArea">An area in world coordinates.</param>
		/// <returns>A tuple indicating the screen-relative position and whether the point is within the screen
		/// boundaries.</returns>
		public static (Vector2 ScreenPosition, bool IsOnScreen) GetFullMapPositionAsScreenPosition( Rectangle worldArea ) {    //Main.mapFullscreen
			float mapScale = HUDMapHelpers.GetFullMapScale();
			//var scrSize = UIHelpers.GetScreenSize();
			var scrSize = UIZoomHelpers.GetScreenSize( null, false );

			//float offscrLitX = 10f * mapScale;
			//float offscrLitY = 10f * mapScale;

			float mapFullscrX = Main.mapFullscreenPos.X * mapScale;
			float mapFullscrY = Main.mapFullscreenPos.Y * mapScale;
			float mapX = -mapFullscrX + (float)(Main.screenWidth / 2);
			float mapY = -mapFullscrY + (float)(Main.screenHeight / 2);

			float originMidX = ((worldArea.X + (worldArea.Width / 2)) / 16f) * mapScale;
			float originMidY = ((worldArea.Y + (worldArea.Height / 2)) / 16f) * mapScale;

			originMidX += mapX;
			originMidY += mapY;

			var scrPos = new Vector2( originMidX, originMidY );
			bool isOnscreen = originMidX >= 0 &&
				originMidY >= 0 &&
				originMidX < scrSize.Item1 &&
				originMidY < scrSize.Item2;

			return ( scrPos, isOnscreen );
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

			Vector2 mapBasePos = HUDMapHelpers.GetFullMapPositionAsScreenPosition( new Rectangle( (int)baseX, (int)baseY, 0, 0 ) ).ScreenPosition;
			Vector2 mapNewPos = HUDMapHelpers.GetFullMapPositionAsScreenPosition( new Rectangle( (int)(baseX + width), (int)(baseY + height), 0, 0 ) ).ScreenPosition;

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


		////////////////

		/// <summary>
		/// Zooms in to find the top left tile position of the current map view.
		/// </summary>
		/// <returns></returns>
		public static (int TileX, int TileY, bool IsOnScreen) FindTopLeftTileOfFullscreenMap() {
			return HUDMapHelpers.FindTopLeftTileOfFullscreenMapStartingAt( Main.maxTilesX / 2, Main.maxTilesY / 2 );
		}

		private static (int TileX, int TileY, bool IsOnScreen) FindTopLeftTileOfFullscreenMapStartingAt( int tileX, int tileY ) {
			int prevLeftX = Main.maxTilesX * -32;
			int prevRightX = Main.maxTilesX * 32;
			int prevTopY = Main.maxTilesY * -32;
			int prevBotY = Main.maxTilesY * 32;

			//

			void IncreaseX( ref int x ) {
				prevLeftX = x;
				x += ( prevRightX - x ) / 2;
			}
			void DecreaseX( ref int x ) {
				prevRightX = x;
				x += ( prevLeftX - x ) / 2;
			}
			void IncreaseY( ref int y ) {
				prevTopY = y;
				y += ( prevBotY - y ) / 2;
			}
			void DecreaseY( ref int y ) {
				prevBotY = y;
				y += ( prevTopY - y ) / 2;
			}

			//

			(Vector2 ScreenPosition, bool) mapPos;
			float prevScrX = float.MaxValue;
			float prefScrY = float.MaxValue;

			while( true ) {
				mapPos = HUDMapHelpers.GetFullMapPositionAsScreenPosition( new Vector2(tileX << 4, tileY << 4) );
//LogHelpers.LogOnce( "x:"+tileX+", y:"+tileY+" -- lx:"+prevLeftX+", rx:"+prevRightX+", uy:"+prevTopY+", dy:"+prevBotY+" -- sx:"+mapPos.ScreenPosition.ToString());

				if( mapPos.ScreenPosition.X < 0f ) {
					IncreaseX( ref tileX );
				} else if( mapPos.ScreenPosition.X > 0f ) {
					DecreaseX( ref tileX );
				}

				if( mapPos.ScreenPosition.Y < 0f ) {
					IncreaseY( ref tileY );
				} else if( mapPos.ScreenPosition.Y > 0f ) {
					DecreaseY( ref tileY );
				}

				if( mapPos.ScreenPosition.X == prevScrX && mapPos.ScreenPosition.Y == prefScrY ) {
					bool isOnScreen = tileX < 0 || tileY < 0;
					return (tileX, tileY, isOnScreen);
				}

				prevScrX = mapPos.ScreenPosition.X;
				prefScrY = mapPos.ScreenPosition.Y;
			}
		}
	}
}
