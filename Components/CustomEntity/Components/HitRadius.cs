using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusEntityComponent : CustomEntityComponent {
		protected class HitRadiusEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : HitRadiusEntityComponent {
			public float Radius;

			public HitRadiusEntityComponentFactory( float radius ) {
				this.Radius = radius;
			}

			protected override void InitializeComponent( T data ) {
				data.Radius = this.Radius;
			}
		}



		////////////////

		public static HitRadiusEntityComponent CreateAttackableEntityComponent( float radius ) {
			var factory = new HitRadiusEntityComponentFactory<HitRadiusEntityComponent>( radius );
			return factory.Create();
		}


		////////////////
		
		public bool IsImmune = false;
		public float Radius;



		////////////////

		protected HitRadiusEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		public virtual bool PreHurtByProjectile( CustomEntity ent, Projectile projectile, ref int damage ) {
			return true;
		}
		public abstract void PostHurtByProjectile( CustomEntity ent, Projectile projectile, int damage );


		public virtual bool PreHurtByNPC( CustomEntity ent, NPC npc, ref int damage ) {
			return true;
		}
		public abstract void PostHurtByNPC( CustomEntity ent, NPC npc, int damage );
	}
}
