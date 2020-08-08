using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		private static void Receive( NetProtocolBroadcastPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.Server ) {
				if( data.ReceiveOnServerBeforeRebroadcast(playerWho) ) {
					NetIO.Send( data, -1, playerWho );
				}
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveBroadcastOnClient();
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

		private static void Receive( NetProtocolClientPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			data.ReceiveOnClient();
		}

		private static void Receive( NetProtocolBidirectionalPayload data, int playerWho ) {
			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServer( playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveOnClient();
			}
		}

		////

		private static void Receive( NetProtocolRequest data, int playerWho ) {
			Type genericArg = null;
			foreach( Type arg in data.GetType().BaseType.GetGenericArguments() ) {
				genericArg = arg;
				break;
			}
			if( genericArg == null ) {
				throw new ModHelpersException( "Invalid NetProtocolRequestPayload ("+data.GetType().Name+")" );
			}

			object rawReply = Activator.CreateInstance(
				genericArg,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				new object[] { },
				null
			);

			if( Main.netMode == NetmodeID.Server ) {
				var reply = rawReply as NetProtocolClientPayload;
				if( reply == null ) {
					throw new ModHelpersException( data.GetType().Name+" is not a NetProtocolRequestServerPayload" );
				}

				if( !ReflectionHelpers.RunMethod(
					data,
					"PreReply",
					new object[] { reply, playerWho },
					out object _
				) ) {
					throw new ModHelpersException( "Could not call PreReply for "+data.GetType().Name );
				}

				NetIO.SendToClients( reply );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				var reply = rawReply as NetProtocolServerPayload;
				if( reply == null ) {
					throw new ModHelpersException( data.GetType().Name+" is not a NetProtocolRequestClientPayload" );
				}

				if( !ReflectionHelpers.RunMethod(
					data,
					"PreReply",
					new object[] { reply, playerWho },
					out object _
				) ) {
					throw new ModHelpersException( "Could not call PreReply for "+data.GetType().Name );
				}

				NetIO.SendToServer( reply );
			}
		}
	}
}
