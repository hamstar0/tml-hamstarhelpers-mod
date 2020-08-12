using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NetSerializer;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
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
		public static void Broadcast( NetIOBroadcastPayload data ) {
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
		public static void SendToServer( NetIOServerPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			NetIO.Send( data, -1, -1 );
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( NetIOBidirectionalPayload data ) {
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
		public static void SendToClients( NetIOClientPayload data, int toWho = -1, int ignoreWho = -1 ) {
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
		public static void SendToClients( NetIOBroadcastPayload data, int ignoreWho = -1 ) {
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
		public static void SendToClients( NetIOBidirectionalPayload data, int toWho = -1, int ignoreWho = -1 ) {
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
		public static void SendToClient( NetIOBroadcastPayload data, int toWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			if( toWho == -1 ) {
				throw new ModHelpersException( "For use with specific clients only." );
			}
			NetIO.Send( data, toWho, -1 );
		}


		////////////////

		private static bool Send( NetIOPayload data, int toWho, int ignoreWho ) {
			var netIO = ModContent.GetInstance<NetIO>();
			ModPacket packet = ModHelpersMod.Instance.GetPacket();

			try {
				Type dataType = data.GetType();
				if( !netIO.PayloadTypeToCode.TryGetValue(dataType, out int code) ) {
					return false;
				}

				Serializer ser = netIO.PayloadCodeToSerializer[ code ];

				packet.Write( (int)code );

				MethodInfo method = ser.GetType().GetMethod( "SerializeDirect", ReflectionHelpers.MostAccess );
				method = method.MakeGenericMethod( new Type[] { dataType } );
				method.Invoke( ser, new object[] { packet.BaseStream, data } );

				packet.Send( toWho, ignoreWho );

				if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
					LogHelpers.Log( ">" + dataType.Name + " "
						+(Main.netMode == NetmodeID.MultiplayerClient ? "from client" : "from server")
					);
				}

				return true;
			} catch( Exception e ) {
				LogHelpers.WarnOnce( e.ToString() );
			}
			
			return false;
		}
	}
}
