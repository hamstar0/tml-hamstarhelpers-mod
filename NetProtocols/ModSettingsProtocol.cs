using HamstarHelpers.Utilities.Network;
using Terraria;


namespace HamstarHelpers.NetProtocols {
	class HHModSettingsProtocol : PacketProtocol {
		public HamstarHelpersConfigData Data;

		////////////////

		public HHModSettingsProtocol() { }

		public override void SetServerDefaults() {
			this.Data = HamstarHelpersMod.Instance.Config;
		}

		public override void ReceiveOnClient() {
			var mymod = HamstarHelpersMod.Instance;
			mymod.Config.LoadFromNetwork( mymod, this.Data );
		}
	}
}
