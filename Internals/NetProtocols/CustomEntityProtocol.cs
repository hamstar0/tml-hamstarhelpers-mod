using System;
using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocolSentToEither {
		protected class MyFactory {
			public SerializableCustomEntity Entity;
			
			public MyFactory( CustomEntity ent ) {
				this.Entity = new SerializableCustomEntity(ent);
			}
		}



		////////////////

		public static void SendToClients( CustomEntity ent ) {
			var factory = new MyFactory( ent );
			var protocol = CustomEntityProtocol.CreateDefault<CustomEntityProtocol>( factory );

			protocol.SendToClient( -1, -1 );
		}

		public static void SyncToAll( CustomEntity ent ) {
			var factory = new MyFactory( ent );
			var protocol = CustomEntityProtocol.CreateDefault<CustomEntityProtocol>( factory );

			protocol.SendToServer( true );
		}



		////////////////

		public SerializableCustomEntity Entity;


		////////////////

		protected override Tuple<object, Type> _MyFactoryType => Tuple.Create( (object)this, typeof(MyFactory) );



		////////////////

		protected CustomEntityProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


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
