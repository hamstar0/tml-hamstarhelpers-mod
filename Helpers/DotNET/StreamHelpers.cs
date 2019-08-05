using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to basic stream IO.
	/// </summary>
	public partial class StreamHelpers {
		/// <summary>
		/// Pumps a byte array to a stream (with compression).
		/// </summary>
		/// <param name="src"></param>
		/// <param name="destStream"></param>
		public static void ToStream( byte[] src, Stream destStream ) {
			var zipStream = new GZipStream( destStream, CompressionMode.Compress, true );

			var destWriter = new BigEndianWriter( zipStream );
			destWriter.Write( src );

			zipStream.Close();
		}

		/// <summary>
		/// Pumps a string to a stream (with compression).
		/// </summary>
		/// <param name="src"></param>
		/// <param name="destStream"></param>
		public static void ToStream( string src, Stream destStream ) {
			var zipStream = new GZipStream( destStream, CompressionMode.Compress, true );
			
			var destWriter = new BigEndianWriter( zipStream );
			destWriter.Write( src );

			zipStream.Close();
		}


		////////////////

		/// <summary>
		/// Pulls a byte array from a compressed stream.
		/// </summary>
		/// <param name="srcStream"></param>
		/// <returns></returns>
		public static byte[] FromStreamToBytes( Stream srcStream ) {
			using( var zipStream = new GZipStream( srcStream, CompressionMode.Decompress ) ) {
				var streamReader = new BigEndianReader( zipStream );
				return streamReader.ReadAllBytes();
			}
		}

		/// <summary>
		/// Pulls a string from a compressed stream.
		/// </summary>
		/// <param name="srcStream"></param>
		/// <returns></returns>
		public static string FromStreamToString( Stream srcStream ) {
			using( var zipStream = new GZipStream( srcStream, CompressionMode.Decompress ) ) {
				var streamReader = new BigEndianReader( zipStream );
				return streamReader.ReadString();
			}
		}
	}
}
