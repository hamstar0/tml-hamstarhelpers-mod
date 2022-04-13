using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Projectiles.Attributes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to projectile attributes.
	/// </summary>
	public partial class ProjectileAttributeHelpers {
		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Projectile proj ) {
			return ProjectileAttributeHelpers.GetQualifiedName( proj.type );
		}

		/// <summary>
		/// Gets the "qualified" (human readable) name of a given projectile.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int projType ) {
			try {
				return Lang.GetProjectileName( projType ).Value;
			} catch {
				return "";
			}
		}


		////////////////

		/// <summary>
		/// Gets the (vanilla) dimensions of a projectile, adjusted by specific projectile idiosyncracies or AIs.
		/// </summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public static Rectangle GetVanillaProjectileDimensions( Projectile projectile ) {
			Vector2? dimScale = null;
			int width = projectile.width, height = projectile.height;

			switch( projectile.type ) {
			case 28:
				if( projectile.aiStyle == 29 || projectile.aiStyle == 49 ) {
					width = projectile.width - 8;
					height = projectile.height - 8;
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
				height = projectile.height;
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
				height = projectile.height;
				break;
			case 688:
			case 689:
			case 690:
				width = 16;
				height = projectile.height;
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
				width = projectile.width - 20;
				height = projectile.height - 20;
				break;
			case 533:
				if( projectile.ai[0] >= 6f ) {
					width = projectile.width + 6;
					height = projectile.height + 6;
				}
				break;
			case 582:
			case 634:
			case 635:
				width = 8;
				height = 8;
				break;
			case 617:
				width = (int)(20f * projectile.scale);
				height = (int)(20f * projectile.scale);
				break;
			default:
				if( projectile.aiStyle == 29 || projectile.type == 28 || projectile.aiStyle == 49 ) {
					width = projectile.width - 8;
					height = projectile.height - 8;
				} else if( projectile.aiStyle == 18 || projectile.type == 254 ) {
					width = projectile.width - 36;
					height = projectile.height - 36;
				} else if( projectile.aiStyle == 27 ) {
					width = projectile.width - 12;
					height = projectile.height - 12;
				}
				break;
			}

			int x, y;

			if( height != projectile.height || width != projectile.width ) {
				if( dimScale.HasValue ) {
					Vector2 position = projectile.Center - new Vector2( (float)width, (float)height ) * dimScale.Value;
					x = (int)position.X;
					y = (int)position.Y;
				} else {
					x = (int)(projectile.position.X + (projectile.width >> 1) - (width >> 1));
					y = (int)(projectile.position.Y + (projectile.height >> 1) - (height >> 1));
				}
			} else {
				x = (int)projectile.position.X;
				y = (int)projectile.position.Y;
			}

			return new Rectangle( x, y, width, height );
		}


		/// <summary>
		/// Indicates if a given vanilla projectile collides with a platform.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="onlySometimes">Indicates if AI behavior can override collisions occasionally.</param>
		/// <returns></returns>
		public static bool DoesVanillaProjectileHitPlatforms( Projectile projectile, out bool onlySometimes ) {
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


		/// <summary>
		/// Indicates if a given projectile is explosive, including by how much.
		/// </summary>
		/// <param name="projectileType"></param>
		/// <param name="radius">Returns explosive radius.</param>
		/// <param name="damage">Returns explosive damage.</param>
		/// <returns></returns>
		public static bool IsExplosive( int projectileType, out int radius, out int damage ) {
			bool isExplosive;
			int inactivePos = 0;

			for( int i = 0; i < Main.projectile.Length; i++ ) {
				if( Main.projectile[i] == null || !Main.projectile[i].active ) {
					inactivePos = i;
					break;
				}
			}

			var proj = new Projectile();
			Main.projectile[inactivePos] = proj;

			try {
				proj.SetDefaults( projectileType );

				isExplosive = proj.aiStyle == 16;

				if( isExplosive ) {
					proj.position = new Vector2( 3000, 1000 );
					proj.owner = Main.myPlayer;
					proj.hostile = true;

					proj.timeLeft = 3;
					proj.VanillaAI();

					radius = (proj.width + proj.height) / 4;
					damage = proj.damage;
				} else {
					radius = 0;
					damage = 0;
				}
			} catch {
				radius = 0;
				damage = 0;
				isExplosive = false;
			}

			Main.projectile[inactivePos] = new Projectile();

			return isExplosive;
		}
	}
}
