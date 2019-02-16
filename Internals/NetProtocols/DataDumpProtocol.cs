using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using HamstarHelpers.Services.DataDumper;
using System;
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

			return true;
		}

		protected override void ReceiveReply() {
			throw new HamstarException( "ReceiveReply not implemented." );
		}
	}
}
