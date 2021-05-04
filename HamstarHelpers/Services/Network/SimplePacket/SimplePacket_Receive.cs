using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Services.Network.SimplePacket {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization.
	/// </summary>
	public partial class SimplePacket : ILoadable {
		private static void Receive( SimplePacketPayload data, int playerWho ) {
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
			} else {
				throw new ModHelpersException( "Not MP" );
			}
		}
	}
}
