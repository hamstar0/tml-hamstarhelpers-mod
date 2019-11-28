using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Draw {
	/// <summary>
	/// Assorted static "helper" functions pertaining to drawing to the screen. 
	/// </summary>
	public partial class DrawHelpers {
		/// <summary>
		/// Draws a bordered rectangle.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="bgColor">Fill color.</param>
		/// <param name="borderColor"></param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="borderWidth"></param>
		public static void DrawBorderedRect( SpriteBatch sb, Color bgColor, Color borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, new Color?( borderColor ), position, size, borderWidth );
		}
		/// <summary>
		/// Draws a bordered rectangle.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="bgColor">Fill color.</param>
		/// <param name="borderColor"></param>
		/// <param name="rect">Position and dimensions of rectangle.</param>
		/// <param name="borderWidth"></param>
		public static void DrawBorderedRect( SpriteBatch sb, Color bgColor, Color borderColor, Rectangle rect, int borderWidth ) {
			DrawHelpers.DrawBorderedRect( sb, bgColor, new Color?( borderColor ), rect, borderWidth );
		}

		/// <summary>
		/// Draws a rectangle with an optional border or optional fill color.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="bgColor">Fill color. Use `null` for transparent.</param>
		/// <param name="borderColor">Border color. User `null for transparent.</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="borderWidth"></param>
		public static void DrawBorderedRect( SpriteBatch sb, Color? bgColor, Color? borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			if( bgColor != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X, (int)position.Y, (int)size.X, (int)size.Y ), (Color)bgColor );
			}
			if( borderColor != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y, borderWidth, (int)size.Y ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X + (int)size.X, (int)position.Y, borderWidth, (int)size.Y ), (Color)borderColor );
			}
		}
		/// <summary>
		/// Draws a rectangle with an optional border or optional fill color.
		/// </summary>
		/// <param name="sb">The spriteBatch to draw to. Typically `Main.spriteBatch`.</param>
		/// <param name="bgColor">Fill color. Use `null` for transparent.</param>
		/// <param name="borderColor">Border color. User `null for transparent.</param>
		/// <param name="rect">Position and dimensions of rectangle.</param>
		/// <param name="borderWidth"></param>
		public static void DrawBorderedRect( SpriteBatch sb, Color? bgColor, Color? borderColor, Rectangle rect, int borderWidth ) {
			if( bgColor != null ) {
				sb.Draw( Main.magicPixel, rect, (Color)bgColor );
			}
			if( borderColor != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y - borderWidth, rect.Width + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y + rect.Height, rect.Width + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y, borderWidth, rect.Height ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X + rect.Width, rect.Y, borderWidth, rect.Height ), (Color)borderColor );
			}
		}


		////////////////

		/// <summary>
		/// Draws a "Terraria style" string, complete with pulsing and black border.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="text"></param>
		/// <param name="pos"></param>
		/// <param name="scale"></param>
		public static void DrawTerrariaString( SpriteBatch sb, string text, Vector2 pos, float scale ) {
			Color color = new Color( (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 255 );

			Utils.DrawBorderStringFourWay( sb, Main.fontMouseText, text, pos.X, pos.Y, color, Color.Black, default( Vector2 ), scale );
			//ChatManager.DrawColorCodedStringWithShadow( sb, Main.fontMouseText, text, pos, Color.White, 0f, default( Vector2 ), new Vector2( scale, scale ) );
		}
	}
}
