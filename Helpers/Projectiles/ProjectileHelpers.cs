using Terraria;


namespace HamstarHelpers.Helpers.Projectiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to players relative to projectiles.
	/// </summary>
	public partial class ProjectileHelpers {
		/// <summary>
		/// Applies projectile "hits", as if to make the effect of impacting something (including consuming penetrations).
		/// </summary>
		/// <param name="proj"></param>
		public static void Hit( Projectile proj ) {
			if( proj.penetrate <= 0 ) {
				proj.Kill();
			} else {
				proj.penetrate--;
				proj.netUpdate = true;
			}
		}


		public static Rectangle GetProjectileDimensions( Projectile proj ) {
			Vector2? dimScale = null;
			int width = proj.width, height = proj.height;

			switch( proj.type ) {
			case 28:
				if( proj.aiStyle == 29 || proj.aiStyle == 49 ) {
					width = proj.width - 8;
					height = proj.height - 8;
				}
				break;
			case 3:
			case 250:
			case 267:
			case 297:
			case 323:
			case 711:
				width = 6;
				height = 6;
				break;
			case 308:
				width = 26;
				height = proj.height;
				break;
			case 663:
			case 665:
			case 667:
			case 677:
			case 678:
			case 679:
			case 691:
			case 692:
			case 693:
				width = 16;
				height = proj.height;
				break;
			case 688:
			case 689:
			case 690:
				width = 16;
				height = proj.height;
				dimScale = new Vector2?( new Vector2( 0.5f, 1f ) );
				break;
			case 669:
			case 706:
				width = 10;
				height = 10;
				break;
			case 261:
			case 277:
				width = 26;
				height = 26;
				break;
			case 481:
			case 491:
			case 106:
			case 262:
			case 271:
			case 270:
			case 272:
			case 273:
			case 274:
			case 280:
			case 288:
			case 301:
			case 320:
			case 333:
			case 335:
			case 343:
			case 344:
			case 497:
			case 496:
			case 6:
			case 19:
			case 113:
			case 52:
			case 520:
			case 523:
			case 585:
			case 598:
			case 599:
			case 636:
				width = 10;
				height = 10;
				break;
			case 514:
			case 248:
			case 247:
			case 507:
			case 508:
			case 662:
			case 680:
			case 685:
			case 254:
				width = 4;
				height = 4;
				break;
			case 182:
			case 190:
			case 33:
			case 229:
			case 237:
			case 243:
				width = proj.width - 20;
				height = proj.height - 20;
				break;
			case 533:
				if( proj.ai[0] >= 6f ) {
					width = proj.width + 6;
					height = proj.height + 6;
				}
				break;
			case 582:
			case 634:
			case 635:
				width = 8;
				height = 8;
				break;
			case 617:
				width = (int)(20f * proj.scale);
				height = (int)(20f * proj.scale);
				break;
			default:
				if( proj.aiStyle == 29 || proj.type == 28 || proj.aiStyle == 49 ) {
					width = proj.width - 8;
					height = proj.height - 8;
				} else if( proj.aiStyle == 18 || proj.type == 254 ) {
					width = proj.width - 36;
					height = proj.height - 36;
				} else if( proj.aiStyle == 27 ) {
					width = proj.width - 12;
					height = proj.height - 12;
				}
				break;
			}

			int x, y;

			if( height != proj.height || width != proj.width ) {
				if( dimScale.HasValue ) {
					Vector2 position = proj.Center - new Vector2( (float)width, (float)height ) * dimScale.Value;
					x = (int)position.X;
					y = (int)position.Y;
				} else {
					x = (int)(proj.position.X + (proj.width >> 1) - (width >> 1));
					y = (int)(proj.position.Y + (proj.height >> 1) - (height >> 1));
				}
			} else {
				x = (int)proj.position.X;
				y = (int)proj.position.Y;
			}

			return new Rectangle( x, y, width, height );
		}


		public static bool VanillaProjectileRespectsPlatforms( Projectile projectile, out bool onlySometimes ) {
			if( Main.projPet[projectile.type] ) {
				onlySometimes = true;
				return true;
			}
			if( projectile.aiStyle == 53 ) {
				onlySometimes = false;
				return true;
			}
			switch( projectile.type ) {
			case 9:
			case 12:
			case 15:
			case 13:
			case 24:
			case 31:
			case 39:
			case 40:
			case 663:
			case 665:
			case 667:
			case 677:
			case 678:
			case 679:
			case 688:
			case 689:
			case 690:
			case 691:
			case 692:
			case 693:
				onlySometimes = false;
				return true;
			}

			onlySometimes = false;
			return false;
		}
	}
}
