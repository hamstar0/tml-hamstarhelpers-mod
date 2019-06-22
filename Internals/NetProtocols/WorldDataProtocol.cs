using HamstarHelpers.Components.Protocol.Packet.Interfaces;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class WorldDataProtocol : PacketProtocolRequestToServer {
		public static void QuickRequest() {
			PacketProtocolRequestToServer.QuickRequest<WorldDataProtocol>( -1 );
		}



		////////////////

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
