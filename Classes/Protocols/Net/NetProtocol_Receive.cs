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

			if( data is IBroadcastNetProtocolPayload ) {
				NetProtocol.Receive( data as IBroadcastNetProtocolPayload, playerWho );
				return true;
			}
			if( data is IServerNetProtocolPayload ) {
				NetProtocol.Receive( data as IServerNetProtocolPayload, playerWho );
				return true;
			}
			if( data is IClientNetProtocolPayload ) {
				NetProtocol.Receive( data as IClientNetProtocolPayload, playerWho );
				return true;
			}

			return false;
		}


		////

		private static void Receive( IBroadcastNetProtocolPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveBroadcastOnServer( playerWho );
				NetProtocol.Send( data, -1, playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveBroadcastOnClient( playerWho );
			} else {
				throw new ModHelpersException( "Not MP" );
			}
		}

		private static void Receive( IServerNetProtocolPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			data.ReceiveOnServer( playerWho );
		}

		private static void Receive( IClientNetProtocolPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			data.ReceiveOnClient();
		}
	}
}
