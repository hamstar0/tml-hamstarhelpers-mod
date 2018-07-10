using Terraria;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	public class PeriodicSyncEntityPropertyData : CustomEntityPropertyData {
		public int LastSynced { get; internal set; }


		internal PeriodicSyncEntityPropertyData() {
			this.LastSynced = Main.rand.Next( 60 * 5 );
		}
	}




	public class PeriodicSyncEntityProperty : CustomEntityProperty {
		public override CustomEntityPropertyData CreateData() {
			return new PeriodicSyncEntityPropertyData();
		}

		
		public override void Update( CustomEntity ent ) {
			var sync_data = (PeriodicSyncEntityPropertyData)ent.GetPropertyData( this );

			if( sync_data.LastSynced-- <= 0 ) {
				sync_data.LastSynced = 60 * 5;

				SyncCustomEntityProtocol.SendToClients( ent );
			}
		}
	}
}
