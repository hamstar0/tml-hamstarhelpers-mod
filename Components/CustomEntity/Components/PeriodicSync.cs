using HamstarHelpers.Services.Promises;
using Terraria.Utilities;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		private static UnifiedRandom MyRand = new UnifiedRandom();


		////////////////

		private int LastSynced;



		////////////////

		public PeriodicSyncEntityComponent() {
			this.LastSynced = PeriodicSyncEntityComponent.MyRand.Next( 60 * 30 );

			this.ConfirmLoad();
		}

		////////////////

		public override void UpdateServer( CustomEntity ent ) {
			if( this.LastSynced-- <= 0 ) {
				this.LastSynced = 60 * 30;

				ent.SyncTo();
			}
		}
	}
}
