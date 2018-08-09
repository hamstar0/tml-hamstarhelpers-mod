using Newtonsoft.Json;
using System.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static partial class FileHelpers {
		public static bool SaveTextFile( string data, string full_path, bool is_cloud, bool backup_old ) {
			if( backup_old && FileUtilities.Exists( full_path, is_cloud ) ) {
				FileUtilities.Copy( full_path, full_path + ".bak", is_cloud );
			}

			if( is_cloud ) {
				if( SocialAPI.Cloud != null ) { return false; }

				using( Stream mem_stream = (Stream)new MemoryStream() ) {
					FileHelpers.ToStream( data, mem_stream );
					SocialAPI.Cloud.Write( full_path, ( (MemoryStream)mem_stream ).ToArray() );
				}
			} else {
				using( Stream file_stream = (Stream)new FileStream( full_path, FileMode.Create, FileAccess.Write ) ) {
					FileHelpers.ToStream( data, file_stream );
				}
			}

			return true;
		}


		public static string LoadTextFile( string full_path, bool is_cloud ) {
			if( !FileUtilities.Exists( full_path, is_cloud ) ) {
				return null;
			}

			byte[] buf = FileUtilities.ReadAllBytes( full_path, is_cloud );
			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				return null;
			}

			using( var mem_stream = new MemoryStream( buf ) ) {
				return FileHelpers.FromStream( mem_stream );
			}
		}
	}
}
