using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	[Serializable]
	class PlayerOldIdRequestClientProtocol : NetProtocolClientPayload {
		public override void ReceiveOnClient() {
			PlayerOldIdProtocol.QuickSendToServer();
		}
	}




	/// @private
	[Serializable]
	class PlayerOldIdProtocol : NetProtocolBidirectionalPayload {
		public static void QuickRequestToClient( int playerWho ) {
			var protocol = new PlayerOldIdRequestClientProtocol();

			NetIO.SendToClients( protocol, -1, -1 );
		}

		public static void QuickSendToServer() {
			var protocol = new PlayerOldIdProtocol();
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.LocalPlayer );
			protocol.ClientPrivateUID = myplayer.Logic.OldPrivateUID;
			protocol.ClientHasUID = myplayer.Logic.HasLoadedOldUID;

			NetIO.SendToServer( protocol );
		}



		////////////////

		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		private PlayerOldIdProtocol() { }


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[fromWho];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveUIDOnServer( this.ClientHasUID, this.ClientPrivateUID );
		}

		public override void ReceiveOnClient() { }
	}
}
