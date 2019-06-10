using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Protocol.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.DataDumper;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class DataDumpProtocol : PacketProtocolRequestToServer {
		private DataDumpProtocol() { }
		
		protected override void InitializeServerSendData( int fromWho ) { }

		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			if( ModHelpersMod.Instance.Config.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
				string _;
				DataDumper.DumpToFile( out _ );
			}

			return false;
		}

		protected override void ReceiveReply() { }
	}
}
