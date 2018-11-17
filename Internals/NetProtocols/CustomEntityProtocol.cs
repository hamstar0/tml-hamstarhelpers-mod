using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityProtocol : PacketProtocolSentToEither {
		protected class MyFactory : PacketProtocolData.Factory<CustomEntityProtocol> {
			private readonly CustomEntity Entity;


			////////////////

			public MyFactory( CustomEntity ent ) {
				this.Entity = ent;
			}

			////

			public override void Initialize( CustomEntityProtocol data ) {
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

		public CustomEntity Entity;



		////////////////

		protected CustomEntityProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void ReceiveOnServer( int from_who ) {
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
			
			ent.SyncFrom( this.Entity );
		}

		protected override void ReceiveOnClient() {
			var existing_ent = CustomEntityManager.GetEntityByWho( this.Entity.Core.whoAmI );

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				if( existing_ent == null ) {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - New entity " + this.Entity.ToString() );
				} else {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - Entity update for " + existing_ent.ToString() + " from "+ this.Entity.ToString() );
				}
			}*/

			if( existing_ent == null ) {
				CustomEntityManager.AddToWorld( this.Entity.Core.whoAmI, this.Entity );
			} else {
				existing_ent.SyncFrom( this.Entity );
			}
		}
	}
}
