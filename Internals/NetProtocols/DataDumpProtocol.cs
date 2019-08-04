using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Debug.DataDumper;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class DataDumpProtocol : PacketProtocolRequestToServer {
		public static void QuickRequest() {
			PacketProtocolRequestToServer.QuickRequest<DataDumpProtocol>( -1 );
		}



		////////////////

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
