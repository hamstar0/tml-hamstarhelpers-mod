using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;


namespace HamstarHelpers.UIHelpers {
	public static partial class UIHelpers {
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


		////////////////

		public static bool JustPressedKey( Keys key ) {
			return Main.inputText.IsKeyDown( key ) && !Main.oldInputText.IsKeyDown( key );
		}

		////////////////

		public static Vector2 GetHoverTipPosition( string str ) {
			Vector2 dim = Main.fontMouseText.MeasureString( str );
			Vector2 pos = new Vector2( Main.mouseX + 40f, Main.mouseY + 16f );

			if( (pos.X + dim.X) > Main.screenWidth ) {
				pos.X = Main.screenWidth - dim.X;
			}

			return pos;
		}


		////////////////

		public static bool MouseInRectangle( Rectangle rect ) {
			if( Main.mouseX >= rect.X && Main.mouseX < (rect.X + rect.Width) ) {
				if( Main.mouseY >= rect.Y && Main.mouseY < ( rect.Y + rect.Height ) ) {
					return true;
				}
			}
			return false;
		}
	}
}
