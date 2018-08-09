using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerIdProtocol : PacketProtocol {
		public bool HasUID = false;
		public string PrivateUID = "";


		////////////////

		private PlayerIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		////////////////

		protected override void SetClientDefaults() {
			var myplayer = Main.LocalPlayer.GetModPlayer<HamstarHelpersPlayer>();

			this.PrivateUID = myplayer.Logic.PrivateUID;
			this.HasUID = myplayer.Logic.HasUID;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			Player player = Main.player[from_who];
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			
			myplayer.Logic.NetReceiveIdServer( this.HasUID, this.PrivateUID );
		}
	}
}
