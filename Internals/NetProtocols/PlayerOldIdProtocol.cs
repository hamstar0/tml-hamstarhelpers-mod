using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using Terraria;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerOldIdProtocol : PacketProtocolSendToServer {
		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";



		////////////////

		protected PlayerOldIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void SetClientDefaults() {
			var myplayer = Main.LocalPlayer.GetModPlayer<ModHelpersPlayer>();

			this.ClientPrivateUID = myplayer.Logic.PrivateUID;
			this.ClientHasUID = myplayer.Logic.HasLoadedUID;
		}


		////////////////

		protected override void Receive( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			
			myplayer.Logic.NetReceiveIdServer( this.ClientHasUID, this.ClientPrivateUID );
		}
	}
}
