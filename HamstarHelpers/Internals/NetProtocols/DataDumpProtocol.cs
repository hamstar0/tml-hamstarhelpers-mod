using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class DataDumpRequestProtocol : NetIORequestPayloadFromClient<DataDumpProtocol> {
		public static void QuickRequest() {
			NetIO.RequestDataFromClient( new DataDumpRequestProtocol() );
		}



		////////////////

		public DataDumpRequestProtocol() { }

		public override bool PreReplyOnClient( DataDumpProtocol reply ) {
			if( ModHelpersConfig.Instance.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
				string _;
				DataDumper.DumpToFile( out _ );
			}
			return false;
		}
	}




	[Serializable]
	class DataDumpProtocol : NetIOServerPayload {
		public DataDumpProtocol() { }


		public override void ReceiveOnServer( int fromWho ) { }
	}
}