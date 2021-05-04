using System;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Stream;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Threading;


namespace HamstarHelpers.Classes.Protocols.Packet {
	public abstract partial class PacketProtocol : StreamProtocol {
		/// <summary>
		/// Sends the current packet to the server.
		/// </summary>
		/// <param name="syncToClients">Indicates packet should resume being sent to each client.</param>
		protected void SendToServer( bool syncToClients ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not a client." );
			}

			var mymod = ModHelpersMod.Instance;
			
			if( this.IsAsync ) {
				TaskLauncher.Run( ( _ ) => {
					ModPacket packet = this.GetClientPacket( false, syncToClients );
					this.Send_Core( packet );
				} );
			} else {
				ModPacket packet = this.GetClientPacket( false, syncToClients );
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
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server." );
			}

			var mymod = ModHelpersMod.Instance;
			
//LogHelpers.LogOnce( "UUU "+string.Join("\n  ", DebugHelpers.GetContextSlice()) );
			if( this.IsAsync ) {
				TaskLauncher.Run( ( _ ) => {
					ModPacket packet = this.GetServerPacket( false );
					this.Send_Core( packet, toWho, ignoreWho );
				} );
			} else {
				ModPacket packet = this.GetServerPacket( false );
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
