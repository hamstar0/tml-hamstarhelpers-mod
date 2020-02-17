using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UI {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).
	/// </summary>
	public class UIHelpers {
		/// <summary></summary>
		public static readonly int MinScreenWidth = 800;
		/// <summary></summary>
		public static readonly int MinScreenHeight = 640;
		/// <summary></summary>
		public static readonly int MaxScreenWidth = 1920;
		/// <summary></summary>
		public static readonly int MaxScreenHeight = 1080;
		/// <summary></summary>
		public static readonly int MinScreenWidthTiles = 80;
		/// <summary></summary>
		public static readonly int MinScreenHeightTiles = 40;
		/// <summary></summary>
		public static readonly int MaxScreenWidthTiles = 124;   //1920 + 64
		/// <summary></summary>
		public static readonly int MaxScreenHeightTiles = 70;   //1080 + 40



		////////////////

		/// <summary>
		/// Gets the current screen size, factoring zoom.
		/// </summary>
		/// <returns></returns>
		public static Tuple<int, int> GetScreenSize() {
			int screenWid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screenHei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );

			return Tuple.Create( screenWid, screenHei );
		}

		/// <summary>
		/// Gets the screen dimensions (factoring game zoom) within world space.
		/// </summary>
		/// <returns></returns>
		public static Rectangle GetWorldFrameOfScreen() {
			float screenWid = (float)Main.screenWidth / Main.GameZoomTarget;
			float screenHei = (float)Main.screenHeight / Main.GameZoomTarget;
			int screenX = (int)Main.screenPosition.X + (int)(((float)Main.screenWidth - screenWid) * 0.5f);
			int screenY = (int)Main.screenPosition.Y + (int)(((float)Main.screenHeight - screenHei) * 0.5f);

			return new Rectangle( screenX, screenY, (int)screenWid, (int)screenHei );
		}

		/// <summary>
		/// Converts a given screen position to world coordinates (factoring zoom).
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static Vector2 ConvertToScreenPosition( Vector2 worldPos ) {
			var frame = UIHelpers.GetWorldFrameOfScreen();
			var screenPos = new Vector2( frame.X, frame.Y );

			return (worldPos - screenPos) * Main.GameZoomTarget;
		}

		/// <summary>
		/// Converts the mouse to world coordinates (factoring screen zoom).
		/// </summary>
		/// <returns></returns>
		public static Vector2 GetWorldMousePosition() {
			Rectangle zoomedScreenFrame = UIHelpers.GetWorldFrameOfScreen();
			var zoomedScreenPos = new Vector2( zoomedScreenFrame.X, zoomedScreenFrame.Y );
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			Vector2 screenMousePos = UIHelpers.ConvertToScreenPosition( mousePos + Main.screenPosition );
			Vector2 worldMousePos = screenMousePos + zoomedScreenPos;

			return worldMousePos;
		}


		////////////////

		/// <summary>
		/// Reports whether a given keyboard key was just pressed. Used primarily for text field inputs.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool JustPressedKey( Keys key ) {
			return Main.inputText.IsKeyDown( key ) && !Main.oldInputText.IsKeyDown( key );
		}

		////////////////

		/// <summary>
		/// Gets the position to use to display a given string of text near the mouse position so as to fit on screen.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Vector2 GetHoverTipPosition( string str ) {
			Vector2 dim = Main.fontMouseText.MeasureString( str );
			Vector2 pos = new Vector2( Main.mouseX + 10f, Main.mouseY + 10f );

			if( ( pos.X + dim.X ) > Main.screenWidth ) {
				pos.X = Main.screenWidth - dim.X;
			}

			return pos;
		}


		////////////////

		/// <summary>
		/// Indicates if the mouse is within a given screen rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
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
