using HamstarHelpers.Components.Errors;
using System;


namespace HamstarHelpers.Components.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending data to the server.
	/// </summary>
	public abstract class PacketProtocolSendToServer : PacketProtocol {
		/// <summary>
		/// "Quick" method for sending packets with any PacketProtocolSendToServer class. Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected static void QuickSend<T>() where T : PacketProtocolSendToServer {
			PacketProtocol.QuickSendToServer<T>();
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
		/// Implements handling of received replies on the server.
		/// </summary>
		protected abstract void Receive( int fromWho );
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.Receive( fromWho );
		}


		////////////////

		/// @private
		protected sealed override void SetServerDefaults( int toWho ) {
			throw new HamstarException( "Not implemented" );
		}
		/// @private
		protected sealed override void ReceiveWithClient() {
			throw new HamstarException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithClient() {
			throw new HamstarException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			throw new HamstarException( "Not implemented" );
		}
	}
}
