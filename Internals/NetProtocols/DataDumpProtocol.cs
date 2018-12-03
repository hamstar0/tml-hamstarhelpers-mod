using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using HamstarHelpers.Services.DataDumper;
using System;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class DataDumpProtocol : PacketProtocolRequestToServer {
		protected DataDumpProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }
		
		protected override void SetServerDefaults( int fromWho ) { }

		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			if( ModHelpersMod.Instance.Config.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
				string _;
				DataDumper.DumpToFile( out _ );
			}

			return true;
		}

		protected override void ReceiveReply() {
			throw new NotImplementedException( "ReceiveReply not implemented." );
		}
	}
}
