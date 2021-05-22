using System.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Libraries.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to basic file IO.
	/// </summary>
	public partial class FileLibraries {
		/// <summary>
		/// Saves string data to a text file.
		/// </summary>
		/// <param name="data">Text file's data.</param>
		/// <param name="fullPath">System path to file.</param>
		/// <param name="isCloud">Indicates to save on the cloud.</param>
		/// <param name="backupOld">Performs a backup of any existing file (with .bak extension).</param>
		/// <returns>`true` on success.</returns>
		public static bool SaveTextFile( string data, string fullPath, bool isCloud, bool backupOld ) {
			if( backupOld && FileUtilities.Exists( fullPath, isCloud ) ) {
				FileUtilities.Copy( fullPath, fullPath + ".bak", isCloud );
			}

			if( isCloud ) {
				if( SocialAPI.Cloud != null ) { return false; }

				File.WriteAllText( fullPath, data );

				using( var memStream = (Stream)new MemoryStream() ) {
					byte[] bytes = System.Text.Encoding.UTF8.GetBytes( data );

					memStream.Write( bytes, 0, data.Length );

					SocialAPI.Cloud.Write( fullPath, ( (MemoryStream)memStream ).ToArray() );
				}
			} else {
				File.WriteAllText( fullPath, data );
			}

			return true;
		}


		/// <summary>
		/// Gets a text file.
		/// </summary>
		/// <param name="fullPath">System path to text file.</param>
		/// <param name="isCloud">Indicates to look on the cloud.</param>
		/// <returns>String data of text file encoded as UTF8.</returns>
		public static string LoadTextFile( string fullPath, bool isCloud ) {
			if( !FileUtilities.Exists( fullPath, isCloud ) ) {
				return null;
			}

			//return File.ReadAllText( fullPath );
			byte[] buf = FileUtilities.ReadAllBytes( fullPath, isCloud );

			return System.Text.Encoding.UTF8.GetString( buf );
			/*using( Stream fileStream = (Stream)new FileStream( fullPath, FileMode.Open ) ) {
				using( StreamReader fileReader = new StreamReader( fileStream ) ) {
					return fileReader.ReadToEnd();
				}
			}*/
		}
	}
}
