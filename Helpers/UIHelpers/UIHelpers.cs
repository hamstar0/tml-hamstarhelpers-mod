using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UIHelpers {
	public static class UIHelpers {
		public static readonly int MinScreenWidth = 800;
		public static readonly int MinScreenHeight = 640;
		public static readonly int MaxScreenWidth = 1920;
		public static readonly int MaxScreenHeight = 1080;
		public static readonly int MinScreenWidthTiles = 80;
		public static readonly int MinScreenHeightTiles = 40;
		public static readonly int MaxScreenWidthTiles = 124;   //1920 + 64
		public static readonly int MaxScreenHeightTiles = 70;   //1080 + 40



		////////////////

		public static Tuple<int, int> GetScreenSize() {
			int screen_wid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screen_hei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );

			return Tuple.Create( screen_wid, screen_hei );
		}

		public static Rectangle GetWorldFrameOfScreen() {
			int screen_wid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screen_hei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );
			int screen_x = (int)Main.screenPosition.X + ( ( Main.screenWidth - screen_wid ) / 2 );
			int screen_y = (int)Main.screenPosition.Y + ( ( Main.screenHeight - screen_hei ) / 2 );

			return new Rectangle( screen_x, screen_y, screen_wid, screen_hei );
		}

		public static Vector2 ConvertToScreenPosition( Vector2 world_pos ) {
			var frame = UIHelpers.GetWorldFrameOfScreen();
			var screen_pos = new Vector2( frame.X, frame.Y );

			return ( world_pos - screen_pos ) * Main.GameZoomTarget;
		}

		public static Vector2 GetWorldMousePosition() {
			Rectangle zoomed_screen_frame = UIHelpers.GetWorldFrameOfScreen();
			var zoomed_screen_pos = new Vector2( zoomed_screen_frame.X, zoomed_screen_frame.Y );
			var mouse_pos = new Vector2( Main.mouseX, Main.mouseY );

			Vector2 screen_mouse_pos = UIHelpers.ConvertToScreenPosition( mouse_pos + Main.screenPosition );
			Vector2 world_mouse_pos = screen_mouse_pos + zoomed_screen_pos;

			return world_mouse_pos;
		}


		////////////////

		public static bool JustPressedKey( Keys key ) {
			return Main.inputText.IsKeyDown( key ) && !Main.oldInputText.IsKeyDown( key );
		}

		////////////////

		public static Vector2 GetHoverTipPosition( string str ) {
			Vector2 dim = Main.fontMouseText.MeasureString( str );
			Vector2 pos = new Vector2( Main.mouseX + 10f, Main.mouseY + 10f );

			if( ( pos.X + dim.X ) > Main.screenWidth ) {
				pos.X = Main.screenWidth - dim.X;
			}

			return pos;
		}


		////////////////

		public static bool MouseInRectangle( Rectangle rect ) {
			if( Main.mouseX >= rect.X && Main.mouseX < ( rect.X + rect.Width ) ) {
				if( Main.mouseY >= rect.Y && Main.mouseY < ( rect.Y + rect.Height ) ) {
					return true;
				}
			}
			return false;
		}
	}
}
