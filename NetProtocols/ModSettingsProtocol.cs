using HamstarHelpers.Utilities.Network;


namespace HamstarHelpers.NetProtocols {
	class HHModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;

		////////////////

		public override void SetServerDefaults() {
			this.Data = HamstarHelpersMod.Instance.Config;
		}

		public override void ReceiveOnClient() {
			var mymod = HamstarHelpersMod.Instance;
			mymod.Config.LoadFromNetwork( mymod, this.Data );
		}
	}
}
