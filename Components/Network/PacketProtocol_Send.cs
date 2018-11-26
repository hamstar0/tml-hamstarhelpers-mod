using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Components.Network {
	public abstract partial class PacketProtocol : PacketProtocolData {
		internal void SendRequestToClient( int to_who, int ignore_who ) {
			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( true );

			try {
				packet.Send( to_who, ignore_who );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendRequestToClient - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				LogHelpers.Log( ">" + this.GetPacketName() + " SendRequestToClient " + to_who + ", " + ignore_who );
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
		protected void SendToServer( bool sync_to_clients ) {
			if( Main.netMode != 1 ) {
				throw new HamstarException( "Not a client.");
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( false, sync_to_clients );
			
			try {
				this.WriteStream( packet );

				packet.Send( -1, -1 );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendToServer - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToServer: " + json_str );
			}
		}


		/// <summary>
		/// Sends the current packet to the client.
		/// </summary>
		protected void SendToClient( int to_who, int ignore_who ) {
			if( Main.netMode != 2 ) {
				throw new HamstarException( "Not server." );
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( false );

			try {
				this.WriteStream( packet );

				packet.Send( to_who, ignore_who );
			} catch( Exception e ) {
				LogHelpers.Log( "!PacketProtocol.SendToClient - " + e.ToString() );
				return;
			}

			if( mymod.Config.DebugModeNetInfo && this.IsVerbose ) {
				string json_str = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToClient " + to_who + ", " + ignore_who + ": " + json_str );
			}
		}
	}
}
