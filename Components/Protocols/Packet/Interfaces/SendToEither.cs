using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending data to server and/or clients.
	/// </summary>
	public abstract class PacketProtocolSentToEither : PacketProtocol {
		/// <summary>
		/// "Quick" method for sending packets to the client with any PacketProtocolSentToEither class. Intended to be wrapped with a
		/// `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toWho">Player "whoAmI" value to send the packet to. -1 for everyone.</param>
		/// <param name="ignoreWho">Player "whoAmI" value to skip sending packet to. -1 for no one.</param>
		protected static void QuickSendToAClient<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}

		/// <summary>
		/// "Quick" method for sending packets to the server with any PacketProtocolSentToEither class. Intended to be wrapped with a
		/// `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected static void QuickSendToTheServer<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
		}



		////////////////

		/// <summary>
		/// Implements handling of received replies on the client.
		/// </summary>
		protected abstract void ReceiveOnClient();
		/// @private
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		/// <summary>
		/// Implements handling of received replies on the server.
		/// </summary>
		protected abstract void ReceiveOnServer( int fromWho );
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );
		}
	}
}
