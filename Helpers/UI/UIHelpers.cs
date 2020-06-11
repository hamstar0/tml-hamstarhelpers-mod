using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;


namespace HamstarHelpers.Helpers.UI {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the in-game UI (positions, interactions, etc.).
	/// </summary>
	public partial class UIHelpers {
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
