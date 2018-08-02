using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocol {
		public CustomEntity[] Entities;


		////////////////

		public override void SetServerDefaults() {
			this.Entities = CustomEntityManager.Instance.EntitiesByIndexes.Values.Where(
				ent => ent.GetComponentByType<PeriodicSyncEntityComponent>() != null
			).ToArray();
		}

		protected override void ReceiveWithClient() {
			foreach( CustomEntity ent in this.Entities ) {
				CustomEntityManager.Instance.Set( ent.Core.whoAmI, ent );
			}

			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey );
		}
	}
}
