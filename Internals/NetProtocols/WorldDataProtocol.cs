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

		protected WorldDataProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


		////////////////

		protected override void InitializeServerSendData( int fromWho ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDays = WorldStateHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoletedID;
			this.ObsoletedWorldId = myworld.ObsoletedID;
		}


		////////////////

		protected override void ReceiveReply() {
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
