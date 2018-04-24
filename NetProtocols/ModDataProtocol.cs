using HamstarHelpers.Utilities.Network;
using Terraria;


namespace HamstarHelpers.NetProtocols {
	class HHModDataProtocol : PacketProtocol {
		public int HalfDays;

		////////////////

		public override void SetServerDefaults() {
			HamstarHelpersMod.Instance.WorldHelpers.SaveForNetwork( this );
		}

		protected override void ReceiveWithClient() {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			HamstarHelpersMod.Instance.WorldHelpers.LoadFromNetwork( this.HalfDays );

			myplayer.Logic.FinishModDataSync();
		}
	}
}
