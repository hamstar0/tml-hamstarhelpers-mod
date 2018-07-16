using Terraria;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	class PeriodicSyncEntityData : CustomEntityPropertyData {
		public int LastSynced { get; internal set; }


		internal PeriodicSyncEntityData() {
			this.LastSynced = Main.rand.Next( 60 * 5 );
		}
	}



	public class PeriodicSyncEntityProperty : CustomEntityProperty {
		protected override CustomEntityPropertyData CreateData() {
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
