using System;
using Terraria;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for syncing data between clients.
	/// </summary>
	public abstract partial class PacketProtocolSyncBetweenClients : PacketProtocol {
		/// <summary>
		/// "Quick" method for syncing packets from everyone else to the current client with any PacketProtocolSyncClient
		/// class. Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="retries">Number of times to attempt to resend the packet if it fails. -1 for perpetual retries.</param>
		protected static void SyncFromClientsToMe<T>( int retries ) where T : PacketProtocolSyncBetweenClients {
			PacketProtocol.QuickRequestToServer<T>( retries );
		}

		/// <summary>
		/// "Quick" method for syncing packets from server to a given client with any PacketProtocolSyncClient class.
		/// Intended to be wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toWho"></param>
		/// <param name="ignoreWho"></param>
		protected static void SyncFromServerToClients<T>( int toWho, int ignoreWho ) where T : PacketProtocolSyncBetweenClients {
			PacketProtocol.QuickSendToClient<T>( toWho, ignoreWho );
		}



		////////////////

		/// @private
		protected sealed override bool ReceiveRequestWithServer( int fromWho ) {
			for( int i = 0; i < Main.player.Length; i++ ) {
				if( i == fromWho ) { continue; }
				if( !Main.player[i].active ) { continue; }

				this.InitializeServerRequestReply( fromWho, i );
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
		/// Initializes request reply packet for sending to a given client.
		/// </summary>
		protected abstract void InitializeServerRequestReply( int toWho, int fromWho );

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


		////////////////

		/// @private
#pragma warning disable CS0672 // Member overrides obsolete member
		protected sealed override void ReceiveWithServer( int fromWho ) {
			throw new ModHelpersException( "Not implemented" );
		}
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
