using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending requests to clients.
	/// </summary>
	public abstract class PacketProtocolRequestToClient : PacketProtocol {
		/// <summary>
		/// "Quick" method for making requests with any PacketProtocolRequestToClient class. Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toWho">Player "whoAmI" value to send the packet to. -1 for everyone.</param>
		/// <param name="ignoreWho">Player "whoAmI" value to skip sending packet to. -1 for no one.</param>
		/// <param name="retries">Number of times to attempt to resend the packet if it fails. -1 for perpetual retries.</param>
		protected static void QuickRequest<T>( int toWho, int ignoreWho, int retries ) where T : PacketProtocolRequestToClient {
			PacketProtocol.QuickRequestToClient<T>( toWho, ignoreWho, retries );
		}



		////////////////

		/// <summary>
		/// Initializes packet for sending.
		/// </summary>
		protected abstract void InitializeClientSendData();
		/// @private
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}

		/// <summary>
		/// Implements handling of received request replies on the server.
		/// </summary>
		protected abstract void ReceiveReply( int fromWho );
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveReply( fromWho );
		}


		////////////////

		/// @private
		protected sealed override void SetServerDefaults( int toWho ) {
			throw new ModHelpersException( "Not implemented" );
		}
		/// @private
		protected sealed override void ReceiveWithClient() {
			throw new ModHelpersException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}
