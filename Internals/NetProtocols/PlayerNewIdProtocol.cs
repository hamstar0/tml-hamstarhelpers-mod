using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Components.Network.Data;
using Terraria;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerNewIdProtocol : PacketProtocolSentToEither {
		public IDictionary<int, string> PlayerIds;



		////////////////

		protected PlayerNewIdProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) {
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
				LogHelpers.Log( "!ModHelpers.PlayerNewIdProtocol.ReceiveWithServer - No UID reported from player id'd "+fromWho );
			}
		}

		protected override void ReceiveOnClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
