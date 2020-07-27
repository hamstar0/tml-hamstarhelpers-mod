﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Network.NetProtocol {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetProtocol : ILoadable {
		/// <summary>
		/// Sends the data to the server, and then rebroadcasts it to each (other) client.
		/// </summary>
		/// <param name="data"></param>
		public static void Broadcast( BroadcastNetProtocolPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetProtocol.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( ServerNetProtocolPayload data ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetProtocol.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( BidirectionalNetProtocolPayload data ) {
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
		public static void SendToClients( ClientNetProtocolPayload data, int toWho=-1, int ignoreWho=-1 ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetProtocol.Send( data, toWho, ignoreWho );
		}

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to. -1 for all players.</param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClients( BidirectionalNetProtocolPayload data, int toWho=-1, int ignoreWho=-1 ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetProtocol.Send( data, toWho, ignoreWho );
		}


		////////////////

		private static void Send( NetProtocolPayload data, int toWho, int ignoreWho ) {
			var netProtocol = ModContent.GetInstance<NetProtocol>();
			ModPacket packet = ModHelpersMod.Instance.GetPacket();

			netProtocol.Serializer.Serialize( packet.BaseStream, data );

			packet.Send( toWho, ignoreWho );
		}
	}
}
