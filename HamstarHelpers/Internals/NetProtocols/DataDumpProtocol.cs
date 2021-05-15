using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Services.Debug.DataDumper;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class DataDumpRequestProtocol : SimplePacketPayload {	//NetIORequestPayloadFromClient<DataDumpProtocol>
		public static bool QuickRequestIf() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not server" );
			}

			if( !DataDumper.CanRequestDumpOnServer() ) {
				return false;
			}

			SimplePacket.SendToServer( new DataDumpRequestProtocol() );
			return true;
		}



		////////////////

		public DataDumpRequestProtocol() { }


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			if( !ModHelpersConfig.Instance.DebugModeDumpAlsoServer ) {
				return;
			}

			if( !Main.player[fromWho].active ) {
				return;
			}

			if( !UserHelpers.HasBasicServerPrivilege(Main.player[fromWho]) ) {
				LogHelpers.Alert( "Player "+Main.player[fromWho].ToString()+" lacks server privilege." );

				return;
			}

			DataDumper.DumpToFile( out string _ );

			//PreReplyOnClient
		}


		public override void ReceiveOnClient() {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}