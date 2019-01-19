using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerOldIdProtocol : PacketProtocolSendToServer {
		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		private PlayerOldIdProtocol() { }


		////////////////

		protected override void InitializeClientSendData() {
			var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();

			this.ClientPrivateUID = myplayer.Logic.PrivateUID;
			this.ClientHasUID = myplayer.Logic.HasLoadedUID;
		}


		////////////////

		protected override void Receive( int fromWho ) {
			Player player = Main.player[ fromWho ];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			
			myplayer.Logic.NetReceiveIdServer( this.ClientHasUID, this.ClientPrivateUID );
		}
	}
}
