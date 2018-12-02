using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusNPCEntityComponent : CustomEntityComponent {
		protected class HitRadiusNPCEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : HitRadiusProjectileEntityComponent {
			public float Radius;

			public HitRadiusNPCEntityComponentFactory( float radius ) {
				this.Radius = radius;
			}

			protected override void InitializeComponent( T data ) {
				data.Radius = this.Radius;
			}
		}



		////////////////

		public static HitRadiusProjectileEntityComponent CreateAttackableEntityComponent( float radius ) {
			var factory = new HitRadiusNPCEntityComponentFactory<HitRadiusProjectileEntityComponent>( radius );
			return factory.Create();
		}


		////////////////
		
		public bool IsImmune = false;
		public float Radius;



		////////////////

		protected HitRadiusNPCEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////
		
		public virtual bool PreHurt( CustomEntity ent, NPC npc, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, NPC npc, int damage );
	}
}
