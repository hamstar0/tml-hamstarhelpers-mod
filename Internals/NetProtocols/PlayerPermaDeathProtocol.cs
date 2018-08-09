using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocol {
		public static void SendToAll( int player_dead_who, string msg ) {
			var protocol = new PlayerPermaDeathProtocol( player_dead_who, msg );

			if( Main.netMode == 1 ) {
				protocol.SendToServer( true );
			} else if( Main.netMode == 2 ) {
				protocol.SendToClient( -1, -1 );
			}
		}


		////////////////

		public int PlayerWho;
		public string Msg;


		////////////////

		private PlayerPermaDeathProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }

		private PlayerPermaDeathProtocol( int player_who, string msg ) {
			this.PlayerWho = player_who;
			this.Msg = msg;
		}

		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			//Player player = Main.player[ this.PlayerWho ];

			//PlayerHelpers.ApplyPermaDeath( player, this.Msg );	?
		}

		protected override void ReceiveWithClient() {
			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "ModHelpers.PlayerPermaDeathProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}
