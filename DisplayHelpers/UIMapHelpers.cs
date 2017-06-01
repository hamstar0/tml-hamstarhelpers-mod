using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.UIHelpers {
	public class UIMapHelpers {
		public static Vector2 GetFullMapPosition( Rectangle origin ) {    //Main.mapFullscreen
			float map_x = 200f;
			float map_y = 300f;

			float map_scale = Main.mapFullscreenScale;

			float offscr_lit_x = 10f * map_scale;
			float offscr_lit_y = 10f * map_scale;

			float map_fullscr_x = Main.mapFullscreenPos.X * map_scale;
			float map_fullscr_y = Main.mapFullscreenPos.Y * map_scale;
			map_x = -map_fullscr_x + (float)(Main.screenWidth / 2);
			map_y = -map_fullscr_y + (float)(Main.screenHeight / 2);

			float origin_mid_x = (origin.X / 16f) * map_scale;
			float origin_mid_y = (origin.Y / 16f) * map_scale;

			origin_mid_x += map_x;
			origin_mid_y += map_y;

			return new Vector2( origin_mid_x, origin_mid_y );
		}


		public static Vector2 GetOverlayMapPosition( Rectangle origin ) {    //Main.mapStyle == 2
			float map_x = 200f;
			float map_y = 300f;

			float map_scale = Main.mapOverlayScale;

			float offscr_lit_x = 10f * map_scale;
			float offscr_lit_y = 10f * map_scale;

			float scr_wrld_pos_mid_x = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
			float scr_wrld_pos_mid_y = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			scr_wrld_pos_mid_x *= map_scale;
			scr_wrld_pos_mid_y *= map_scale;
			map_x = -scr_wrld_pos_mid_x + (float)(Main.screenWidth / 2);
			map_y = -scr_wrld_pos_mid_y + (float)(Main.screenHeight / 2);

			float origin_mid_x = (origin.X / 16f) * map_scale;
			float origin_mid_y = (origin.Y / 16f) * map_scale;

			origin_mid_x += map_x;
			origin_mid_y += map_y;

			return new Vector2( origin_mid_x, origin_mid_y );
		}


		public static Vector2? GetMiniMapPosition( Rectangle origin ) {    //Main.mapStyle == 1
			float map_scale = Main.mapMinimapScale;

			float world_screen_pos_x = (Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f;
			float world_screen_pos_y = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			float minimap_wid_scaled = (float)Main.miniMapWidth / map_scale;
			float minimap_hei_scaled = (float)Main.miniMapHeight / map_scale;
			float minimap_world_x = (float)((int)world_screen_pos_x) - minimap_wid_scaled / 2f;
			float minimap_world_y = (float)((int)world_screen_pos_y) - minimap_hei_scaled / 2f;
			float float_remainder_x = (world_screen_pos_x - (float)((int)world_screen_pos_x)) * map_scale;
			float float_remainder_y = (world_screen_pos_y - (float)((int)world_screen_pos_y)) * map_scale;
			float origin_x_relative_to_map = (((origin.X + (float)(origin.Width / 2)) / 16f) - minimap_world_x) * map_scale;
			float origin_y_relative_to_map = (((origin.Y + (float)(origin.Height / 2)) / 16f) - minimap_world_y) * map_scale;
			float origin_x_screen_pos = origin_x_relative_to_map + (float)Main.miniMapX;
			float origin_y_screen_pos = origin_y_relative_to_map + (float)Main.miniMapY;
			origin_y_screen_pos -= 2f * map_scale / 5f;

			// Is on screen?
			if( origin_x_screen_pos > (float)(Main.miniMapX + 12) &&
					origin_x_screen_pos < (float)(Main.miniMapX + Main.miniMapWidth - 16) &&
					origin_y_screen_pos > (float)(Main.miniMapY + 10) &&
					origin_y_screen_pos < (float)(Main.miniMapY + Main.miniMapHeight - 14) ) {
				return new Vector2( origin_x_screen_pos - float_remainder_x, origin_y_screen_pos - float_remainder_y );
			}

			return null;
		}
	}
}
