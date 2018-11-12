using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.WorldHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class WorldDataProtocol : PacketProtocolRequestToServer {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;


		////////////////

		private WorldDataProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }


		////////////////

		protected override void SetServerDefaults( int from_who ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDays = WorldStateHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoletedID;
			this.ObsoletedWorldId = myworld.ObsoletedID;
		}


		////////////////

		protected override void ReceiveWithClient() {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();
			var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();

			myworld.HasObsoletedID = this.HasObsoletedWorldId;
			myworld.ObsoletedID = this.ObsoletedWorldId;

			mymod.WorldStateHelpers.LoadFromData( mymod, this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSync();
		}
	}
}
