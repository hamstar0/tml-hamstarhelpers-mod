using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Services.Promises;
using Terraria;
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

			this.ConfirmLoad();
		}

		////////////////

		public override void UpdateClient( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 15;

				ent.SyncTo();
			}
		}

		public override void UpdateServer( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 15;
				
				ent.SyncTo();
			}
		}
	}
}
