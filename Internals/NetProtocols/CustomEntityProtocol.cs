using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;

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

		protected override void ReceiveWithServer( int from_who ) {
			var ent = CustomEntityManager.Get( this.Entity.Core.whoAmI );
			
			if( ent == null ) {
				LogHelpers.Log( "HamstarHelpers.CustomEntityProtocol.ReceiveWithServer - Could not find existing entity for " + this.Entity.ToString() );
				return;
			}
			
			ent.SyncFrom( this.Entity );
		}

		protected override void ReceiveWithClient() {
			var ent = CustomEntityManager.Get( this.Entity.Core.whoAmI );

			if( ent == null ) {
				CustomEntityManager.Set( this.Entity.Core.whoAmI, this.Entity );
			} else {
				ent.SyncFrom( this.Entity );
			}
		}
	}
}
