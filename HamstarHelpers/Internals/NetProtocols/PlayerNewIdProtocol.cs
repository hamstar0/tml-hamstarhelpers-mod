using System;
using Terraria;
using System.Collections.Generic;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Network.SimplePacket;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerNewIdRequestProtocol : SimplePacketPayload {	//NetIORequest<PlayerNewIdProtocol>
		public static void QuickRequestToClient( int playerWho ) {
			var protocol = new PlayerNewIdRequestProtocol( playerWho );

			SimplePacket.SendToClient( protocol, playerWho, -1 );
		}


		////////////////

		public int PlayerWho;



		////////////////

		public PlayerNewIdRequestProtocol() { }

		public PlayerNewIdRequestProtocol( int playerWho ) {
			this.PlayerWho = playerWho;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			throw new ModHelpersException( "Not implemented" );
		}

		public override void ReceiveOnClient() {
			var protocol = new PlayerNewIdProtocol(
				(Dictionary<int, string>)ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds
			);

			SimplePacket.SendToClient( protocol, this.PlayerWho );
		}
	}




	[Serializable]
	class PlayerNewIdProtocol : SimplePacketPayload {   //NetIOBidirectionalPayload
		public static void QuickSendToServer() {
			var protocol = new PlayerNewIdProtocol(
				(Dictionary<int, string>)ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds
			);
			protocol.PlayerIds[Main.myPlayer] = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer );

			SimplePacket.SendToServer( protocol );
		}



		////////////////

		public Dictionary<int, string> PlayerIds;



		////////////////

		public PlayerNewIdProtocol() {
			this.PlayerIds = new Dictionary<int, string>();
		}

		public PlayerNewIdProtocol( Dictionary<int, string> playerIds ) {
			if( playerIds == null ) {
				this.PlayerIds = new Dictionary<int, string>();

				LogHelpers.Warn( "Player ids not specified." );
				return;
			}
			this.PlayerIds = playerIds;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			try {
				if( this.PlayerIds.TryGetValue( fromWho, out string uid ) ) {
					ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[fromWho] = uid;
				} else {
					LogHelpers.Warn( "No UID reported from player id'd " + fromWho );
				}
			} catch {
				this.PlayerIds = new Dictionary<int, string>();
				LogHelpers.Warn( "Deserialization error." );
			}
		}

		public override void ReceiveOnClient() {
			try {
				this.PlayerIds.TryGetValue( 0, out string _ );
			} catch {
				this.PlayerIds = new Dictionary<int, string>();
				LogHelpers.Warn( "Deserialization error." );
			}
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}