using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Timers;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		internal void SendRequestToClient( int toWho, int ignoreWho, int retries ) {
			var mymod = ModHelpersMod.Instance;
			try {
				ModPacket packet = this.GetServerPacket( true );

				packet.Send( toWho, ignoreWho );
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return;
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				LogLibraries.Log( ">" + this.GetPacketName() + " SendRequestToClient " + toWho + ", " + ignoreWho );
			}
			
			this.RetryRequestToClientIfTimeout( toWho, ignoreWho, retries );
		}


		internal void SendRequestToServer( int retries ) {
			try {
				ModPacket packet = this.GetClientPacket( true, false );

				packet.Send();
			} catch( Exception e ) {
				LogLibraries.Warn( e.ToString() );
				return;
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				LogLibraries.Log( ">" + this.GetPacketName() + " SendRequestToServer" );
			}

			this.RetryRequestToServerIfTimeout( retries );
		}


		////////////////

		private void RetryRequestToClientIfTimeout( int toWho, int ignoreWho, int retries ) {
			if( retries == 0 ) { return; }

			var mymod = ModHelpersMod.Instance;
			string protocolName = this.GetType().Name;
			string timerName = protocolName + "RequestToClientTimeout";
			int retryDuration = ModHelpersConfig.Instance.PacketRequestRetryDuration;

			mymod.PacketProtocolMngr.ExpectReqest( protocolName );

			Timers.SetTimer( timerName, retryDuration, true, () => {
				var mymod2 = ModHelpersMod.Instance;
				if( !mymod2.PacketProtocolMngr.IsRequesting(protocolName) ) {
					return false;
				}

				if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
					LogLibraries.Log( "  Request (to client) timed out. Retrying " + this.GetType().Name + " request "
						+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
				}

				mymod2.PacketProtocolMngr.FulfillRequest( protocolName );

				this.SendRequestToClient( toWho, ignoreWho, retries - 1 );

				return false;
			} );
		}

		private void RetryRequestToServerIfTimeout( int retries ) {
			if( retries == 0 ) { return; }

			var mymod = ModHelpersMod.Instance;
			string protocolName = this.GetType().Name;
			string timerName = this.GetType().Name + "RequestToServerTimeout";
			int retryDuration = ModHelpersConfig.Instance.PacketRequestRetryDuration;

			mymod.PacketProtocolMngr.ExpectReqest( protocolName );

			Timers.SetTimer( timerName, retryDuration, true, () => {
				var mymod2 = ModHelpersMod.Instance;
				if( !mymod2.PacketProtocolMngr.IsRequesting( protocolName ) ) {
					return false;
				}

				if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
					LogLibraries.Log( "  Request (to server) timed out. Retrying " + this.GetType().Name + " request "
						+ ( retries > 0 ? ( retries + " tries left" ) : ( "until success" ) ) + ")..." );
				}

				mymod2.PacketProtocolMngr.FulfillRequest( protocolName );

				this.SendRequestToServer( retries - 1 );

				return false;
			} );
		}
	}
}
