using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocol {
		public IDictionary<int, CustomEntity> Entities;


		////////////////

		public override void SetServerDefaults() {
			this.Entities = CustomEntityManager.Instance.EntitiesByIds.Where(
				kv => kv.Value.GetComponentByType<PeriodicSyncEntityComponent>() != null
			).ToDictionary( kv => kv.Key, kv => kv.Value );
		}

		protected override void ReceiveWithClient() {
			foreach( var kv in this.Entities ) {
				CustomEntityManager.Instance.Set( kv.Key, kv.Value );
			}

			Promises.TriggerCustomPromiseForObject( SaveableEntityComponent.LoadHook );
		}
	}
}
