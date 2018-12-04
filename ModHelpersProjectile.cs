using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
try {
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusProjectileEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				float radius = ent.GetComponentByType<HitRadiusProjectileEntityComponent>().Radius;
DebugHelpers.Print( "proj"+projectile.whoAmI+":"+projectile.Name, radius+" vs "+Vector2.Distance(ent.Core.Center, projectile.Center), 20 );

				if( Vector2.Distance(ent.Core.Center, projectile.Center) <= radius ) {
					if( !this.ApplyHits( ent, projectile ) ) {
						projectile.Kill();
						return false;
					}
				}
			}
} catch( Exception e ) { LogHelpers.Log( "? "+e.ToString() ); }

			return true;
		}

		////////////////

		private bool ApplyHits( CustomEntity ent, Projectile projectile ) {
			int dmg = projectile.damage;
			var hitComp = ent.GetComponentByType<HitRadiusProjectileEntityComponent>();

			if( !hitComp.PreHurt( ent, projectile, ref dmg ) ) {
				return true;
			}

			hitComp.PostHurt( ent, projectile, dmg );

			if( projectile.numHits > 1 ) {
				projectile.numHits--;
				return true;
			}
			return false;
		}
	}
}
