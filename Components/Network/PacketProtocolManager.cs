using HamstarHelpers.Components.Errors;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.Network {
	class PacketProtocolManager {
		private IDictionary<int, Type> PacketProtocolTypesByCode = new Dictionary<int, Type>();
		private IDictionary<int, int> RequestResponseQueue = new Dictionary<int, int>();



		////////////////

		internal void OnPostSetupContent() {
			this.PacketProtocolTypesByCode = PacketProtocol.GetProtocolTypes();
		}


		////////////////

		public Type GetProtocolType( int protocolCode ) {
			var mymod = ModHelpersMod.Instance;

			if( !this.PacketProtocolTypesByCode.ContainsKey( protocolCode ) ) {
				//throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Unrecognized packet." );
				throw new HamstarException( "Unrecognized packet." );
			}

			Type protocolType;
			this.PacketProtocolTypesByCode.TryGetValue( protocolCode, out protocolType );

			return protocolType;
		}


		////////////////

		public int GetRequestsOf( int code ) {
			if( !this.RequestResponseQueue.ContainsKey( code ) ) {
				return 0;
			} else {
				return this.RequestResponseQueue[code];
			}
		}

		public bool FulfillRequest( int code ) {
			if( this.RequestResponseQueue[ code ] > 0 ) {
				this.RequestResponseQueue[ code ] -= 1;
				return true;
			}
			return false;
		}

		public void ExpectReqest( int code ) {
			if( !this.RequestResponseQueue.ContainsKey(code) ) {
				this.RequestResponseQueue[ code ] = 1;
			} else {
				this.RequestResponseQueue[ code ] += 1;
			}
		}
	}
}
