using HamstarHelpers.Components.Network;
using HamstarHelpers.DebugHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocol {
		public int PlayerWho;
		public string Msg;


		////////////////

		internal PlayerPermaDeathProtocol( int player_who, string msg ) {
			this.PlayerWho = player_who;
			this.Msg = msg;

			if( Main.netMode == 1 ) {
				this.SendToServer( true );
			} else if( Main.netMode == 2 ) {
				this.SendToClient( -1, -1 );
			}
		}

		////////////////

		protected override void ReceiveWithClient() {
			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "HamstarHelpers - PlayerPermaDeathProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}
