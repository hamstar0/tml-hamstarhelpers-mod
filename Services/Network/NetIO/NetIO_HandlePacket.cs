using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		internal static bool HandlePacket( BinaryReader reader, int playerWho ) {
			var netProtocol = ModContent.GetInstance<NetIO>();

			object data = netProtocol.Serializer.Deserialize( reader.BaseStream );
			if( data == null || data.GetType() == typeof( object ) ) {
				return false;
			}

			if( data is NetProtocolBroadcastPayload ) {
				NetIO.Receive( data as NetProtocolBroadcastPayload, playerWho );
				return true;
			}
			if( data is NetProtocolServerPayload ) {
				NetIO.Receive( data as NetProtocolServerPayload, playerWho );
				return true;
			}
			if( data is NetProtocolClientPayload ) {
				NetIO.Receive( data as NetProtocolClientPayload, playerWho );
				return true;
			}
			if( data is NetProtocolBidirectionalPayload ) {
				NetIO.Receive( data as NetProtocolBidirectionalPayload, playerWho );
				return true;
			}

			return false;
		}
	}
}
