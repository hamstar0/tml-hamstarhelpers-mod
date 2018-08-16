using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.WorldHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class WorldDataProtocol : PacketProtocol {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;


		////////////////

		private WorldDataProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }


		////////////////

		protected override void SetServerDefaults() {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();

			this.HalfDays = WorldHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoletedID;
			this.ObsoletedWorldId = myworld.ObsoletedID;
		}


		////////////////

		protected override void ReceiveWithClient() {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			myworld.HasObsoletedID = this.HasObsoletedWorldId;
			myworld.ObsoletedID = this.ObsoletedWorldId;

			mymod.WorldHelpers.LoadFromData( mymod, this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSync();
		}
	}
}
