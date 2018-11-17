using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using Terraria;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerPermaDeathProtocol : PacketProtocolSentToEither {
		protected class MyFactory : PacketProtocolData.Factory<PlayerPermaDeathProtocol> {
			private readonly int PlayerWho;
			private readonly string Msg;


			////////////////

			public MyFactory( int player_who, string msg ) {
				this.PlayerWho = player_who;
				this.Msg = msg;
			}

			////

			public override void Initialize( PlayerPermaDeathProtocol data ) {
				data.PlayerWho = this.PlayerWho;
				data.Msg = this.Msg;
			}
		}
		


		////////////////
		
		public static void SendToAll( int player_dead_who, string msg ) {
			var factory = new MyFactory( player_dead_who, msg );
			PlayerPermaDeathProtocol protocol = factory.Create();

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
