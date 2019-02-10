using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal void SendRequestToClient( int toWho, int ignoreWho, int retries ) {
			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( true );

			try {
				packet.Send( toWho, ignoreWho );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToClient " + toWho + ", " + ignoreWho );
			}
			
			if( retries != 0 ) {
				this.RetryRequestToClientIfTimeout( toWho, ignoreWho, retries );
			}
		}


		internal void SendRequestToServer( int retries ) {
			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( true, false );

			try {
				packet.Send();
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToServer" );
			}

			if( retries != 0 ) {
				this.RetryRequestToServerIfTimeout( retries );
			}
		}


		////////////////

		private void RetryRequestToClientIfTimeout( int toWho, int ignoreWho, int retries ) {
			var mymod = ModHelpersMod.Instance;
			int packetCode = PacketProtocol.GetPacketCode( this.GetType().Name );
			int retryDuration = mymod.Config.PacketRequestRetryDuration;

			mymod.PacketProtocolMngr.ExpectReqest( packetCode );

			if( retries > 0 ) {
				Timers.SetTimer( "PacketProtocolRequestToClientTimeout", retryDuration, () => {
					if( mymod.PacketProtocolMngr.GetRequestsOf( packetCode ) > 0 ) {
						if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
							LogHelpers.Log( "  Request timed out. Retrying " + this.GetType().Name + " request to client"
								+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
						}

						mymod.PacketProtocolMngr.FulfillRequest( packetCode );

						this.SendRequestToClient( toWho, ignoreWho, retries - 1 );
					}
					return false;
				} );
			}
		}

		private void RetryRequestToServerIfTimeout( int retries ) {
			var mymod = ModHelpersMod.Instance;
			int packetCode = PacketProtocol.GetPacketCode( this.GetType().Name );
			int retryDuration = mymod.Config.PacketRequestRetryDuration;

			mymod.PacketProtocolMngr.ExpectReqest( packetCode );

			if( retries > 0 ) {
				Timers.SetTimer( "PacketProtocolRequestToServerTimeout", retryDuration, () => {
					if( mymod.PacketProtocolMngr.GetRequestsOf( packetCode ) > 0 ) {
						if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
							LogHelpers.Log( "  Request timed out. Retrying " + this.GetType().Name + " request to server ("
								+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
						}

						mymod.PacketProtocolMngr.FulfillRequest( packetCode );

						this.SendRequestToServer( retries - 1 );
					}
					return false;
				} );
			}
		}
	}
}
