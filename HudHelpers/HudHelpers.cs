using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.HudHelpers {
	public static class HudHelpers {
		public static void GetTopHeartPosition( Player player, ref int x, ref int y ) {
			x = Main.screenWidth - 66;
			y = 60;

			if( player.statLifeMax2 < 400 && (player.statLifeMax2 / 20) % 10 != 0 ) {
				x -= (10 - ((player.statLifeMax2 / 20) % 10)) * 26;
			}
			if( player.statLifeMax2 / 20 <= 10 ) {
				y -= 32;
			}
		}


		public static void DrawBorderedRect( SpriteBatch sb, Color color, Color border_color, Vector2 position, Vector2 size, int border_width ) {
			sb.Draw( Main.magicPixel, new Rectangle( (int)position.X, (int)position.Y, (int)size.X, (int)size.Y ), color );
			sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y - border_width, (int)size.X + border_width * 2, border_width ), border_color );
			sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y + (int)size.Y, (int)size.X + border_width * 2, border_width ), border_color );
			sb.Draw( Main.magicPixel, new Rectangle( (int)position.X - border_width, (int)position.Y, (int)border_width, (int)size.Y ), border_color );
			sb.Draw( Main.magicPixel, new Rectangle( (int)position.X + (int)size.X, (int)position.Y, (int)border_width, (int)size.Y ), border_color );
		}   // Blatantly lifted from Jopo's mod
	}
}
