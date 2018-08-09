using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.HudHelpers {
	public static class HudHelpers {
		public static void GetTopHeartPosition( Player player, ref int x, ref int y ) {
			x = Main.screenWidth - 66;
			y = 59;

			int hp = player.statLifeMax2 <= 400 ? player.statLifeMax2 : (player.statLifeMax2 - 400) * 4;
			if( hp > 500 ) { hp = 500; }
			int hearts = hp / 20;

			if( hearts % 10 != 0 ) {
				x -= (10 - (hearts % 10)) * 26;
			}
			if( hearts <= 10 ) {
				y -= 27;
			}
		}


		////////////////

		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color border_color, Vector2 position, Vector2 size, int border_width ) {
			HudHelpers.DrawBorderedRect( sb, color, new Color?(border_color), position, size, border_width );
		}
		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color border_color, Rectangle rect, int border_width ) {
			HudHelpers.DrawBorderedRect( sb, color, new Color?(border_color), rect, border_width );
		}

		public static void DrawBorderedRect( SpriteBatch sb, Color? color, Color? border_color, Vector2 position, Vector2 size, int border_width ) {
			if( color != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X, (int)position.Y, (int)size.X, (int)size.Y ), (Color)color );
			}
			if( border_color != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y - border_width, (int)size.X + border_width * 2, border_width ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y + (int)size.Y, (int)size.X + border_width * 2, border_width ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y, border_width, (int)size.Y ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( (int)position.X + (int)size.X, (int)position.Y, border_width, (int)size.Y ), (Color)border_color );
			}
		}
		public static void DrawBorderedRect( SpriteBatch sb, Color? color, Color? border_color, Rectangle rect, int border_width ) {
			if( color != null ) {
				sb.Draw( Main.magicPixel, rect, (Color)color );
			}
			if( border_color != null ) {
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - border_width, rect.Y - border_width, rect.Width + border_width * 2, border_width ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - border_width, rect.Y + rect.Height, rect.Width + border_width * 2, border_width ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X - border_width, rect.Y, border_width, rect.Height ), (Color)border_color );
				sb.Draw( Main.magicPixel, new Rectangle( rect.X + rect.Width, rect.Y, border_width, rect.Height ), (Color)border_color );
			}
		}


		////////////////

		public static void DrawGlowingString( string text, Vector2 pos, float scale ) {
			Color color = new Color( (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 255 );

			Utils.DrawBorderStringFourWay( Main.spriteBatch, Main.fontMouseText, text, pos.X, pos.Y, color, Color.Black, default( Vector2 ), scale );
			//ChatManager.DrawColorCodedStringWithShadow( Main.spriteBatch, Main.fontMouseText, text, pos, Color.White, 0f, default( Vector2 ), new Vector2( scale, scale ) );
		}
	}
}
