using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		/// <summary>
		/// Runs when data received on client (class's own fields).
		/// </summary>
		protected virtual void ReceiveWithClient() {
			throw new NotImplementedException( "No ReceiveWithClient" );
		}

		/// <summary>
		/// Runs when data received on server (class's own fields).
		/// </summary>
		/// <param name="fromWho">Main.player index of the player (client) sending us our data.</param>
		protected virtual void ReceiveWithServer( int fromWho ) {
			throw new NotImplementedException( "No ReceiveWithServer" );
		}


		/// <summary>
		/// Runs when a request is received for the client to send data to the server. Expects
		/// `SetClientDefaults()` to be implemented.
		/// </summary>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithClient() {
			return false;
		}

		/// <summary>
		/// Runs when a request is received for the server to send data to the client. Expects
		/// `SetServerDefaults()` to be implemented.
		/// </summary>
		/// <param name="fromWho">Main.player index of player (client) sending this request.</param>
		/// <returns>True to indicate the request is being handled manually.</returns>
		protected virtual bool ReceiveRequestWithServer( int fromWho ) {
			return false;
		}
	}
}
