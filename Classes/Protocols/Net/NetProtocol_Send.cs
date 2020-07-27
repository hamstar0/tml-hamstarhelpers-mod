using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Classes.Protocols.Net {
	/// <summary>
	/// Implement to define a network protocol. Protocols define what data to transmit, and how and where it can be
	/// transmitted.
	/// </summary>
	public partial class NetProtocol : ILoadable {
		/// <summary>
		/// Sends the data to the server, and then rebroadcasts it to each (other) client.
		/// </summary>
		/// <param name="data"></param>
		public static void Broadcast( IBroadcastNetProtocolPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetProtocol.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( IServerNetProtocolPayload data ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetProtocol.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to. -1 for all players.</param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClients( IClientNetProtocolPayload data, int toWho=-1, int ignoreWho=-1 ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetProtocol.Send( data, toWho, ignoreWho );
		}


		////////////////

		private static void Send( INetProtocolPayload data, int toWho, int ignoreWho ) {
			var netProtocol = ModContent.GetInstance<NetProtocol>();
			ModPacket packet = ModHelpersMod.Instance.GetPacket();

			netProtocol.Serializer.Serialize( packet.BaseStream, data );

			packet.Send( toWho, ignoreWho );
		}
	}
}
