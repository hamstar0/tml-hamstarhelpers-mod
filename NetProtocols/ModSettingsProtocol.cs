using HamstarHelpers.Components.Network;


namespace HamstarHelpers.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;

		////////////////

		public override void SetServerDefaults() {
			this.Data = HamstarHelpersMod.Instance.Config;
		}

		protected override void ReceiveWithClient() {
			HamstarHelpersMod.Instance.Config.LoadFromNetwork( HamstarHelpersMod.Instance, this.Data );
		}
	}
}
