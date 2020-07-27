using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Network.NetProtocol {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetProtocol : ILoadable {
		internal static bool Receive( BinaryReader reader, int playerWho ) {
			var netProtocol = ModContent.GetInstance<NetProtocol>();

			object data = netProtocol.Serializer.Deserialize( reader.BaseStream );
			if( data == null || data.GetType() == typeof( object ) ) {
				return false;
			}

			if( data is BroadcastNetProtocolPayload ) {
				NetProtocol.Receive( data as BroadcastNetProtocolPayload, playerWho );
				return true;
			}
			if( data is ServerNetProtocolPayload ) {
				NetProtocol.Receive( data as ServerNetProtocolPayload, playerWho );
				return true;
			}
			if( data is ClientNetProtocolPayload ) {
				NetProtocol.Receive( data as ClientNetProtocolPayload, playerWho );
				return true;
			}
			if( data is BidirectionalNetProtocolPayload ) {
				NetProtocol.Receive( data as BidirectionalNetProtocolPayload, playerWho );
				return true;
			}

			return false;
		}


		////

		private static void Receive( BroadcastNetProtocolPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveBroadcastOnServer( playerWho );
				NetProtocol.Send( data, -1, playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveBroadcastOnClient( playerWho );
			} else {
				throw new ModHelpersException( "Not MP" );
			}
		}

		private static void Receive( ServerNetProtocolPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			data.ReceiveOnServer( playerWho );
		}

		private static void Receive( ClientNetProtocolPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			data.ReceiveOnClient( playerWho );
		}

		private static void Receive( BidirectionalNetProtocolPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveOnClient( playerWho );
			} else if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServer( playerWho );
			}
		}
	}
}
