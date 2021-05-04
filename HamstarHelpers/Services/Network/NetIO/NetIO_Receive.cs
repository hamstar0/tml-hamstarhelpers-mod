using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.NetIO {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization and
	/// routing.
	/// </summary>
	public partial class NetIO : ILoadable {
		private static void Receive( NetIOBroadcastPayload data, int playerWho ) {
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof( IsNoisyAttribute ), false );

				if( !isNoisy ) {
					LogHelpers.Log( "<" + dataType.Name );
				}
			}

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

		private static void Receive( NetIOServerPayload data, int playerWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof(IsNoisyAttribute), false );

				if( !isNoisy ) {
					LogHelpers.Log( "<" + dataType.Name );
				}
			}

			data.ReceiveOnServer( playerWho );
		}

		private static void Receive( NetIOClientPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof( IsNoisyAttribute ), false );

				if( !isNoisy ) {
					LogHelpers.Log( "<" + dataType.Name );
				}
			}

			data.ReceiveOnClient();
		}

		private static void Receive( NetIOBidirectionalPayload data, int playerWho ) {
			if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
				Type dataType = data.GetType();
				bool isNoisy = dataType
					.IsDefined( typeof( IsNoisyAttribute ), false );

				if( !isNoisy ) {
					LogHelpers.Log( "<" + dataType.Name );
				}
			}

			if( Main.netMode == NetmodeID.Server ) {
				data.ReceiveOnServer( playerWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				data.ReceiveOnClient();
			}
		}
	}
}
