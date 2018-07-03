using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace HamstarHelpers.Helpers.ProjectileHelpers {
	public static class ProjectileHelpers {
		public static void DrawSimple( SpriteBatch sb, Projectile proj, Vector2 pos, float rot, Color color, float scale ) {
			Texture2D tex = Main.projectileTexture[proj.type];
			int tex_height = tex.Height / Main.projFrames[proj.type];

			Vector2 newpos = UIHelpers.UIHelpers.ConvertToScreenPosition( pos );
			var rect = new Rectangle( 0, proj.frame * tex_height, tex.Width, tex_height );
			var origin = new Vector2( tex.Width * 0.5f, (float)tex_height * 0.5f );
			SpriteEffects dir = SpriteEffects.None;

			if( proj.spriteDirection == -1 ) {
				dir = SpriteEffects.FlipHorizontally;
			}
			if( proj.type == 681 && (double)proj.velocity.X > 0.0 ) {
				dir ^= SpriteEffects.FlipHorizontally;
			}

			sb.Draw( tex, newpos, rect, color, rot, origin, scale, dir, 1 );
		}

		/*public static void DrawSimple( SpriteBatch sb, Projectile proj, Color color, Vector2 pos, float rot, float scale ) {
			Texture2D tex = Main.projectileTexture[proj.type];
			int y_origin_offset = 0;
			int x_offset = 0;
			float tex_x_offset = (float)((double)(tex.Width - proj.width) * 0.5f + (double)proj.width * 0.5f);
			//Color color = Lighting.GetColor( (int)((double)proj.position.X + (double)proj.width * 0.5) / 16, (int)(((double)proj.position.Y + (double)proj.height * 0.5) / 16.0) );

			//if( proj.hide && !ProjectileID.Sets.DontAttachHideToAlpha[proj.type] ) {
			//	Vector2 mounted_center = Main.player[proj.owner].MountedCenter;
			//	color = Lighting.GetColor( (int)mounted_center.X / 16, (int)((double)mounted_center.Y / 16.0) );
			//}
			//if( proj.type == 14 ) {
			//	color = Color.White;
			//}

			if( proj.type == 175 ) {
				y_origin_offset = 10;
			}
			if( proj.type == 392 ) {
				y_origin_offset = -2;
			}
			if( proj.type == 499 ) {
				y_origin_offset = 12;
			}
			if( proj.bobber ) {
				y_origin_offset = 8;
			}
			if( proj.type == 519 ) {
				y_origin_offset = 6;
				x_offset -= 6;
			}
			if( proj.type == 520 ) {
				y_origin_offset = 12;
			}
			if( proj.type == 492 ) {
				x_offset -= 4;
				y_origin_offset += 5;
			}
			if( proj.type == 498 ) {
				y_origin_offset = 6;
			}
			if( proj.type == 489 ) {
				y_origin_offset = -2;
			}
			if( proj.type == 486 ) {
				y_origin_offset = -6;
			}
			if( proj.type == 525 ) {
				y_origin_offset = 5;
			}
			if( proj.type == 488 ) {
				x_offset -= 8;
			}
			if( proj.type == 373 ) {
				x_offset = -10;
				y_origin_offset = 6;
			}
			if( proj.type == 375 ) {
				x_offset = -11;
				y_origin_offset = 12;
			}
			if( proj.type == 423 ) {
				x_offset = -5;
			}
			if( proj.type == 346 ) {
				y_origin_offset = 4;
			}
			if( proj.type == 331 ) {
				x_offset = -4;
			}
			if( proj.type == 254 ) {
				y_origin_offset = 3;
			}
			if( proj.type == 273 ) {
				x_offset = 2;
			}
			if( proj.type == 335 ) {
				y_origin_offset = 6;
			}
			if( proj.type == 162 ) {
				y_origin_offset = 1;
				x_offset = 1;
			}
			if( proj.type == 377 ) {
				y_origin_offset = -6;
			}
			if( proj.type == 353 ) {
				y_origin_offset = 36;
				x_offset = -12;
			}
			if( proj.type == 324 ) {
				y_origin_offset = 22;
				x_offset = -6;
			}
			if( proj.type == 266 ) {
				y_origin_offset = 10;
				x_offset = -10;
			}
			if( proj.type == 319 ) {
				y_origin_offset = 10;
				x_offset = -12;
			}
			if( proj.type == 315 ) {
				y_origin_offset = -13;
				x_offset = -6;
			}
			if( proj.type == 313 && proj.height != 54 ) {
				x_offset = -12;
				y_origin_offset = 20;
			}
			if( proj.type == 314 ) {
				x_offset = -8;
				y_origin_offset = 0;
			}
			if( proj.type == 269 ) {
				y_origin_offset = 18;
				x_offset = -14;
			}
			if( proj.type == 268 ) {
				y_origin_offset = 22;
				x_offset = -2;
			}
			if( proj.type == 18 ) {
				y_origin_offset = 3;
				x_offset = 3;
			}
			if( proj.type == 16 ) {
				y_origin_offset = 6;
			}
			if( proj.type == 17 || proj.type == 31 ) {
				y_origin_offset = 2;
			}
			if( proj.type == 25 || proj.type == 26 || (proj.type == 35 || proj.type == 63) || proj.type == 154 ) {
				y_origin_offset = 6;
				x_offset -= 6;
			}
			if( proj.type == 28 || proj.type == 37 || proj.type == 75 ) {
				y_origin_offset = 8;
			}
			if( proj.type == 29 || proj.type == 470 || proj.type == 637 ) {
				y_origin_offset = 11;
			}
			if( proj.type == 43 ) {
				y_origin_offset = 4;
			}
			if( proj.type == 208 ) {
				y_origin_offset = 2;
				x_offset -= 12;
			}
			if( proj.type == 209 ) {
				y_origin_offset = 4;
				x_offset -= 8;
			}
			if( proj.type == 210 ) {
				y_origin_offset = 2;
				x_offset -= 22;
			}
			if( proj.type == 251 ) {
				y_origin_offset = 18;
				x_offset -= 10;
			}
			if( proj.type == 163 || proj.type == 310 ) {
				y_origin_offset = 10;
			}
			if( proj.type == 69 || proj.type == 70 ) {
				y_origin_offset = 4;
				x_offset = 4;
			}
			if( proj.type == 50 || proj.type == 53 || proj.type == 515 ) {
				x_offset = -8;
			}
			if( proj.type == 473 ) {
				x_offset = -6;
				y_origin_offset = 2;
			}
			if( proj.type == 72 || proj.type == 86 || proj.type == 87 ) {
				x_offset = -16;
				y_origin_offset = 8;
			}
			if( proj.type == 74 ) {
				x_offset = -6;
			}
			if( proj.type == 99 ) {
				y_origin_offset = 1;
			}
			if( proj.type == 655 ) {
				y_origin_offset = 1;
			}
			if( proj.type == 111 ) {
				y_origin_offset = 18;
				x_offset = -16;
			}
			if( proj.type == 334 ) {
				x_offset = -18;
				y_origin_offset = 8;
			}
			if( proj.type == 200 ) {
				y_origin_offset = 12;
				x_offset = -12;
			}
			if( proj.type == 211 ) {
				y_origin_offset = 14;
				x_offset = 0;
			}
			if( proj.type == 236 ) {
				y_origin_offset = 30;
				x_offset = -14;
			}
			if( proj.type >= 191 && proj.type <= 194 ) {
				y_origin_offset = 26;
				x_offset = proj.direction != 1 ? -22 : -10;
			}
			if( proj.type >= 390 && proj.type <= 392 ) {
				x_offset = 4 * proj.direction;
			}
			if( proj.type == 112 ) {
				y_origin_offset = 12;
			}
			if( proj.type == 517 || proj.type == 681 ) {
				y_origin_offset = 6;
			}
			if( proj.type == 516 ) {
				y_origin_offset = 6;
			}
			if( proj.type == 255 ) {
				y_origin_offset = 8;
			}
			if( proj.type == 155 ) {
				y_origin_offset = 3;
				x_offset = 3;
			}
			if( proj.type == 397 ) {
				--tex_x_offset;
				y_origin_offset = -2;
				x_offset = -2;
			}
			if( proj.type == 398 ) {
				y_origin_offset = 8;
			}

			SpriteEffects dir = SpriteEffects.None;
			if( proj.spriteDirection == -1 ) {
				dir = SpriteEffects.FlipHorizontally;
			}
			if( proj.type == 681 && (double)proj.velocity.X > 0.0 ) {
				dir ^= SpriteEffects.FlipHorizontally;
			}

			float x = pos.X + tex_x_offset + (float)x_offset;
			float y = pos.Y + (float)(proj.height / 2) + proj.gfxOffY;
			Vector2 newpos = UIHelpers.UIHelpers.ConvertToScreenPosition( new Vector2( x, y ) );
			Vector2 origin = new Vector2( tex_x_offset, (float)(proj.height / 2 + y_origin_offset) );

			sb.Draw( tex, newpos, new Rectangle( 0, 0, tex.Width, tex.Height ), proj.GetAlpha( color ), rot, origin, scale, dir, 1.0f );
		}*/
	}
}
