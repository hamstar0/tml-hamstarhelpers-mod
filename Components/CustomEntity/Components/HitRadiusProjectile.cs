using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusProjectileEntityComponent : CustomEntityComponent {
		protected HitRadiusProjectileEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }
		

		////////////////
		
		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Projectile projectile, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Projectile projectile, int damage );
	}
}
