using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NetSerializer;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;


namespace HamstarHelpers.Services.Network.SimplePacket {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization.
	/// </summary>
	public partial class SimplePacket : ILoadable {
		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="data"></param>
		public static void SendToServer( SimplePacketPayload data ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}
			SimplePacket.Send( data, -1, -1 );
		}

		////

		/// <summary>
		/// Sends the data to the specified client(s).
		/// </summary>
		/// <param name="data"></param>
		/// <param name="toWho">Main.player array index of player (`player.whoAmI`) to send to. -1 for all players.</param>
		/// <param name="ignoreWho">Main.player array index of player (`player.whoAmI`) to ignore. -1 for no one.</param>
		public static void SendToClient( SimplePacketPayload data, int toWho = -1, int ignoreWho = -1 ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}
			SimplePacket.Send( data, toWho, ignoreWho );
		}


		////////////////

		private static bool Send( SimplePacketPayload data, int toWho, int ignoreWho ) {
			var netIO = ModContent.GetInstance<SimplePacket>();
			ModPacket packet = ModHelpersMod.Instance.GetPacket();

			try {
				Type dataType = data.GetType();
				if( !netIO.PayloadTypeToCode.TryGetValue(dataType, out int code) ) {
					return false;
				}

				Serializer ser = netIO.PayloadCodeToSerializer[ code ];

				packet.Write( (int)code );

				MethodInfo method = ser.GetType().GetMethod( "SerializeDirect", ReflectionLibraries.MostAccess );
				method = method.MakeGenericMethod( new Type[] { dataType } );
				method.Invoke( ser, new object[] { packet.BaseStream, data } );

				packet.Send( toWho, ignoreWho );

				if( ModHelpersConfig.Instance.DebugModeNetInfo ) {
					bool isNoisy = dataType
						.IsDefined( typeof(IsNoisyAttribute), false );

					if( !isNoisy ) {
						LogLibraries.Log( ">" + dataType.Name + " "+toWho+" "+ignoreWho );
					}
				}

				return true;
			} catch( Exception e ) {
				LogLibraries.WarnOnce( e.ToString() );
			}
			
			return false;
		}
	}
}
