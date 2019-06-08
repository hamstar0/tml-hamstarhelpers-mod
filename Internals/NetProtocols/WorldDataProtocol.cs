using HamstarHelpers.Components.Protocol.Packet.Interfaces;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class WorldDataProtocol : PacketProtocolRequestToServer {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;



		////////////////

		private WorldDataProtocol() { }


		////////////////

		protected override void InitializeServerSendData( int fromWho ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();

			this.HalfDays = WorldStateHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoleteId;
			this.ObsoletedWorldId = myworld.ObsoleteId;
		}


		////////////////

		protected override void ReceiveReply() {
			var mymod = ModHelpersMod.Instance;
			var myworld = mymod.GetModWorld<ModHelpersWorld>();
			var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			myworld.HasObsoleteId = this.HasObsoletedWorldId;
			myworld.ObsoleteId = this.ObsoletedWorldId;

			mymod.WorldStateHelpers.LoadFromData( this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSyncOnClient();
		}
	}
}
