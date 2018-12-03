using System;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static partial class FileHelpers {
		public static string SanitizePath( string path ) {
			char[] invChars = Path.GetInvalidFileNameChars();
			string[] splits = path.Split( invChars );
			if( splits.Length == 1 ) { return path; }

			return String.Concat( splits );
		}


		////////////////

		public static void ToStream( string src, Stream destStream ) {
			var zipStream = new GZipStream( destStream, CompressionMode.Compress, true );
			
			var destWriter = new BigEndianWriter( zipStream );
			destWriter.Write( src );

			zipStream.Close();
		}

		public static string FromStream( Stream srcStream ) {
			using( var zipStream = new GZipStream( srcStream, CompressionMode.Decompress ) ) {
				var streamReader = new BigEndianReader( zipStream );
				return streamReader.ReadString();
			}
		}
	}
}
