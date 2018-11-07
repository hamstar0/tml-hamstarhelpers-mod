using HamstarHelpers.Components.Network.Data;
using Terraria.Utilities;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		private static UnifiedRandom MyRand = new UnifiedRandom();


		////////////////

		private int LastSynced;



		////////////////

		private PeriodicSyncEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : this() { }

		public PeriodicSyncEntityComponent() {
			this.LastSynced = PeriodicSyncEntityComponent.MyRand.Next( 60 * 30 );
		}


		////////////////

		public void UpdateMe( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 15;
				
				ent.SyncTo();
			}
		}

		public override void UpdateServer( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
	}
}
