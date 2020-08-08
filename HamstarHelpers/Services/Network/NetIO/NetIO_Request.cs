using System;
using Terraria;
using Terraria.ID;
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
		/// Sends a request to the given client(s).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="toWho"></param>
		/// <param name="ignoreWho"></param>
		public static void RequestDataFromClient<T>( NetProtocolRequest<T> data, int toWho = -1, int ignoreWho = -1 )
					where T : NetProtocolBidirectionalPayload {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetIO.Send( data, toWho, ignoreWho );
		}
		
		/// <summary>
		/// Sends a request to the given client(s).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="toWho"></param>
		/// <param name="ignoreWho"></param>
		public static void RequestDataFromClient<T>( NetProtocolRequestFromClientToServer<T> data, int toWho = -1, int ignoreWho = -1 )
					where T : NetProtocolServerPayload {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			NetIO.Send( data, toWho, ignoreWho );
		}


		////

		/// <summary>
		/// Sends a request to the server.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		public static void RequestDataFromServer<T>( NetProtocolRequest<T> data )
					where T : NetProtocolBidirectionalPayload {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends a request to the server.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		public static void RequestDataFromServer<T>( NetProtocolRequestFromServerToClient<T> data )
					where T : NetProtocolClientPayload {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}
	}
}
