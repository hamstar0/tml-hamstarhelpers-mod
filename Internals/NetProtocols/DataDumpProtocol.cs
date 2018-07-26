using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class DataDumpProtocol : PacketProtocol {
		public override void SetServerDefaults() { }


		protected override bool ReceiveRequestWithServer( int from_who ) {
			bool success;
			if( UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer, out success ) ) {
				DataDumpHelpers.DumpToFile();
			}

			return true;
		}
	}
}
