using System;
using Terraria;
using System.Collections.Generic;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerNewIdRequestProtocol : NetProtocolRequest<PlayerNewIdProtocol> {
		public static void QuickRequestToClient( int playerWho ) {
			var protocol = new PlayerNewIdRequestProtocol();

			NetIO.RequestDataFromClient( protocol, playerWho );
		}


		////////////////

		private PlayerNewIdRequestProtocol() { }


		public override bool PreReplyOnClient( PlayerNewIdProtocol reply ) {
			reply.PlayerIds = (Dictionary<int, string>)ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
			return true;
		}
	}




	[Serializable]
	class PlayerNewIdProtocol : NetProtocolBidirectionalPayload {
		public static void QuickSendToServer() {
			var protocol = new PlayerNewIdProtocol(
				(Dictionary<int, string>)ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds
			);
			protocol.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer );

			NetIO.SendToServer( protocol );
		}



		////////////////

		public Dictionary<int, string> PlayerIds;



		////////////////

		private PlayerNewIdProtocol() {
			this.PlayerIds = new Dictionary<int, string>();
		}

		private PlayerNewIdProtocol( Dictionary<int, string> playerIds ) {
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
				if( this.PlayerIds.TryGetValue(fromWho, out string uid) ) {
					ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[ fromWho ] = uid;
				} else {
					LogHelpers.Warn( "No UID reported from player id'd "+fromWho );
				}
			} catch { }
		}

		public override void ReceiveOnClient() {
			try {
				this.PlayerIds.TryGetValue( 0, out string _ );
			} catch {
				return;
			}
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
