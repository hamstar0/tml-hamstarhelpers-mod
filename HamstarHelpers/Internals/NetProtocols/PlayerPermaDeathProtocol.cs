using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerPermaDeathProtocol : SimplePacketPayload {	//NetIOBroadcastPayload {
		public static void BroadcastFromClient( int playerDeadWho, string msg ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );
			SimplePacket.SendToServer( protocol );
		}

		
		public static void BroadcastFromServer( int playerDeadWho, string msg, bool ignoresDeadOne ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new PlayerPermaDeathProtocol( playerDeadWho, msg );
			SimplePacket.SendToClient(
				protocol,
				-1,
				ignoresDeadOne ? playerDeadWho : -1
			);
		}



		////////////////

		public int PlayerWho;
		public string Msg;



		////////////////

		public PlayerPermaDeathProtocol() { }

		protected PlayerPermaDeathProtocol( int playerWho, string msg ) {
			this.PlayerWho = playerWho;
			this.Msg = msg;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			//Player player = Main.player[ this.PlayerWho ];

			//PlayerHelpers.ApplyPermaDeath( player, this.Msg );	?
			PlayerPermaDeathProtocol.BroadcastFromServer( this.PlayerWho, this.Msg, true );
		}

		public override void ReceiveOnClient() {
			Player player = Main.player[this.PlayerWho];
			if( player == null || !player.active ) {
				LogHelpers.Alert( "Inactive player indexed as " + this.PlayerWho );
				return;
			}

			PlayerHelpers.ApplyPermaDeathState( player, this.Msg );
		}
	}
}