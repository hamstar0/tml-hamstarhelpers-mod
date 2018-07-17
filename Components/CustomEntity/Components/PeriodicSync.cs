using Terraria;


namespace HamstarHelpers.Components.CustomEntity.EntityProperties {
	public class PeriodicSyncEntityComponentData : CustomEntityComponentData {
		public int LastSynced { get; internal set; }


		internal PeriodicSyncEntityComponentData() {
			this.LastSynced = Main.rand.Next( 60 * 5 );
		}
	}



	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		protected override CustomEntityComponentData CreateData() {
			return new PeriodicSyncEntityComponentData();
		}

		
		public override void Update( CustomEntity ent ) {
			var sync_data = (PeriodicSyncEntityComponentData)ent.GetPropertyData( this );

			if( sync_data.LastSynced-- <= 0 ) {
				sync_data.LastSynced = 60 * 5;

				ent.Sync();
			}
		}
	}
}
