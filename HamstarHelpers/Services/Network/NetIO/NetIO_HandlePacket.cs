using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="playerWho"></param>
		/// <returns>`true` to be considered to have completely handled the incoming packet.</returns>
		internal static bool HandlePacket( BinaryReader reader, int playerWho ) {
			var netIO = ModContent.GetInstance<NetIO>();
			long oldStreamPos = reader.BaseStream.Position;
			object data;

			try {
				data = netIO.Serializer.Deserialize( reader.BaseStream );
			} catch( Exception e ) {
				LogHelpers.Warn( e.Message );

				reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );
				return false;
			}

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
				NetIO.Receive( data as NetProtocolClientPayload );
				return true;
			}
			if( data is NetProtocolBidirectionalPayload ) {
				NetIO.Receive( data as NetProtocolBidirectionalPayload, playerWho );
				return true;
			}
			if( data is NetProtocolRequest ) {
				NetIO.ReceiveRequest( data as NetProtocolRequest, playerWho );
				return true;
			}

			reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );
			return false;
		}
	}
}
