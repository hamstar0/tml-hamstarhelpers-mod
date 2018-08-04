using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocol {
		public CustomEntity[] Entities;


		////////////////

		private CustomEntityAllProtocol() { }
		
		protected override void SetServerDefaults() {
			CustomEntityManager mngr = HamstarHelpersMod.Instance.CustomEntMngr;

			this.Entities = mngr.EntitiesByIndexes.Values.Where(
				ent => ent.GetComponentByType<PeriodicSyncEntityComponent>() != null
			).ToArray();
		}


		////////////////

		protected override void ReceiveWithClient() {
			foreach( CustomEntity ent in this.Entities ) {
				CustomEntityManager.Set( ent.Core.whoAmI, ent );
			}

LogHelpers.Log("Loaded? "+string.Join(", ",CustomEntityManager.GetByComponentType<SaveableEntityComponent>().Select(c=>c.Core.DisplayName)));
			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey );
		}
	}
}
