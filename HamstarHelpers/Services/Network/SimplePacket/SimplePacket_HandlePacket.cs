using System;
using System.IO;
using System.Reflection;
using Terraria;
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
		/// <summary></summary>
		/// <param name="reader"></param>
		/// <param name="playerWho"></param>
		/// <returns>`true` to be considered to have completely handled the incoming packet.</returns>
		internal static bool HandlePacket( BinaryReader reader, int playerWho ) {
			long oldStreamPos = reader.BaseStream.Position;
			object data = null;

			try {
				if( !SimplePacket.DeserializeStream( reader, out data ) ) {
					// Reset stream for other handlers:
					reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );

					return false;
				}
			} catch( Exception e ) {
				LogLibraries.Warn( e.Message );

				// Reset stream for other handlers:
				reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );

				return false;
			}

			if( data == null || !(data is SimplePacketPayload ) ) {
				// Reset stream for other handlers:
				reader.BaseStream.Seek( oldStreamPos, SeekOrigin.Begin );

				return false;
			}

			SimplePacket.Receive( (SimplePacketPayload)data, playerWho );

			return true;
		}


		private static bool DeserializeStream( BinaryReader reader, out object data ) {
			var self = ModContent.GetInstance<SimplePacket>();

			try {
				int code = reader.ReadInt32();
				if( !self.PayloadCodeToType.TryGetValue(code, out Type type) ) {
					data = null;
					return false;
				}

				Type dataType = self.PayloadCodeToType[ code ];
				Serializer ser = self.PayloadCodeToSerializer[ code ];

				MethodInfo method = ser.GetType().GetMethod( "DeserializeDirect", ReflectionLibraries.MostAccess );
				method = method.MakeGenericMethod( new Type[] { dataType } );

				var parameters = new object[] { reader.BaseStream, null };
				method.Invoke( ser, parameters );

				data = parameters[1];
				return true;
			} catch( Exception e ) {
				LogLibraries.Warn( e.Message );

				data = null;
				return false;
			}
		}
	}
}
