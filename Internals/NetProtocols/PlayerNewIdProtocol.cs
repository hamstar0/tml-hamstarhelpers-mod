using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;
using HamstarHelpers.Helpers.PlayerHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerNewIdProtocol : PacketProtocol {
		public IDictionary<int, string> PlayerIds;



		////////////////

		private PlayerNewIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) {
			this.PlayerIds = ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
		}


		////////////////

		protected override void SetClientDefaults() {
			this.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetProperUniqueId( Main.LocalPlayer );
		}

		protected override void SetServerDefaults( int to_who ) {
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			string uid;
			if( this.PlayerIds.TryGetValue( from_who, out uid ) ) {
				ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds[ from_who ] = uid;
			} else {
				LogHelpers.Log( "!ModHelpers.PlayerNewIdProtocol.ReceiveWithServer - No UID reported from player id'd "+from_who );
			}
		}

		protected override void ReceiveWithClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}
