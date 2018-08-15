using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerIdProtocol : PacketProtocol {
		public bool ClientHasUID = false;
		public string ClientPrivateUID = "";


		////////////////

		private PlayerIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }


		////////////////

		protected override void SetClientDefaults() {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			this.ClientPrivateUID = myplayer.Logic.PrivateUID;
			this.ClientHasUID = myplayer.Logic.HasUID;
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[ from_who ];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			
			myplayer.Logic.NetReceiveIdServer( this.ClientHasUID, this.ClientPrivateUID );
		}
	}
}
