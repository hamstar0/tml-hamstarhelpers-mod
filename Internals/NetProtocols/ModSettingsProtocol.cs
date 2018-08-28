using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults( int from_who ) {
			this.Data = (HamstarHelpersConfigData)ModHelpersMod.Instance.Config.Clone();
			this.Data.PrivilegedUserId = "";
		}

		////////////////

		protected override void ReceiveWithClient() {
			ModHelpersMod.Instance.Config.LoadFromNetwork( ModHelpersMod.Instance, this.Data );
		}
	}
}
