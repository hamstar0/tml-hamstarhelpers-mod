using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class PlayerPermaDeathProtocol : PacketProtocolSentToEither {
		public static void BroadcastFromClient( int playerDeadWho, string msg ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );

			protocol.SendToServer( true );
		}

		public static void BroadcastFromServer( int playerDeadWho, string msg ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );

			protocol.SendToClient( -1, -1 );
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
			Player player = Main.player[this.PlayerWho];
			if( player == null || !player.active ) {
				LogHelpers.Log( "ModHelpers.PlayerPermaDeathProtocol.ReceiveWithClient - Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.ApplyPermaDeath( player, this.Msg );
		}
	}
}