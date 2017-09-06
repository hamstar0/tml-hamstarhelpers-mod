﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.UIHelpers {
	public static class UIHelpers {
		[System.Obsolete( "use HudHelpers.DrawBorderedRect", true )]
		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color border_color, Vector2 position, Vector2 size, int border_width ) {
			HudHelpers.HudHelpers.DrawBorderedRect( sb, color, border_color, position, size, border_width );
		}


		public static Rectangle GetWorldFrameOfScreen() {
			int screen_wid = (int)((float)Main.screenWidth / Main.GameZoomTarget);
			int screen_hei = (int)((float)Main.screenHeight / Main.GameZoomTarget);
			int screen_x = (int)Main.screenPosition.X + ((Main.screenWidth - screen_wid) / 2);
			int screen_y = (int)Main.screenPosition.Y + ((Main.screenHeight - screen_hei) / 2);

			return new Rectangle( screen_x, screen_y, screen_wid, screen_hei );
		}
		
		public static Vector2 ConvertToScreenPosition( Vector2 world_pos ) {
			var frame = UIHelpers.GetWorldFrameOfScreen();
			var screen_pos = new Vector2( frame.X, frame.Y );

			return (world_pos - screen_pos) * Main.GameZoomTarget;
		}

		public static Vector2 GetWorldMousePosition() {
			Rectangle screen_frame = UIHelpers.GetWorldFrameOfScreen();
			Vector2 screen_mouse = UIHelpers.ConvertToScreenPosition( new Vector2( Main.mouseX, Main.mouseY ) + Main.screenPosition );
			return screen_mouse + new Vector2( screen_frame.X, screen_frame.Y );
		}
	}
}
