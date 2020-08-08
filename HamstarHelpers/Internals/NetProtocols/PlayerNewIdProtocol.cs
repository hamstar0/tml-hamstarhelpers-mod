﻿using System;
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
	}




	[Serializable]
	class PlayerNewIdProtocol : NetProtocolBidirectionalPayload {
		public static void QuickSendToServer() {
			var protocol = new PlayerNewIdProtocol();
			protocol.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer );

			NetIO.SendToServer( protocol );
		}



		////////////////

		public Dictionary<int, string> PlayerIds;



		////////////////

		private PlayerNewIdProtocol() {
			this.PlayerIds = (Dictionary<int, string>)ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			string uid;
			if( this.PlayerIds.TryGetValue(fromWho, out uid) ) {
				ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[ fromWho ] = uid;
			} else {
				LogHelpers.Warn( "No UID reported from player id'd "+fromWho );
			}
		}

		public override void ReceiveOnClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
