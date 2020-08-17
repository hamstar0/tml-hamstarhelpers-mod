using System;
using System.IO;
using System.Reflection;
using Terraria;
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
		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="playerWho"></param>
		/// <returns>`true` to be considered to have completely handled the incoming packet.</returns>
		internal static bool HandlePacket( BinaryReader reader, int playerWho ) {
			long oldStreamPos = reader.BaseStream.Position;
			object data = null;

			try {
				if( !NetIO.DeserializeStream( reader, out data ) ) {
					reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );
					return false;
				}
			} catch( Exception e ) {
				LogHelpers.Warn( e.Message );
				reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );
				return false;
			}

			if( data == null || data.GetType() == typeof( object ) ) {
				return false;
			}

			if( data is NetIOBroadcastPayload ) {
				NetIO.Receive( data as NetIOBroadcastPayload, playerWho );
				return true;
			}
			if( data is NetIOServerPayload ) {
				NetIO.Receive( data as NetIOServerPayload, playerWho );
				return true;
			}
			if( data is NetIOClientPayload ) {
				NetIO.Receive( data as NetIOClientPayload );
				return true;
			}
			if( data is NetIOBidirectionalPayload ) {
				NetIO.Receive( data as NetIOBidirectionalPayload, playerWho );
				return true;
			}
			if( data is NetIORequest ) {
				NetIO.ReceiveRequest( data as NetIORequest, playerWho );
				return true;
			}

			reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );
			return false;
		}


		private static bool DeserializeStream( BinaryReader reader, out object data ) {
			var netIO = ModContent.GetInstance<NetIO>();

			try {
				int code = reader.ReadInt32();
				if( !netIO.PayloadCodeToType.TryGetValue(code, out Type type) ) {
					data = null;
					return false;
				}

				Type dataType = netIO.PayloadCodeToType[ code ];
				Serializer ser = netIO.PayloadCodeToSerializer[ code ];

				MethodInfo method = ser.GetType().GetMethod( "DeserializeDirect", ReflectionHelpers.MostAccess );
				method = method.MakeGenericMethod( new Type[] { dataType } );

				var parameters = new object[] { reader.BaseStream, null };
				method.Invoke( ser, parameters );

				data = parameters[1];
				return true;
			} catch( Exception e ) {
				LogHelpers.Warn( e.Message );

				data = null;
				return false;
			}
		}
	}
}
