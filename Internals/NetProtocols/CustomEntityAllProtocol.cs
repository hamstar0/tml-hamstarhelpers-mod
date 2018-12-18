using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocolSendToClient {
		public SerializableCustomEntity[] Entities;


		////////////////
		
		protected CustomEntityAllProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		////
		
		protected override void InitializeServerSendData( int fromWho ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<PeriodicSyncEntityComponent>();

			this.Entities = ents.Where( ent => ent.SyncFromClientServer.Item2 )
				.Select( ent => new SerializableCustomEntity(ent) )
				.ToArray();

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntityAllProtocol.SetServerDefaults - Sending " + string.Join(",\n   ", this.Entities.Select(e=>e.ToString())) );
			}*/
		}


		////////////////

		protected override void Receive() {
			CustomEntityManager.ClearAllEntities();

			foreach( SerializableCustomEntity ent in this.Entities ) {
				/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
					LogHelpers.Log( "ModHelpers.CustomEntityAllProtocol.ReceiveWithClient - New entity " + ent.ToString() );
				}*/
				
				var realEnt = CustomEntityManager.AddToWorld( ent.Core.whoAmI, ent, true );
			}

			SaveableEntityComponent.PostLoadAll();
		}
	}
}
