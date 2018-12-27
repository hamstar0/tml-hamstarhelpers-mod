using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		public class StaticInitializer {
			internal StaticInitializer() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}

			protected virtual void StaticInitialize() { }
		}



		////////////////

		internal void InternalOnAddToWorld( CustomEntity ent ) {
			this.OnAddToWorld( ent );
		}

		protected virtual void OnAddToWorld( CustomEntity ent ) { }

		public virtual void UpdateSingle( CustomEntity ent ) { }
		public virtual void UpdateClient( CustomEntity ent ) { }
		public virtual void UpdateServer( CustomEntity ent ) { }
	}
}
