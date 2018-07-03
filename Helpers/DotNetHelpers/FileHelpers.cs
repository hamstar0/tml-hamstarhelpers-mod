using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static class FileHelpers {
		public static string SanitizePath( string path ) {
			char[] inv_chars = Path.GetInvalidFileNameChars();
			string[] splits = path.Split( inv_chars );
			if( splits.Length == 1 ) { return path; }

			return String.Concat( splits );
		}

		////////////////

		public static bool SaveBinaryFile<T>( T obj, string full_path, bool is_cloud, bool backup_old )
				where T : class {
			if( backup_old && FileUtilities.Exists( full_path, is_cloud ) ) {
				FileUtilities.Copy( full_path, full_path + ".bak", is_cloud );
			}

			string json_str = JsonConvert.SerializeObject( obj, obj.GetType(), new JsonSerializerSettings() );
			
			if( is_cloud ) {
				if( SocialAPI.Cloud != null ) { return false; }

				using( Stream mem_stream = (Stream)new MemoryStream() ) {
					FileHelpers.ToStream( json_str, mem_stream );
					SocialAPI.Cloud.Write( full_path, ( (MemoryStream)mem_stream ).ToArray() );
				}
			} else {
				using( Stream file_stream = (Stream)new FileStream( full_path, FileMode.Create, FileAccess.Write ) ) {
					FileHelpers.ToStream( json_str, file_stream );
				}
			}

			return true;
		}


		public static T LoadBinaryFile<T>( string full_path, bool is_cloud )
				where T : class {
			if( !FileUtilities.Exists( full_path, is_cloud ) ) {
				return null;
			}

			byte[] buf = FileUtilities.ReadAllBytes( full_path, is_cloud );
			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				return null;
			}

			using( var mem_stream = new MemoryStream( buf ) ) {
				string json_str = FileHelpers.FromStream( mem_stream );
				return JsonConvert.DeserializeObject<T>( json_str );
			}
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
