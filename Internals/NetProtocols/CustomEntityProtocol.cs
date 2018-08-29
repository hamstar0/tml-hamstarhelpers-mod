using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


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

		private CustomEntityProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }
		
		private CustomEntityProtocol( CustomEntity ent ) {
			this.Entity = ent;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
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

		protected override void ReceiveWithClient() {
			var existing_ent = CustomEntityManager.GetEntityByWho( this.Entity.Core.whoAmI );

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				if( existing_ent == null ) {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - New entity " + this.Entity.ToString() );
				} else {
					LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithClient - Entity update for " + existing_ent.ToString() + " from "+ this.Entity.ToString() );
				}
			}*/

			if( existing_ent == null ) {
				CustomEntityManager.SetEntityByWho( this.Entity.Core.whoAmI, this.Entity );
			} else {
				existing_ent.SyncFrom( this.Entity );
			}
		}
	}
}
