using HamstarHelpers.Components.Errors;
using System;
using Terraria;


namespace HamstarHelpers.Components.Protocol.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for syncing data between clients and server.
	/// </summary>
	public abstract class PacketProtocolSyncClient : PacketProtocol {
		protected static void SyncFromMe<T>() where T : PacketProtocolSyncClient {
			PacketProtocol.QuickSendToServer<T>();
		}
		protected static void SyncToMe<T>( int retries ) where T : PacketProtocolSyncClient {
			PacketProtocol.QuickRequestToServer<T>( retries );
		}



		////////////////

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

		protected abstract void InitializeClientSendData();
		protected sealed override void SetClientDefaults() {
			this.InitializeClientSendData();
		}

		protected abstract void InitializeServerRequestReplyDataOfClient( int toWho, int fromWho );
		protected sealed override void SetServerDefaults( int toWho ) {
		}

		////////////////

		protected abstract void ReceiveOnClient();
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		protected abstract void ReceiveOnServer( int fromWho );
		protected sealed override void ReceiveWithServer( int fromWho ) {
			this.ReceiveOnServer( fromWho );
		}


		////////////////

		protected sealed override bool ReceiveRequestWithClient() {
			throw new HamstarException( "Not implemented" );
		}
	}
}
