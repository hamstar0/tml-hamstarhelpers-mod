using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocolSentToEither {
		protected class MyFactory : PacketProtocolData.Factory<PlayerPermaDeathProtocol> {
			public MyFactory( int player_who, string msg, out PlayerPermaDeathProtocol protocol ) : base( out protocol ) {
				protocol.PlayerWho = player_who;
				protocol.Msg = msg;
			}
		}


		////////////////

		public static void SendToAll( int player_dead_who, string msg ) {
			PlayerPermaDeathProtocol protocol;
			new MyFactory( player_dead_who, msg, out protocol );

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
		
		protected PlayerPermaDeathProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		protected override void ReceiveOnServer( int from_who ) {
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
