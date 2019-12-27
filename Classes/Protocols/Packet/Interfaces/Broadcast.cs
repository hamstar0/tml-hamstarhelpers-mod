using HamstarHelpers.Classes.Errors;
using System;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending data to server and/or clients.
	/// </summary>
	public abstract class PacketProtocolBroadcast : PacketProtocol {
		/// <summary>
		/// "Quick" method for sending packets to all clients with any PacketProtocolBroadcast class. Intended to be
		/// wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ignoreWho">Player "whoAmI" value to skip sending packet to. -1 for no one.</param>
		protected static void QuickBroadcastFromServer<T>( int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( -1, ignoreWho );
		}

		/// <summary>
		/// "Quick" method for sending packets to the server with any PacketProtocolSentToEither class. Intended to be
		/// wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected static void QuickBroadcastFromClient<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}



		////////////////

		/// <summary>
		/// Implements handling of received broadcasts on the client.
		/// </summary>
		protected abstract void ReceiveOnClient();
		/// @private
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		/// <summary>
		/// Implements handling of received messages on the server. Re-broadcasting to clients is handled automatically.
		/// </summary>
		protected abstract bool ReceiveOnServer( int fromWho );
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );

			this.SendToClient( -1, fromWho );
		}
	}
}
