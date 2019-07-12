using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectiles.
	/// </summary>
	public partial class ProjectileHelpers {
		/// <summary>
		/// Draws a given projectile
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="proj"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="color"></param>
		/// <param name="scale"></param>
		/// <param name="applyZoom">Whether to convert to current zoom amount of the screen.</param>
		public static void DrawSimple( SpriteBatch sb, Projectile proj, Vector2 pos, float rot, Color color, float scale,
					bool applyZoom = false ) {
			Texture2D tex = Main.projectileTexture[proj.type];
			int texHeight = tex.Height / Main.projFrames[proj.type];

			var rect = new Rectangle( 0, proj.frame * texHeight, tex.Width, texHeight );
			var origin = new Vector2( tex.Width * 0.5f, (float)texHeight * 0.5f );
			SpriteEffects dir = SpriteEffects.None;

			if( proj.spriteDirection == -1 ) {
				dir = SpriteEffects.FlipHorizontally;
			}
			if( proj.type == 681 && (double)proj.velocity.X > 0.0 ) {
				dir ^= SpriteEffects.FlipHorizontally;
			}

			Vector2 newpos;
			if( applyZoom ) {
				newpos = UIHelpers.ConvertToScreenPosition( pos );
			} else {
				newpos = pos - Main.screenPosition;
			}

			sb.Draw( tex, newpos, rect, color, rot, origin, scale, dir, 1 );
		}

		/*public static void DrawSimple( SpriteBatch sb, Projectile proj, Color color, Vector2 pos, float rot, float scale ) {
			Texture2D tex = Main.projectileTexture[proj.type];
			int yOriginOffset = 0;
			int xOffset = 0;
			float texXOffset = (float)((double)(tex.Width - proj.width) * 0.5f + (double)proj.width * 0.5f);
			//Color color = Lighting.GetColor( (int)((double)proj.position.X + (double)proj.width * 0.5) / 16, (int)(((double)proj.position.Y + (double)proj.height * 0.5) / 16.0) );

			//if( proj.hide && !ProjectileID.Sets.DontAttachHideToAlpha[proj.type] ) {
			//	Vector2 mountedCenter = Main.player[proj.owner].MountedCenter;
			//	color = Lighting.GetColor( (int)mountedCenter.X / 16, (int)((double)mountedCenter.Y / 16.0) );
			//}
			//if( proj.type == 14 ) {
			//	color = Color.White;
			//}

			if( proj.type == 175 ) {
				yOriginOffset = 10;
			}
			if( proj.type == 392 ) {
				yOriginOffset = -2;
			}
			if( proj.type == 499 ) {
				yOriginOffset = 12;
			}
			if( proj.bobber ) {
				yOriginOffset = 8;
			}
			if( proj.type == 519 ) {
				yOriginOffset = 6;
				xOffset -= 6;
			}
			if( proj.type == 520 ) {
				yOriginOffset = 12;
			}
			if( proj.type == 492 ) {
				xOffset -= 4;
				yOriginOffset += 5;
			}
			if( proj.type == 498 ) {
				yOriginOffset = 6;
			}
			if( proj.type == 489 ) {
				yOriginOffset = -2;
			}
			if( proj.type == 486 ) {
				yOriginOffset = -6;
			}
			if( proj.type == 525 ) {
				yOriginOffset = 5;
			}
			if( proj.type == 488 ) {
				xOffset -= 8;
			}
			if( proj.type == 373 ) {
				xOffset = -10;
				yOriginOffset = 6;
			}
			if( proj.type == 375 ) {
				xOffset = -11;
				yOriginOffset = 12;
			}
			if( proj.type == 423 ) {
				xOffset = -5;
			}
			if( proj.type == 346 ) {
				yOriginOffset = 4;
			}
			if( proj.type == 331 ) {
				xOffset = -4;
			}
			if( proj.type == 254 ) {
				yOriginOffset = 3;
			}
			if( proj.type == 273 ) {
				xOffset = 2;
			}
			if( proj.type == 335 ) {
				yOriginOffset = 6;
			}
			if( proj.type == 162 ) {
				yOriginOffset = 1;
				xOffset = 1;
			}
			if( proj.type == 377 ) {
				yOriginOffset = -6;
			}
			if( proj.type == 353 ) {
				yOriginOffset = 36;
				xOffset = -12;
			}
			if( proj.type == 324 ) {
				yOriginOffset = 22;
				xOffset = -6;
			}
			if( proj.type == 266 ) {
				yOriginOffset = 10;
				xOffset = -10;
			}
			if( proj.type == 319 ) {
				yOriginOffset = 10;
				xOffset = -12;
			}
			if( proj.type == 315 ) {
				yOriginOffset = -13;
				xOffset = -6;
			}
			if( proj.type == 313 && proj.height != 54 ) {
				xOffset = -12;
				yOriginOffset = 20;
			}
			if( proj.type == 314 ) {
				xOffset = -8;
				yOriginOffset = 0;
			}
			if( proj.type == 269 ) {
				yOriginOffset = 18;
				xOffset = -14;
			}
			if( proj.type == 268 ) {
				yOriginOffset = 22;
				xOffset = -2;
			}
			if( proj.type == 18 ) {
				yOriginOffset = 3;
				xOffset = 3;
			}
			if( proj.type == 16 ) {
				yOriginOffset = 6;
			}
			if( proj.type == 17 || proj.type == 31 ) {
				yOriginOffset = 2;
			}
			if( proj.type == 25 || proj.type == 26 || (proj.type == 35 || proj.type == 63) || proj.type == 154 ) {
				yOriginOffset = 6;
				xOffset -= 6;
			}
			if( proj.type == 28 || proj.type == 37 || proj.type == 75 ) {
				yOriginOffset = 8;
			}
			if( proj.type == 29 || proj.type == 470 || proj.type == 637 ) {
				yOriginOffset = 11;
			}
			if( proj.type == 43 ) {
				yOriginOffset = 4;
			}
			if( proj.type == 208 ) {
				yOriginOffset = 2;
				xOffset -= 12;
			}
			if( proj.type == 209 ) {
				yOriginOffset = 4;
				xOffset -= 8;
			}
			if( proj.type == 210 ) {
				yOriginOffset = 2;
				xOffset -= 22;
			}
			if( proj.type == 251 ) {
				yOriginOffset = 18;
				xOffset -= 10;
			}
			if( proj.type == 163 || proj.type == 310 ) {
				yOriginOffset = 10;
			}
			if( proj.type == 69 || proj.type == 70 ) {
				yOriginOffset = 4;
				xOffset = 4;
			}
			if( proj.type == 50 || proj.type == 53 || proj.type == 515 ) {
				xOffset = -8;
			}
			if( proj.type == 473 ) {
				xOffset = -6;
				yOriginOffset = 2;
			}
			if( proj.type == 72 || proj.type == 86 || proj.type == 87 ) {
				xOffset = -16;
				yOriginOffset = 8;
			}
			if( proj.type == 74 ) {
				xOffset = -6;
			}
			if( proj.type == 99 ) {
				yOriginOffset = 1;
			}
			if( proj.type == 655 ) {
				yOriginOffset = 1;
			}
			if( proj.type == 111 ) {
				yOriginOffset = 18;
				xOffset = -16;
			}
			if( proj.type == 334 ) {
				xOffset = -18;
				yOriginOffset = 8;
			}
			if( proj.type == 200 ) {
				yOriginOffset = 12;
				xOffset = -12;
			}
			if( proj.type == 211 ) {
				yOriginOffset = 14;
				xOffset = 0;
			}
			if( proj.type == 236 ) {
				yOriginOffset = 30;
				xOffset = -14;
			}
			if( proj.type >= 191 && proj.type <= 194 ) {
				yOriginOffset = 26;
				xOffset = proj.direction != 1 ? -22 : -10;
			}
			if( proj.type >= 390 && proj.type <= 392 ) {
				xOffset = 4 * proj.direction;
			}
			if( proj.type == 112 ) {
				yOriginOffset = 12;
			}
			if( proj.type == 517 || proj.type == 681 ) {
				yOriginOffset = 6;
			}
			if( proj.type == 516 ) {
				yOriginOffset = 6;
			}
			if( proj.type == 255 ) {
				yOriginOffset = 8;
			}
			if( proj.type == 155 ) {
				yOriginOffset = 3;
				xOffset = 3;
			}
			if( proj.type == 397 ) {
				--texXOffset;
				yOriginOffset = -2;
				xOffset = -2;
			}
			if( proj.type == 398 ) {
				yOriginOffset = 8;
			}

			SpriteEffects dir = SpriteEffects.None;
			if( proj.spriteDirection == -1 ) {
				dir = SpriteEffects.FlipHorizontally;
			}
			if( proj.type == 681 && (double)proj.velocity.X > 0.0 ) {
				dir ^= SpriteEffects.FlipHorizontally;
			}

			float x = pos.X + texXOffset + (float)xOffset;
			float y = pos.Y + (float)(proj.height / 2) + proj.gfxOffY;
			Vector2 newpos = UIHelpers.UIHelpers.ConvertToScreenPosition( new Vector2( x, y ) );
			Vector2 origin = new Vector2( texXOffset, (float)(proj.height / 2 + yOriginOffset) );

			sb.Draw( tex, newpos, new Rectangle( 0, 0, tex.Width, tex.Height ), proj.GetAlpha( color ), rot, origin, scale, dir, 1.0f );
		}*/
	}
}
