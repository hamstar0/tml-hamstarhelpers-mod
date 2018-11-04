using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.HudHelpers {
	public static partial class HudMapHelpers {
		public static bool GetFullMapScreenPosition( Rectangle origin, out Vector2 position ) {    //Main.mapFullscreen
			float map_scale = Main.mapFullscreenScale / Main.UIScale;
			var scr_size = UIHelpers.UIHelpers.GetScreenSize();

			float offscr_lit_x = 10f * map_scale;
			float offscr_lit_y = 10f * map_scale;

			float map_fullscr_x = Main.mapFullscreenPos.X * map_scale;
			float map_fullscr_y = Main.mapFullscreenPos.Y * map_scale;
			float map_x = -map_fullscr_x + (float)(Main.screenWidth / 2);
			float map_y = -map_fullscr_y + (float)(Main.screenHeight / 2);

			float origin_mid_x = (origin.X / 16f) * map_scale;
			float origin_mid_y = (origin.Y / 16f) * map_scale;

			origin_mid_x += map_x;
			origin_mid_y += map_y;

			position = new Vector2( origin_mid_x, origin_mid_y );
			return origin_mid_x >= 0 && origin_mid_y >= 0 && origin_mid_x < scr_size.Item1 && origin_mid_y < scr_size.Item2;
		}


		public static bool GetOverlayMapScreenPosition( Rectangle origin, out Vector2 position ) {    //Main.mapStyle == 2
			float map_scale = Main.mapOverlayScale;
			var scr_size = UIHelpers.UIHelpers.GetScreenSize();

			float offscr_lit_x = 10f * map_scale;
			float offscr_lit_y = 10f * map_scale;

			float scr_wrld_pos_mid_x = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
			float scr_wrld_pos_mid_y = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			scr_wrld_pos_mid_x *= map_scale;
			scr_wrld_pos_mid_y *= map_scale;
			float map_x = -scr_wrld_pos_mid_x + (float)(Main.screenWidth / 2);
			float map_y = -scr_wrld_pos_mid_y + (float)(Main.screenHeight / 2);

			float origin_mid_x = (origin.X / 16f) * map_scale;
			float origin_mid_y = (origin.Y / 16f) * map_scale;

			origin_mid_x += map_x;
			origin_mid_y += map_y;

			position = new Vector2( origin_mid_x, origin_mid_y );
			return origin_mid_x >= 0 && origin_mid_y >= 0 && origin_mid_x < scr_size.Item1 && origin_mid_y < scr_size.Item2;
		}


		public static bool GetMiniMapScreenPosition( Rectangle origin, out Vector2 position ) {    //Main.mapStyle == 1
			float map_scale = Main.mapMinimapScale;

			float world_screen_pos_x = ( Main.screenPosition.X + (float)( Main.screenWidth / 2 ) ) / 16f;
			float world_screen_pos_y = ( Main.screenPosition.Y + (float)( Main.screenHeight / 2 ) ) / 16f;
			float minimap_wid_scaled = (float)Main.miniMapWidth / map_scale;
			float minimap_hei_scaled = (float)Main.miniMapHeight / map_scale;
			float minimap_world_x = (float)( (int)world_screen_pos_x ) - minimap_wid_scaled * 0.5f;
			float minimap_world_y = (float)( (int)world_screen_pos_y ) - minimap_hei_scaled * 0.5f;
			float float_remainder_x = ( world_screen_pos_x - (float)( (int)world_screen_pos_x ) ) * map_scale;
			float float_remainder_y = ( world_screen_pos_y - (float)( (int)world_screen_pos_y ) ) * map_scale;

			float origin_x = origin.X + (float)( origin.Width * 0.5f );
			float origin_y = origin.Y + (float)( origin.Height * 0.5f );
			float origin_x_relative_to_map = ( ( origin_x / 16f ) - minimap_world_x ) * map_scale;
			float origin_y_relative_to_map = ( ( origin_y / 16f ) - minimap_world_y ) * map_scale;
			float origin_x_screen_pos = origin_x_relative_to_map + (float)Main.miniMapX;
			float origin_y_screen_pos = origin_y_relative_to_map + (float)Main.miniMapY;
			origin_y_screen_pos -= 2f * map_scale / 5f;

			position = new Vector2( origin_x_screen_pos - float_remainder_x, origin_y_screen_pos - float_remainder_y );

			// Is on screen?
			return	origin_x_screen_pos > (float)( Main.miniMapX + 12 ) &&
					origin_x_screen_pos < (float)( Main.miniMapX + Main.miniMapWidth - 16 ) &&
					origin_y_screen_pos > (float)( Main.miniMapY + 10 ) &&
					origin_y_screen_pos < (float)( Main.miniMapY + Main.miniMapHeight - 14 );
		}


		////////////////

		public static Vector2 GetSizeOnFullMap( int width, int height ) {	//Main.mapFullscreen 
			int base_x = (int)Main.screenPosition.X;
			int base_y = (int)Main.screenPosition.Y;

			Vector2 map_base_pos, map_new_pos;
			HudMapHelpers.GetFullMapScreenPosition( new Rectangle( base_x, base_y, 0, 0 ), out map_base_pos );
			HudMapHelpers.GetFullMapScreenPosition( new Rectangle( base_x + width, base_y + height, 0, 0 ), out map_new_pos );

			return map_new_pos - map_base_pos;
		}

		public static Vector2 GetSizeOnOverlayMap( int width, int height ) {	//Main.mapStyle == 2
			int base_x = (int)Main.screenPosition.X;
			int base_y = (int)Main.screenPosition.Y;

			Vector2 map_base_pos, map_new_pos;
			HudMapHelpers.GetOverlayMapScreenPosition( new Rectangle( base_x, base_y, 0, 0 ), out map_base_pos );
			HudMapHelpers.GetOverlayMapScreenPosition( new Rectangle( base_x + width, base_y + height, 0, 0 ), out map_new_pos );

			return map_new_pos - map_base_pos;
		}

		public static Vector2 GetSizeOnMinimap( int width, int height ) {   //Main.mapStyle == 1
			int base_x = (int)Main.screenPosition.X;
			int base_y = (int)Main.screenPosition.Y;

			Vector2 map_base_pos, map_new_pos;
			HudMapHelpers.GetMiniMapScreenPosition( new Rectangle( base_x, base_y, 0, 0 ), out map_base_pos );
			HudMapHelpers.GetMiniMapScreenPosition( new Rectangle( base_x + width, base_y + height, 0, 0 ), out map_new_pos );

			return map_new_pos - map_base_pos;
		}
	}
}
