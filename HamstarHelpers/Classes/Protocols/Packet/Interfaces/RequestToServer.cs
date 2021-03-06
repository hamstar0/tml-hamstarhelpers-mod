﻿using HamstarHelpers.Classes.Errors;
using System;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending requests to the server.
	/// </summary>
	public abstract class PacketProtocolRequestToServer : PacketProtocol {
		/// <summary>
		/// "Quick" method for making requests with any PacketProtocolRequestToServer class. Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="retries">Number of times to attempt to resend the packet if it fails. -1 for perpetual retries.</param>
		protected static void QuickRequest<T>( int retries ) where T : PacketProtocolRequestToServer {
			PacketProtocolRequestToServer.QuickRequestToServer<T>( retries );
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
		/// Implements handling of received request replies on the client.
		/// </summary>
		protected abstract void ReceiveReply();
		/// @private
		protected sealed override void ReceiveWithClient() {
			this.ReceiveReply();
		}


		////////////////

		/// @private
		protected sealed override void SetClientDefaults() {
			throw new ModHelpersException( "Not implemented" );
		}

		/// @private
#pragma warning disable CS0672 // Member overrides obsolete member
		protected sealed override void ReceiveWithServer( int fromWho ) { }
#pragma warning restore CS0672 // Member overrides obsolete member
		/// @private
		protected sealed override void ReceiveWithServer( int fromWho, bool isSyncedWithClients ) {
			throw new ModHelpersException( "Not implemented" );
		}
		/// @private
		protected sealed override bool ReceiveRequestWithClient() {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}
