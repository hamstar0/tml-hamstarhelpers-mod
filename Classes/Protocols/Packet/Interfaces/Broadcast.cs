using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Classes.Protocols.Packet.Interfaces {
	/// <summary>
	/// Recommended PacketProtocol form for sending data to server and/or clients.
	/// </summary>
	public abstract class PacketProtocolBroadcast : PacketProtocol {
		/// <summary>
		/// "Quick" method for sending packets to the server with any PacketProtocolSentToEither class. Intended to be
		/// wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected static void QuickBroadcastFromClient<T>() where T : PacketProtocolBroadcast {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			PacketProtocol.QuickSyncToServerAndClients<T>();
		}

		/// <summary>
		/// "Quick" method for sending packets to all clients with any PacketProtocolBroadcast class. Intended to be
		/// wrapped with a `public static` method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ignoreWho">Player "whoAmI" value to skip sending packet to. -1 for no one.</param>
		protected static void QuickBroadcastFromServer<T>( int ignoreWho ) where T : PacketProtocolBroadcast {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			PacketProtocol.QuickSendToClient<T>( -1, ignoreWho );
		}



		////////////////

		/// @private
		protected sealed override void SetServerDefaults( int toWho ) { }


		/// <summary>
		/// Implements handling of received broadcasts on the client.
		/// </summary>
		protected abstract void ReceiveOnClient();
		/// @private
		protected sealed override void ReceiveWithClient() {
			this.ReceiveOnClient();
		}

		/// <summary>
		/// Implements handling of received messages on the server (before re-broadcast to clients).
		/// </summary>
		/// <param name="fromWho"></param>
		protected abstract void ReceiveOnServer( int fromWho );

		/// @private
		[Obsolete]
		protected sealed override void ReceiveWithServer( int fromWho ) { }

		/// @private
		protected sealed override void ReceiveWithServer( int fromWho, bool isSyncedWithClients ) {
			this.ReceiveOnServer( fromWho );
		}
	}
}
