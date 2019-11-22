using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using Newtonsoft.Json;
using System;
using System.Threading;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		/// <summary>
		/// Sends the current packet to the server.
		/// </summary>
		/// <param name="syncToClients">Indicates packet should resume being sent to each client.</param>
		protected void SendToServer( bool syncToClients ) {
			if( Main.netMode != 1 ) {
				throw new ModHelpersException( "Not a client." );
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetClientPacket( false, syncToClients );

			if( this.IsAsync ) {
				ThreadPool.QueueUserWorkItem( _ => {
					this.Send_Core( packet );
				} );
			} else {
				this.Send_Core( packet );
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				string jsonStr = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToServer: " + jsonStr );
			}
		}


		/// <summary>
		/// Sends the current packet to the client.
		/// </summary>
		protected void SendToClient( int toWho, int ignoreWho ) {
			if( Main.netMode != 2 ) {
				throw new ModHelpersException( "Not server." );
			}

			var mymod = ModHelpersMod.Instance;
			ModPacket packet = this.GetServerPacket( false );

			if( this.IsAsync ) {
				ThreadPool.QueueUserWorkItem( _ => {
					this.Send_Core( packet, toWho, ignoreWho );
				} );
			} else {
				this.Send_Core( packet, toWho, ignoreWho );
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo && this.IsVerbose ) {
				string jsonStr = JsonConvert.SerializeObject( this );
				LogHelpers.Log( ">" + this.GetPacketName() + " SendToClient " + toWho + ", " + ignoreWho + ": " + jsonStr );
			}
		}


		////////////////

		private void Send_Core( ModPacket packet, int toWho=-1, int ignoreWho=-1 ) {
			try {
				this.WriteStream( packet );
				packet.Send( toWho, ignoreWho );
			} catch( Exception e ) {
				LogHelpers.Warn( e.ToString() );
				return;
			}
		}
	}
}
