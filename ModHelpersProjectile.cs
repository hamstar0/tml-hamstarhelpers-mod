using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.DataStore;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+ projectile.whoAmI+":"+ projectile.type+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusProjectileEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				var hitComp = ent.GetComponentByType<HitRadiusProjectileEntityComponent>();
				float radius = hitComp.GetRadius( ent );

				if( Vector2.Distance(ent.Core.Center, projectile.Center) <= radius ) {
					if( !this.ApplyHits( ent, projectile ) ) {
						projectile.Kill();
						return false;
					}
				}
			}
			
DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+ projectile.whoAmI+":"+ projectile.type+"_B", 1 );
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
