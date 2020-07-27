using System;
using System.IO;
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
		private static void Receive( NetProtocolBroadcastPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServerBeforeRebroadcast( playerWho );
				NetIO.Send( data, -1, playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveBroadcastOnClient( playerWho );
			} else {
				throw new ModHelpersException( "Not MP" );
			}
		}

		private static void Receive( NetProtocolServerPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			data.ReceiveOnServer( playerWho );
		}

		private static void Receive( NetProtocolClientPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			data.ReceiveOnClient( playerWho );
		}

		private static void Receive( NetProtocolBidirectionalPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveOnClient( playerWho );
			} else if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServer( playerWho );
			}
		}
	}
}
