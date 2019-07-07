using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using Terraria;
using System.Collections.Generic;
using HamstarHelpers.Components.Protocols.Packet.Interfaces;


namespace HamstarHelpers.Internals.NetProtocols {
	/** @private */
	class PlayerNewIdProtocol : PacketProtocolSentToEither {
		public static void QuickRequestToClient( int playerWho ) {
			PacketProtocolSentToEither.QuickRequestToClient<PlayerNewIdProtocol>( playerWho, -1, -1 );
		}

		public static void QuickSendToServer() {
			PlayerNewIdProtocol.QuickSendToServer<PlayerNewIdProtocol>();
		}



		////////////////

		public IDictionary<int, string> PlayerIds;



		////////////////

		private PlayerNewIdProtocol() {
			this.PlayerIds = ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
		}

		
		////////////////

		protected override void SetClientDefaults() {
			this.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetProperUniqueId( Main.LocalPlayer );
		}

		protected override void SetServerDefaults( int toWho ) {
		}


		////////////////
		
		protected override void ReceiveOnServer( int fromWho ) {
			string uid;
			if( this.PlayerIds.TryGetValue( fromWho, out uid ) ) {
				ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[ fromWho ] = uid;
			} else {
				LogHelpers.Warn( "No UID reported from player id'd "+fromWho );
			}
		}

		protected override void ReceiveOnClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
