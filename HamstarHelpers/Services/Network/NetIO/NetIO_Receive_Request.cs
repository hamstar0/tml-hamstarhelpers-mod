using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
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
		private static void ReceiveRequest( NetIORequest data, int playerWho ) {
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof( IsNoisyAttribute ), false );

				if( !isNoisy ) {
					LogHelpers.Log( "<" + dataType.Name );
				}
			}

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
				var toClientReply = rawReply as NetIOClientPayload;
				if( toClientReply != null ) {
					NetIO.ProcessRequestOnServer( data, playerWho, toClientReply );
					return;
				}

				var toBiClientReply = rawReply as NetIOBidirectionalPayload;
				if( toBiClientReply != null ) {
					NetIO.ProcessRequestOnServer( data, playerWho, toBiClientReply );
					return;
				}

				throw new ModHelpersException( data.GetType().Name + " is not a valid client NetProtocolRequest"
					+" (indicates reply of "+(rawReply?.GetType().Name ?? "unknown type")+")" );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				var toServerReply = rawReply as NetIOServerPayload;
				if( toServerReply != null ) {
					NetIO.ProcessRequestOnClient( data, toServerReply );
					return;
				}

				var toBiServerReply = rawReply as NetIOBidirectionalPayload;
				if( toBiServerReply != null ) {
					NetIO.ProcessRequestOnClient( data, toBiServerReply );
					return;
				}

				throw new ModHelpersException( data.GetType().Name + " is not a valid server NetProtocolRequest"
					+ " (indicates reply of " + (rawReply?.GetType().Name ?? "unknown type") + ")" );
			}
		}


		////////////////

		private static void ProcessRequestOnServer( NetIORequest data, int playerWho, NetIOClientPayload reply ) {
			bool success;

			if( !ReflectionHelpers.RunMethod(
				data,
				"PreReplyOnServer",
				new object[] { reply, playerWho },
				out success
			) ) {
				throw new ModHelpersException( "Could not call PreReplyOnServer for "
					+ data.GetType().Name
					+ " with payload " + reply.GetType().Name
				);
			}

			if( success ) {
				NetIO.SendToClients( reply, playerWho, -1 );
			}
		}

		private static void ProcessRequestOnServer(
					NetIORequest data,
					int playerWho,
					NetIOBidirectionalPayload reply ) {
			bool success;

			if( !ReflectionHelpers.RunMethod(
				data,
				"PreReplyOnServer",
				new object[] { reply, playerWho },
				out success
			) ) {
				throw new ModHelpersException( "Could not call PreReplyOnServer for "
					+ data.GetType().Name
					+ " with payload " + reply.GetType().Name
				);
			}

			if( success ) {
				NetIO.SendToClients( reply, playerWho, -1 );
			}
		}

		////

		private static void ProcessRequestOnClient( NetIORequest data, NetIOServerPayload reply ) {
			bool success;

			if( !ReflectionHelpers.RunMethod(
				data,
				"PreReplyOnClient",
				new object[] { reply },
				out success
			) ) {
				throw new ModHelpersException( "Could not call PreReplyOnClient for "
					+ data.GetType().Name
					+ " with payload " + reply.GetType().Name
				);
			}

			if( success ) {
				NetIO.SendToServer( reply );
			}
		}

		private static void ProcessRequestOnClient( NetIORequest data, NetIOBidirectionalPayload reply ) {
			bool success;

			if( !ReflectionHelpers.RunMethod(
				data,
				"PreReplyOnClient",
				new object[] { reply },
				out success
			) ) {
				throw new ModHelpersException( "Could not call PreReplyOnClient for "
					+ data.GetType().Name
					+ " with payload " + reply.GetType().Name
				);
			}

			if( success ) {
				NetIO.SendToServer( reply );
			}
		}
	}
}
