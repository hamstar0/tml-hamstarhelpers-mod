using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;


namespace HamstarHelpers.HudHelpers {
	public static class HudHealthBarHelpers {
		public static Color GetHealthBarColor( int hp, int max_hp, float alpha ) {
			if( hp <= 0 ) { return Color.Black; }

			float ratio = (float)hp / (float)max_hp;
			if( ratio > 1f ) { ratio = 1f; }
			ratio -= 0.1f;

			float r;
			float g;
			float b = 0f;
			float a = 255f;

			if( (double)ratio > 0.5 ) {
				r = 255f * (1f - ratio) * 2f;
				g = 255f;
			} else {
				r = 255f;
				g = 255f * ratio * 2f;
			}
			r = r * alpha * 0.95f;
			g = g * alpha * 0.95f;
			a = a * alpha * 0.95f;
			if( r < 0f ) { r = 0f; } else if( r > 255f ) { r = 255f; }
			if( g < 0f ) { g = 0f; } else if( g > 255f ) { g = 255f; }
			if( a < 0f ) { a = 0f; } else if( a > 255f ) { a = 255f; }

			return new Color( (int)((byte)r), (int)((byte)g), (int)((byte)b), (int)((byte)a) );
		}


		public static Vector2 MeasureHealthText( string text ) {
			return Main.fontItemStack.MeasureString( text ) * 0.75f;
		}


		public static void DrawHealthText( SpriteBatch sb, float x, float y, int hp, Color color ) {
			int offset_x = (int)((Math.Log10( (double)hp ) + 1d) * 3.3d);
			//byte c = (byte)MathHelper.Clamp( (alpha+0.2f) * 254f, 0f, 255f );

			Vector2 pos = new Vector2( x - offset_x, y - 4 );
			//Color color = new Color( c, c, c, c );

			sb.DrawString( Main.fontItemStack, hp.ToString(), pos, Color.Black, 0f, new Vector2( -1f, -1f ), 0.85f, SpriteEffects.None, 1f );
			sb.DrawString( Main.fontItemStack, hp.ToString(), pos, color, 0f, new Vector2( 0f, 0f ), 0.82f, SpriteEffects.None, 1f );
		}


		public static void DrawHealthBar( SpriteBatch sb, float x, float y, int hp, int max_hp, Color color, float scale = 1f ) {
			if( hp <= 0 ) { return; }

			float ratio = (float)hp / (float)max_hp;
			if( ratio > 1f ) { ratio = 1f; }
			ratio -= 0.1f;

			int ratio_large = (int)(36f * ratio);
			float offset_x = x - (18f * scale);
			float offset_y = y + 4;
			float depth = 1f;

			if( ratio_large < 3 ) { ratio_large = 3; }

			Vector2 pos;
			Rectangle? rect;

			if( ratio_large < 34 ) {
				if( ratio_large < 36 ) {
					pos = new Vector2( offset_x + (float)ratio_large * scale, offset_y );
					rect = new Rectangle?( new Rectangle( 2, 0, 2, Main.hbTexture2.Height ) );
					sb.Draw( Main.hbTexture2, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}
				if( ratio_large < 34 ) {
					pos = new Vector2( offset_x + (float)(ratio_large + 2) * scale, offset_y );
					rect = new Rectangle?( new Rectangle( ratio_large + 2, 0, 36 - ratio_large - 2, Main.hbTexture2.Height ) );
					sb.Draw( Main.hbTexture2, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}
				if( ratio_large > 2 ) {
					pos = new Vector2( offset_x, offset_y );
					rect = new Rectangle?( new Rectangle( 0, 0, ratio_large - 2, Main.hbTexture1.Height ) );
					sb.Draw( Main.hbTexture1, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				}

				pos = new Vector2( offset_x + (float)(ratio_large - 2) * scale, offset_y );
				rect = new Rectangle?( new Rectangle( 32, 0, 2, Main.hbTexture1.Height ) );
				sb.Draw( Main.hbTexture1, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
				return;
			}

			if( ratio_large < 36 ) {
				pos = new Vector2( offset_x + (float)ratio_large * scale, offset_y );
				rect = new Rectangle?( new Rectangle( ratio_large, 0, 36 - ratio_large, Main.hbTexture2.Height ) );
				sb.Draw( Main.hbTexture2, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
			}

			pos = new Vector2( offset_x, offset_y );
			rect = new Rectangle?( new Rectangle( 0, 0, ratio_large, Main.hbTexture1.Height ) );
			sb.Draw( Main.hbTexture1, pos, rect, color, 0f, new Vector2( 0f, 0f ), scale, SpriteEffects.None, depth );
		}
	}



	[System.Obsolete( "use HudHelpers.HudHealthBarHelpers", true )]
	public static class HealthBarHelpers {
		public static Color GetHealthBarColor( int hp, int max_hp, float alpha ) {
			return HudHealthBarHelpers.GetHealthBarColor( hp, max_hp, alpha );
		}
		public static Vector2 MeasureHealthText( string text ) {
			return HudHealthBarHelpers.MeasureHealthText( text );
		}
		public static void DrawHealthText( SpriteBatch sb, float x, float y, int hp, Color color ) {
			HudHealthBarHelpers.DrawHealthText( sb, x, y, hp, color );
		}
		public static void DrawHealthBar( SpriteBatch sb, float x, float y, int hp, int max_hp, Color color, float scale = 1f ) {
			HudHealthBarHelpers.DrawHealthBar( sb, x, y, hp, max_hp, color, scale );
		}
	}
}
