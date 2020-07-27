using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Classes.Protocols.Net {
	/// <summary>
	/// Implement to define a network protocol. Protocols define what data to transmit, and how and where it can be
	/// transmitted.
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
			data.ReceiveOnClient();
		}
	}
}
