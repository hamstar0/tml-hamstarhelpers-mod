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

		protected PeriodicSyncEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) {
			this.NextSync = PeriodicSyncEntityComponent.MyRand.Next( 60 * 30 ) + 60;
		}


		////////////////

		public override void UpdateClient( CustomEntity ent ) {
			if( ent.SyncClientServer.Item1 ) {
				this.UpdateMe( ent );
			}
		}

		public override void UpdateServer( CustomEntity ent ) {
			if( ent.SyncClientServer.Item2 ) {
				this.UpdateMe( ent );
			}
		}

		////

		protected virtual bool UpdateMe( CustomEntity ent ) {
			if( this.NextSync-- <= 0 ) {
				this.NextSync = 60 * 15;

				ent.SyncToAll();
				return true;
			}
			return false;
		}
	}
}
