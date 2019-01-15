using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusNpcEntityComponent : CustomEntityComponent {
		protected HitRadiusNpcEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


		////////////////

		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, NPC npc, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, NPC npc, int damage );
	}
}
