using HamstarHelpers.Utilities.Network;
using Terraria;


namespace HamstarHelpers.NetProtocols {
	class HHModDataProtocol : PacketProtocol {
		public int HalfDays;

		////////////////

		public HHModDataProtocol() { }

		public override void SetServerDefaults() {
			HamstarHelpersMod.Instance.WorldHelpers.SaveForNetwork( this );
		}

		public override void ReceiveOnClient() {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			HamstarHelpersMod.Instance.WorldHelpers.LoadFromNetwork( this.HalfDays );

			myplayer.Logic.FinishModDataSync();
		}
	}
}
