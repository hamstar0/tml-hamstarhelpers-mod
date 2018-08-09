using System;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static partial class FileHelpers {
		public static string SanitizePath( string path ) {
			char[] inv_chars = Path.GetInvalidFileNameChars();
			string[] splits = path.Split( inv_chars );
			if( splits.Length == 1 ) { return path; }

			return String.Concat( splits );
		}


		////////////////

		public static void ToStream( string src, Stream dest_stream ) {
			var zip_stream = new GZipStream( dest_stream, CompressionMode.Compress, true );
			
			var dest_writer = new BigEndianWriter( zip_stream );
			dest_writer.Write( src );

			zip_stream.Close();
		}

		public static string FromStream( Stream src_stream ) {
			using( var zip_stream = new GZipStream( src_stream, CompressionMode.Decompress ) ) {
				var stream_reader = new BigEndianReader( zip_stream );
				return stream_reader.ReadString();
			}
		}
	}
}
