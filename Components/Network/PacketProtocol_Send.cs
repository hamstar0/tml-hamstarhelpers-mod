using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal void SendRequestToClient( int toWho, int ignoreWho ) {
			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( true );

			try {
				packet.Send( toWho, ignoreWho );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendRequestToClient - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToClient " + toWho + ", " + ignoreWho );
			}
		}


		internal void SendRequestToServer() {
			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( true, false );

			try {
				packet.Send();
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendRequestToServer - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToServer" );
			}
		}


		////////////////

		/// <summary>
		/// Sends the current packet to the server.
		/// </summary>
		protected void SendToServer( bool syncToClients ) {
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Not a client.");
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( false, syncToClients );
			
			try {
				this.WriteStream( packet );

				packet.Send( -1, -1 );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendToServer - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string jsonStr = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToServer: " + jsonStr );
			}
		}


		/// <summary>
		/// Sends the current packet to the client.
		/// </summary>
		protected void SendToClient( int toWho, int ignoreWho ) {
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( false );

			try {
				this.WriteStream( packet );

				packet.Send( toWho, ignoreWho );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendToClient - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string jsonStr = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToClient " + toWho + ", " + ignoreWho + ": " + jsonStr );
			}
		}
	}
}
