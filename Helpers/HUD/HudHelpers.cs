using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.HUD {
	/** <summary>Assorted static "helper" functions pertaining to general HUD.</summary> */
	public static class HUDHelpers {
		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			HUDHelpers.DrawBorderedRect( sb, color, new Color?(borderColor), position, size, borderWidth );
		}
		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color borderColor, Rectangle rect, int borderWidth ) {
			HUDHelpers.DrawBorderedRect( sb, color, new Color?(borderColor), rect, borderWidth );
		}

		public static void DrawBorderedRect( SpriteBatch sb, Color? color, Color? borderColor, Vector2 position, Vector2 size, int borderWidth ) {
			if( color != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X, (int)position.Y, (int)size.X, (int)size.Y ), (Color)color );
			}
			if( borderColor != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - borderWidth, (int)position.Y, borderWidth, (int)size.Y ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X + (int)size.X, (int)position.Y, borderWidth, (int)size.Y ), (Color)borderColor );
			}
		}
		public static void DrawBorderedRect( SpriteBatch sb, Color? color, Color? borderColor, Rectangle rect, int borderWidth ) {
			if( color != null ) {
				sb.Draw( Main.magicPixel, rect, (Color)color );
			}
			if( borderColor != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y - borderWidth, rect.Width + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y + rect.Height, rect.Width + borderWidth * 2, borderWidth ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - borderWidth, rect.Y, borderWidth, rect.Height ), (Color)borderColor );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X + rect.Width, rect.Y, borderWidth, rect.Height ), (Color)borderColor );
			}
		}


		////////////////

		public static void DrawTerrariaString( string text, Vector2 pos, float scale ) {
			Color color = new Color( (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 255 );

			Utils.DrawBorderStringFourWay( Main.spriteBatch, Main.fontMouseText, text, pos.X, pos.Y, color, Color.Black, default( Vector2 ), scale );
			//ChatManager.DrawColorCodedStringWithShadow( Main.spriteBatch, Main.fontMouseText, text, pos, Color.White, 0f, default( Vector2 ), new Vector2( scale, scale ) );
		}
	}
}
