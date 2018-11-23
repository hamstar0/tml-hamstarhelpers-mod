using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Newtonsoft.Json;
using Terraria.Utilities;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		public static PeriodicSyncEntityComponent CreatePeriodicSyncEntityComponent() {
			return (PeriodicSyncEntityComponent)PacketProtocolData.CreateRaw( typeof(PeriodicSyncEntityComponent) );
		}



		////////////////

		private static UnifiedRandom MyRand = new UnifiedRandom();


		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		private int NextSync;



		////////////////

		protected PeriodicSyncEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			this.NextSync = PeriodicSyncEntityComponent.MyRand.Next( 60 * 30 );
		}


		////////////////

		public void UpdateMe( CustomEntity ent ) {
			if( this.NextSync-- <= 0 ) {
				this.NextSync = 60 * 15;
				
				ent.SyncToAll();
			}
		}

		public override void UpdateServer( CustomEntity ent ) {
			this.UpdateMe( ent );
		}
	}
}
