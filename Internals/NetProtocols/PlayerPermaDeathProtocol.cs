using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocolSentToEither {
		protected class MyFactory {
			private readonly int PlayerWho;
			private readonly string Msg;
			
			public MyFactory( int playerWho, string msg ) {
				this.PlayerWho = playerWho;
				this.Msg = msg;
			}
		}
		


		////////////////
		
		public static void SendToAll( int playerDeadWho, string msg ) {
			var factory = new MyFactory( playerDeadWho, msg );
			var protocol = PacketProtocolData.CreateDefault<PlayerPermaDeathProtocol>( factory );

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
		
		protected PlayerPermaDeathProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }


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
