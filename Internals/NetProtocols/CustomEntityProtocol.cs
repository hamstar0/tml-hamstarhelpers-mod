using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocol {
		public static void SendToClients( CustomEntity ent ) {
			var protocol = new CustomEntityProtocol( ent );
			protocol.SendToClient( -1, -1 );
		}

		public static void SyncToAll( CustomEntity ent ) {
			var protocol = new CustomEntityProtocol( ent );
			protocol.SendToServer( true );
		}



		////////////////

		public CustomEntity Entity;


		////////////////

		private CustomEntityProtocol() { }

		private CustomEntityProtocol( CustomEntity ent ) {
			this.Entity = ent;
		}

		////////////////

		protected override void ReceiveWithClient() {
			var ent = CustomEntityManager.Instance.Get( this.Entity.Core.whoAmI );

			if( ent == null ) {
				CustomEntityManager.Instance.Set( this.Entity.Core.whoAmI, this.Entity );
			} else {
				ent.CopyFrom( this.Entity );
			}
		}
	}
}
