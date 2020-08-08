using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		/// <summary>
		/// Sends the data to the server, and then rebroadcasts it to each (other) client.
		/// </summary>
		/// <param name="data"></param>
		public static void Broadcast( NetProtocolBroadcastPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}

		////

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( NetProtocolServerPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( NetProtocolBidirectionalPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}

		////

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to. -1 for all players.</param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClients( NetProtocolClientPayload data, int toWho = -1, int ignoreWho = -1 ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetIO.Send( data, toWho, ignoreWho );
		}

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClients( NetProtocolBroadcastPayload data, int ignoreWho = -1 ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetIO.Send( data, -1, ignoreWho );
		}

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to. -1 for all players.</param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClients( NetProtocolBidirectionalPayload data, int toWho = -1, int ignoreWho = -1 ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetIO.Send( data, toWho, ignoreWho );
		}


		////////////////

		/// <summary>
		/// Sends the data to the specified client.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to.</param>
		public static void SendToClient( NetProtocolBroadcastPayload data, int toWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			if( toWho == -1 ) {
				throw new ModHelpersException( "For use with specific clients only." );
			}
			NetIO.Send( data, toWho, -1 );
		}


		////////////////

		private static void Send( NetProtocolPayload data, int toWho, int ignoreWho ) {
			var netProtocol = ModContent.GetInstance<NetIO>();
			ModPacket packet = ModHelpersMod.Instance.GetPacket();

			netProtocol.Serializer.Serialize( packet.BaseStream, data );

			packet.Send( toWho, ignoreWho );
		}
	}
}
