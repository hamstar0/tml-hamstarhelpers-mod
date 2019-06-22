using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UI {
	/** <summary>Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).</summary> */
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
			int screenWid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screenHei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );

			return Tuple.Create( screenWid, screenHei );
		}

		public static Rectangle GetWorldFrameOfScreen() {
			int screenWid = (int)( (float)Main.screenWidth / Main.GameZoomTarget );
			int screenHei = (int)( (float)Main.screenHeight / Main.GameZoomTarget );
			int screenX = (int)Main.screenPosition.X + ( ( Main.screenWidth - screenWid ) / 2 );
			int screenY = (int)Main.screenPosition.Y + ( ( Main.screenHeight - screenHei ) / 2 );

			return new Rectangle( screenX, screenY, screenWid, screenHei );
		}

		public static Vector2 ConvertToScreenPosition( Vector2 worldPos ) {
			var frame = UIHelpers.GetWorldFrameOfScreen();
			var screenPos = new Vector2( frame.X, frame.Y );

			return ( worldPos - screenPos ) * Main.GameZoomTarget;
		}

		public static Vector2 GetWorldMousePosition() {
			Rectangle zoomedScreenFrame = UIHelpers.GetWorldFrameOfScreen();
			var zoomedScreenPos = new Vector2( zoomedScreenFrame.X, zoomedScreenFrame.Y );
			var mousePos = new Vector2( Main.mouseX, Main.mouseY );

			Vector2 screenMousePos = UIHelpers.ConvertToScreenPosition( mousePos + Main.screenPosition );
			Vector2 worldMousePos = screenMousePos + zoomedScreenPos;

			return worldMousePos;
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
