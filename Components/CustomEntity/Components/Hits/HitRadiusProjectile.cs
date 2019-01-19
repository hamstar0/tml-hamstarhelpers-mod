using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusProjectileEntityComponent : CustomEntityComponent {
		private HitRadiusProjectileEntityComponent() { }
		

		////////////////
		
		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Projectile projectile, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Projectile projectile, int damage );
	}
}
