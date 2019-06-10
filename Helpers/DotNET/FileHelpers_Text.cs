using Newtonsoft.Json;
using System.IO;
using System.Text;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNET {
	public static partial class FileHelpers {
		public static bool SaveTextFile( string data, string fullPath, bool isCloud, bool backupOld ) {
			if( backupOld && FileUtilities.Exists( fullPath, isCloud ) ) {
				FileUtilities.Copy( fullPath, fullPath + ".bak", isCloud );
			}

			if( isCloud ) {
				if( SocialAPI.Cloud != null ) { return false; }

				File.WriteAllText( fullPath, data );

				using( var memStream = (Stream)new MemoryStream() ) {
					byte[] bytes = Encoding.UTF8.GetBytes( data );

					memStream.Write( bytes, 0, data.Length );

					SocialAPI.Cloud.Write( fullPath, ( (MemoryStream)memStream ).ToArray() );
				}
			} else {
				File.WriteAllText( fullPath, data );
			}

			return true;
		}


		public static string LoadTextFile( string fullPath, bool isCloud ) {
			if( !FileUtilities.Exists( fullPath, isCloud ) ) {
				return null;
			}

			//return File.ReadAllText( fullPath );
			byte[] buf = FileUtilities.ReadAllBytes( fullPath, isCloud );

			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				return null;
			}

			return Encoding.UTF8.GetString( buf );
			/*using( Stream fileStream = (Stream)new FileStream( fullPath, FileMode.Open ) ) {
				using( StreamReader fileReader = new StreamReader( fileStream ) ) {
					return fileReader.ReadToEnd();
				}
			}*/
		}
	}
}
