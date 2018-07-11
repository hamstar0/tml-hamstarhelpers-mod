using HamstarHelpers.Components.Network;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	class PeriodicSyncEntityData : CustomEntityData {
		public int LastSynced { get; internal set; }


		internal PeriodicSyncEntityData() {
			this.LastSynced = Main.rand.Next( 60 * 5 );
		}
	}



	class PeriodicSyncEntityProtocol : PacketProtocol {

	}



	public class PeriodicSyncEntityProperty : CustomEntityProperty {
		public override CustomEntityData CreateData() {
			return new PeriodicSyncEntityData();
		}

		
		public override void Update( CustomEntity ent ) {
			var sync_data = (PeriodicSyncEntityData)ent.GetPropertyData( this );

			if( sync_data.LastSynced-- <= 0 ) {
				sync_data.LastSynced = 60 * 5;

				ent.Sync();
			}
		}
	}
}
