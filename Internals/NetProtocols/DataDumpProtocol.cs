using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using HamstarHelpers.Services.DataDumper;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class DataDumpProtocol : PacketProtocol {
		private DataDumpProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetServerDefaults( int from_who ) { }

		////////////////

		protected override bool ReceiveRequestWithServer( int from_who ) {
			if( ModHelpersMod.Instance.Config.DebugModeDumpAlsoServer || UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer ) ) {
				string _;
				DataDumper.DumpToFile( out _ );
			}

			return true;
		}
	}
}
