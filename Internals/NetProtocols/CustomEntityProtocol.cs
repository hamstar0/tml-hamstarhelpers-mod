using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocolSentToEither {
		protected sealed class MyFactory : PacketProtocolData.Factory<CustomEntityProtocol> {
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

		protected override void ReceiveOnServer( int fromWho ) {
			var ent = CustomEntityManager.GetEntityByWho( this.Entity.Core.whoAmI );

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				if( ent != null ) {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithServer - Syncing entity " + ent.ToString() + "..." );
				}
			}*/

			if( ent == null ) {
				LogHelpers.Log( "!ModHelpers.CustomEntityProtocol.ReceiveWithServer - No existing entity to sync " + this.Entity.ToString() );
				return;
			}
			
			ent.CopyChangesFrom( this.Entity );
		}

		protected override void ReceiveOnClient() {
			var existingEnt = CustomEntityManager.GetEntityByWho( this.Entity.Core.whoAmI );

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				if( existingEnt == null ) {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - New entity " + this.Entity.ToString() );
				} else {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - Entity update for " + existingEnt.ToString() + " from "+ this.Entity.ToString() );
				}
			}*/

			if( existingEnt == null ) {
				var realEnt = CustomEntityManager.AddToWorld( this.Entity.Core.whoAmI, this.Entity );
			} else {
				existingEnt.CopyChangesFrom( this.Entity );
			}
		}
	}
}
