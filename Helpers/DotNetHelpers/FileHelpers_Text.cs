using Newtonsoft.Json;
using System.IO;
using System.Text;
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

				File.WriteAllText( full_path, data );

				using( var mem_stream = (Stream)new MemoryStream() ) {
					byte[] bytes = Encoding.ASCII.GetBytes( data );

					mem_stream.Write( bytes, 0, data.Length );

					SocialAPI.Cloud.Write( full_path, ( (MemoryStream)mem_stream ).ToArray() );
				}
			} else {
				File.WriteAllText( full_path, data );
			}

			return true;
		}


		public static string LoadTextFile( string full_path, bool is_cloud ) {
			if( !FileUtilities.Exists( full_path, is_cloud ) ) {
				return null;
			}

			//return File.ReadAllText( full_path );
			byte[] buf = FileUtilities.ReadAllBytes( full_path, is_cloud );

			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				return null;
			}

			return Encoding.Default.GetString( buf );
			/*using( Stream file_stream = (Stream)new FileStream( full_path, FileMode.Open ) ) {
				using( StreamReader file_reader = new StreamReader( file_stream ) ) {
					return file_reader.ReadToEnd();
				}
			}*/
		}
	}
}
