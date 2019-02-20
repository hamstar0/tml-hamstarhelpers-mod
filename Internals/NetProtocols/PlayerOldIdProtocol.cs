using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using Terraria;
using HamstarHelpers.Components.Errors;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerOldIdProtocol : PacketProtocolSentToEither {
		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		private PlayerOldIdProtocol() { }
		
		////

		protected override void SetClientDefaults() {
			var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();

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
