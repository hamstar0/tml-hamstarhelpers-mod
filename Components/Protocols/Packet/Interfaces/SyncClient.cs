using HamstarHelpers.Components.Errors;
using System;
using Terraria;


namespace HamstarHelpers.Components.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for syncing data between clients and server.
	/// </summary>
	public abstract class PacketProtocolSyncClient : PacketProtocol {
		/// <summary>
		/// "Quick" method for syncing packets from a client to everyone else with any PacketProtocolSyncClient class. Intended to be wrapped with
		/// a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected static void SyncFromMe<T>() where T : PacketProtocolSyncClient {
			PacketProtocol.QuickSendToServer<T>();
		}
		/// <summary>
		/// "Quick" method for syncing packets from everyone else to the current client with any PacketProtocolSyncClient class. Intended to be
		/// wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="retries">Number of times to attempt to resend the packet if it fails. -1 for perpetual retries.</param>
		protected static void SyncToMe<T>( int retries ) where T : PacketProtocolSyncClient {
			PacketProtocol.QuickRequestToServer<T>( retries );
		}



		////////////////

		/// @private
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			for( int i = 0; i < Main.player.Length; i++ ) {
				if( i == fromWho ) { continue; }

				this.InitializeServerRequestReplyDataOfClient( fromWho, i );
				this.OnClone();

				this.SendToClient( fromWho, -1 );
			}

			return true;
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
		/// Initializes request reply packet for sending.
		/// </summary>
		protected abstract void InitializeServerRequestReplyDataOfClient( int toWho, int fromWho );
		/// @private
		protected sealed override void SetServerDefaults( int toWho ) {
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


		////////////////

		/// @private
		protected sealed override bool ReceiveRequestWithClient() {
			throw new ModHelpersException( "Not implemented" );
		}
	}
}
