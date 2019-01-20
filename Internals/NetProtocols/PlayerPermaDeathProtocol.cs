using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocolSentToEither {
		public static void SendToAll( int playerDeadWho, string msg ) {
			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );

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

		private PlayerPermaDeathProtocol() { }

		protected PlayerPermaDeathProtocol( int playerWho, string msg ) {
			this.PlayerWho = playerWho;
			this.Msg = msg;
		}


		////////////////

		protected override void ReceiveOnServer( int fromWho ) {
			//Player player = Main.player[ this.PlayerWho ];

			//PlayerHelpers.ApplyPermaDeath( player, this.Msg );	?
		}

		protected override void ReceiveOnClient() {
			Player player = Main.player[ this.PlayerWho ];
			if( player == null || !player.active ) {
				LogHelpers.Log( "ModHelpers.PlayerPermaDeathProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}
