using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocol {
		public CustomEntity[] Entities;


		////////////////

		private CustomEntityAllProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults( int from_who ) {
			//CustomEntityManager mngr = ModHelpersMod.Instance.CustomEntMngr;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<PeriodicSyncEntityComponent>();

			this.Entities = ents.ToArray();
			//this.Entities = mngr.EntitiesByIndexes.Values.Where(
			//	ent => ent.GetComponentByType<PeriodicSyncEntityComponent>() != null
			//).ToArray();
		}


		////////////////

		protected override void ReceiveWithClient() {
			foreach( CustomEntity ent in this.Entities ) {
				CustomEntityManager.SetEntityByWho( ent.Core.whoAmI, ent );
			}

			SaveableEntityComponent.PostLoadAll();

			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey, null );
		}
	}
}
