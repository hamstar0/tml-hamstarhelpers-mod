using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using Terraria;
using HamstarHelpers.Helpers.PlayerHelpers;
using System.Collections.Generic;
using HamstarHelpers.Components.Network.Data;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerNewIdProtocol : PacketProtocolSentToEither {
		public IDictionary<int, string> PlayerIds;



		////////////////

		protected PlayerNewIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			this.PlayerIds = ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
		}

		
		////////////////

		protected override void SetClientDefaults() {
			this.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetProperUniqueId( Main.LocalPlayer );
		}

		protected override void SetServerDefaults( int to_who ) {
		}


		////////////////

		protected override void ReceiveOnServer( int from_who ) {
			string uid;
			if( this.PlayerIds.TryGetValue( from_who, out uid ) ) {
				ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[ from_who ] = uid;
			} else {
				LogHelpers.Log( "!ModHelpers.PlayerNewIdProtocol.ReceiveWithServer - No UID reported from player id'd "+from_who );
			}
		}

		protected override void ReceiveOnClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
