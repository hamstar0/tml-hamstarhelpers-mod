using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusProjectileEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				float radius = ent.GetComponentByType<HitRadiusProjectileEntityComponent>().Radius;

				if( Vector2.Distance(ent.Core.Center, projectile.Center) <= radius ) {
					if( !this.ApplyHits( ent, projectile ) ) {
						projectile.Kill();
						return false;
					}
				}
			}

			return true;
		}

		////////////////

		private bool ApplyHits( CustomEntity ent, Projectile projectile ) {
			int dmg = projectile.damage;
			var atkComp = ent.GetComponentByType<HitRadiusProjectileEntityComponent>();

			if( !atkComp.PreHurt( ent, projectile, ref dmg ) ) {
				return true;
			}

			atkComp.PostHurt( ent, projectile, dmg );

			if( projectile.numHits > 1 ) {
				projectile.numHits--;
				return true;
			}
			return false;
		}
	}
}
