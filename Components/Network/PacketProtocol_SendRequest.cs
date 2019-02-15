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
			
			this.RetryRequestToClientIfTimeout( toWho, ignoreWho, retries );
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

			this.RetryRequestToServerIfTimeout( retries );
		}


		////////////////

		private void RetryRequestToClientIfTimeout( int toWho, int ignoreWho, int retries ) {
			if( retries == 0 ) { return; }

			int retryDuration = ModHelpersMod.Instance.Config.PacketRequestRetryDuration;
			
			Timers.SetTimer( "PacketProtocolRequestToClientTimeout", retryDuration, () => {
				if( ModHelpersMod.Instance.Config.DebugModeNetInfo && this.IsVerbose ) {
					LogHelpers.Log( "  Request (client) timed out. Retrying " + this.GetType().Name + " request "
						+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
				}

				this.SendRequestToClient( toWho, ignoreWho, retries - 1 );

				return false;
			} );
		}

		private void RetryRequestToServerIfTimeout( int retries ) {
			if( retries == 0 ) { return; }

			int retryDuration = ModHelpersMod.Instance.Config.PacketRequestRetryDuration;
			
			Timers.SetTimer( "PacketProtocolRequestToServerTimeout", retryDuration, () => {
				if( ModHelpersMod.Instance.Config.DebugModeNetInfo && this.IsVerbose ) {
					LogHelpers.Log( "  Request (server) timed out. Retrying " + this.GetType().Name + " request "
						+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
				}

				this.SendRequestToServer( retries - 1 );

				return false;
			} );
		}
	}
}
