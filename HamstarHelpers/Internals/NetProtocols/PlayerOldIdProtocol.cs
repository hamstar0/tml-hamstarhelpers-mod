using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerOldIdRequestProtocol : SimplePacketPayload {	//NetIORequest<PlayerOldIdProtocol> {
		public static void QuickRequestToClient( int playerWho ) {
			var protocol = new PlayerOldIdRequestProtocol();

			SimplePacket.SendToClient( protocol, playerWho, -1 );
		}


		////////////////

		public PlayerOldIdRequestProtocol() { }

		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException();
		}

		public override void ReceiveOnClient() {
			PlayerOldIdProtocol.QuickSendToServer();
		}
	}




	[Serializable]
	class PlayerOldIdProtocol : SimplePacketPayload {	//NetIOBidirectionalPayload {
		public static void QuickSendToServer() {
			var protocol = new PlayerOldIdProtocol();
			var myplayer = TmlLibraries.SafelyGetModPlayer<ModHelpersPlayer>( Main.LocalPlayer );

			protocol.ClientPrivateUID = myplayer.Logic.OldPrivateUID;
			protocol.ClientHasUID = myplayer.Logic.HasLoadedOldUID;

			SimplePacket.SendToServer( protocol );
		}



		////////////////

		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		public PlayerOldIdProtocol() { }


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[fromWho];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveUIDOnServer( this.ClientHasUID, this.ClientPrivateUID );
		}

		public override void ReceiveOnClient() { }
	}
}