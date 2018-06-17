using HamstarHelpers.Components.Network;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class WorldDataProtocol : PacketProtocol {
		public int HalfDays;
		public bool HasCorrectWorldId;
		public string ObsoleteWorldId;


		////////////////

		public override void SetServerDefaults() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			this.HalfDays = WorldHelpers.WorldHelpers.GetElapsedHalfDays();
			this.HasCorrectWorldId = myworld.HasCorrectID;
			this.ObsoleteWorldId = myworld.ObsoleteID;
		}

		protected override void ReceiveWithClient() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			myworld.HasCorrectID = this.HasCorrectWorldId;
			myworld.ObsoleteID = this.ObsoleteWorldId;
			HamstarHelpersMod.Instance.WorldHelpers.LoadFromData( HamstarHelpersMod.Instance, this.HalfDays, this.ObsoleteWorldId );

			myplayer.Logic.FinishWorldDataSync();
		}
	}
}
