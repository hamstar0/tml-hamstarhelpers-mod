using Terraria;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		public int LastSynced;


		internal PeriodicSyncEntityComponent() {
			this.LastSynced = Main.rand.Next( 60 * 5 );
		}


		public override void Update( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 5;

				ent.Sync();
			}
		}
	}
}
