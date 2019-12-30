using HamstarHelpers.Classes.Errors;
using System;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending data to clients.
	/// </summary>
	public abstract class PacketProtocolSendToClient : PacketProtocol {
		/// <summary>
		/// "Quick" method for sending packets with any PacketProtocolSendToClient class. Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toWho">Player "whoAmI" value to send the packet to. -1 for everyone.</param>
		/// <param name="ignoreWho">Player "whoAmI" value to skip sending packet to. -1 for no one.</param>
		protected static void QuickSend<T>( int toWho, int ignoreWho ) where T : PacketProtocolSendToClient {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}



		////////////////

		/// <summary>
		/// Initializes packet for sending.
		/// </summary>
		protected abstract void InitializeServerSendData( int toWho );
		/// @private
		protected sealed override void SetServerDefaults( int toWho ) {
			this.InitializeServerSendData( toWho );
		}

		/// <summary>
		/// Implements handling of received replies on the client.
		/// </summary>
		protected abstract void Receive();
		/// @private
		protected sealed override void ReceiveWithClient() {
			this.Receive();
		}


		////////////////

		/// @private
		protected sealed override void SetClientDefaults() {
			throw new ModHelpersException( "Not implemented" );
		}

		/// @private
		protected sealed override void ReceiveWithServer( int fromWho ) { }
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho, bool isSyncedWithClients ) {
			throw new ModHelpersException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithClient() {
			throw new ModHelpersException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}
