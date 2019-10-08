using HamstarHelpers.Classes.Errors;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Classes.Protocols.Packet {
	class PacketProtocolManager {
		private static object MyLock = new object();



		////////////////

		private IDictionary<int, Type> PacketProtocolTypesByCode = new Dictionary<int, Type>();
		private readonly IDictionary<string, bool> RequestResponseQueue = new Dictionary<string, bool>();



		////////////////

		internal void OnPostModsLoad() {
			this.PacketProtocolTypesByCode = PacketProtocol.GetProtocolTypes();
		}


		////////////////

		public Type GetProtocolType( int protocolCode ) {
			var mymod = ModHelpersMod.Instance;
			Type protocolType;

			lock( PacketProtocolManager.MyLock ) {
				if( !this.PacketProtocolTypesByCode.ContainsKey( protocolCode ) ) {
					//throw new HamstarException( "PacketProtocol.HandlePacketOnServer - Unrecognized packet." );
					throw new ModHelpersException( "Unrecognized packet." );
				}

				this.PacketProtocolTypesByCode.TryGetValue( protocolCode, out protocolType );
			}

			return protocolType;
		}


		////////////////

		public bool IsRequesting( string protocolName ) {
			if( this.RequestResponseQueue.ContainsKey( protocolName ) ) {
				return this.RequestResponseQueue[protocolName];
			}
			return false;
		}

		public bool FulfillRequest( string protocolName ) {
			if( this.RequestResponseQueue.ContainsKey( protocolName ) && this.RequestResponseQueue[protocolName] ) {
				this.RequestResponseQueue[protocolName] = false;
				return true;
			}
			return false;
		}

		public void ExpectReqest( string protocolName ) {
			this.RequestResponseQueue[protocolName] = true;
		}
	}
}
