using System;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocolSentToEither {
		public static void SendToClients( CustomEntity ent ) {
			var protocol = new CustomEntityProtocol( ent );
			protocol.SendToClient( -1, -1 );
		}

		public static void SyncToAll( CustomEntity ent ) {
			var protocol = new CustomEntityProtocol( ent );
			protocol.SendToServer( true );
		}



		////////////////

		public SerializableCustomEntity Entity;



		////////////////

		private CustomEntityProtocol() { }

		private CustomEntityProtocol( CustomEntity ent ) {
			this.Entity = new SerializableCustomEntity( ent );
		}


		////////////////

		private void Receive() {
			var newEnt = CustomEntityManager.GetEntityByWho( this.Entity.Core.WhoAmI );

			if( newEnt == null ) {
				CustomEntityManager.AddToWorld( this.Entity.Core.WhoAmI, this.Entity.Convert(), true );
			} else {
				if( newEnt.GetType().Name != this.Entity.MyTypeName ) {
					LogHelpers.Warn( "Entity mismatch: "
							+ "Client sends " + newEnt.GetType().Name+", "
							+ "server expects " + this.Entity.MyTypeName );
					return;
				}

				newEnt.CopyChangesFrom( this.Entity );
			}
		}


		protected override void ReceiveOnServer( int fromWho ) {
			this.Receive();
		}

		protected override void ReceiveOnClient() {
			this.Receive();
		}
	}
}
