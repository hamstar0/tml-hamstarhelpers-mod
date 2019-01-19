using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusPlayerEntityComponent : CustomEntityComponent {
		private HitRadiusPlayerEntityComponent() { }


		////////////////

		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Player player, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Player player, int damage );
	}
}
