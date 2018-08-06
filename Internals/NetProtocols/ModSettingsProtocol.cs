using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults() {
			this.Data = (HamstarHelpersConfigData)HamstarHelpersMod.Instance.Config.Clone();
			this.Data.PrivilegedUserId = "";
		}

		////////////////

		protected override void ReceiveWithClient() {
			HamstarHelpersMod.Instance.Config.LoadFromNetwork( HamstarHelpersMod.Instance, this.Data );
		}
	}
}
