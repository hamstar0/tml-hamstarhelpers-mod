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
			
			if( ent == null ) {
				LogHelpers.Log( "!ModHelpers.CustomEntityProtocol.ReceiveWithServer - Could not find existing entity for " + this.Entity.ToString() );
				return;
			}
			
//LogHelpers.Log( "ModHelpers.CustomEntityProtocol.ReceiveWithServer - "+ent.ToString()+" behav:"+ent.GetComponentByName( "TrainBehaviorEntityComponent" )?.ToString() );
			ent.SyncFrom( this.Entity );
		}

		protected override void ReceiveWithClient() {
			var ent = CustomEntityManager.GetEntityByWho( this.Entity.Core.whoAmI );

			if( ent == null ) {
				CustomEntityManager.SetEntityByWho( this.Entity.Core.whoAmI, this.Entity );
			} else {
				ent.SyncFrom( this.Entity );
			}
		}
	}
}
