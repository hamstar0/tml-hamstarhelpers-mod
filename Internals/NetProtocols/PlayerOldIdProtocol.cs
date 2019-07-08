using HamstarHelpers.Helpers.Debug;
using Terraria;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Components.Protocols.Packet.Interfaces;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class PlayerOldIdProtocol : PacketProtocolSentToEither {
		public static void QuickRequestToClient( int playerWho ) {
			PacketProtocolSentToEither.QuickRequestToClient<PlayerOldIdProtocol>( playerWho, -1, -1 );
		}

		public static void QuickSendToServer() {
			PlayerNewIdProtocol.QuickSendToServer<PlayerOldIdProtocol>();
		}



		////////////////

		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		private PlayerOldIdProtocol() { }
		
		////

		protected override void SetClientDefaults() {
			var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			this.ClientPrivateUID = myplayer.Logic.OldPrivateUID;
			this.ClientHasUID = myplayer.Logic.HasLoadedOldUID;
		}

		protected override void SetServerDefaults( int toWho ) { }


		////////////////

		protected override bool ReceiveRequestWithServer( int fromWho ) {
			throw new HamstarException( "Not implemented." );
		}

		////

		protected override void ReceiveOnClient() { }

		protected override void ReceiveOnServer( int fromWho ) {
			Player player = Main.player[fromWho];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			myplayer.Logic.NetReceiveIdOnServer( this.ClientHasUID, this.ClientPrivateUID );
		}
	}
}
