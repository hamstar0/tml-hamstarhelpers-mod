using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	class DataDumpProtocol : NetProtocolClientPayload {
		public override void ReceiveOnClient( int fromWho ) { }
	}




	[Serializable]
	class DataDumpRequestProtocol : NetProtocolRequestServerPayload<DataDumpProtocol> {
		public static void QuickRequest() {
			NetIO.RequestFromServer( new DataDumpRequestProtocol() );
		}



		////////////////

		private DataDumpRequestProtocol() { }

		public override void PreReply( DataDumpProtocol reply, int fromWho ) {
			if( ModHelpersConfig.Instance.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
				string _;
				DataDumper.DumpToFile( out _ );
			}
		}
	}
}
