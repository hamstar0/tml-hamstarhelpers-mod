using HamstarHelpers.Helpers.Projectiles.Attributes;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineProjectileGroups1( IList<EntityGroupMatcherDefinition<Projectile>> defs ) {
			// General

			defs.Add( new EntityGroupMatcherDefinition<Projectile>(
				"Any Explosive", null,
				new ProjectileGroupMatcher( ( proj, grp ) => {
					int _;
					return ProjectileAttributeHelpers.IsExplosive( proj.type, out _, out _ );
				} )
			) );
		}
	}
}
