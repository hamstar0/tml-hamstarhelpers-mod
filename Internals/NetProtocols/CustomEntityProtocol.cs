using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocolSentToEither {
		protected sealed class MyFactory : Factory<CustomEntityProtocol> {
			private readonly SerializableCustomEntity Entity;


			////////////////

			public MyFactory( CustomEntity ent ) {
				this.Entity = new SerializableCustomEntity(ent);
			}

			////

			protected override void Initialize( CustomEntityProtocol data ) {
				data.Entity = this.Entity;
			}
		}



		////////////////

		public static void SendToClients( CustomEntity ent ) {
			var factory = new MyFactory( ent );
			CustomEntityProtocol protocol = factory.Create();

			protocol.SendToClient( -1, -1 );
		}

		public static void SyncToAll( CustomEntity ent ) {
			var factory = new MyFactory( ent );
			CustomEntityProtocol protocol = factory.Create();

			protocol.SendToServer( true );
		}



		////////////////

		public SerializableCustomEntity Entity;



		////////////////

		protected CustomEntityProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


		////////////////

		private void Receive() {
			var newEnt = CustomEntityManager.GetEntityByWho( this.Entity.Core.WhoAmI );

			if( newEnt == null ) {
				CustomEntityManager.AddToWorld( this.Entity.Core.WhoAmI, this.Entity.Convert(), true );
			} else {
				if( newEnt.GetType().Name != this.Entity.MyTypeName ) {
					LogHelpers.Log( "!ModHelpers.CustomEntityProtocol.Receive - Entity mismatch: "
						+ "Client sends " + newEnt.GetType().Name + ", server expects " + this.Entity.MyTypeName );
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
