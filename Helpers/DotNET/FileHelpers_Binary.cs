using HamstarHelpers.Helpers.Debug;
using System.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNET {
	/// <summary>
	/// Assorted static "helper" functions pertaining to basic file IO.
	/// </summary>
	public partial class FileHelpers {
		/// <summary>
		/// Outputs a byte array to file as binary data.
		/// </summary>
		/// <param name="data">Object to output.</param>
		/// <param name="fullPath">System path to the file.</param>
		/// <param name="isCloud">Stores the file on the cloud.</param>
		/// <param name="backupOld">Writes any existing file to a .bak backup file, but still overrides the current file.</param>
		/// <returns>Returns `true` if file wrote successfully.</returns>
		public static bool SaveBinaryFile( byte[] data, string fullPath, bool isCloud, bool backupOld ) {
			string status;
			bool success = FileHelpers.SaveBinaryFile( data, fullPath, isCloud, backupOld, out status );

			if( !success ) {
				LogHelpers.Warn( status );
			}
			return success;
		}

		/// <summary>
		/// Outputs a byte array to file as binary data.
		/// </summary>
		/// <param name="data">Object to output.</param>
		/// <param name="fullPath">System path to the file.</param>
		/// <param name="isCloud">Stores the file on the cloud.</param>
		/// <param name="backupOld">Writes any existing file to a .bak backup file, but still overrides the current file.</param>
		/// <param name="status">Reports specific information on IO result's status.</param>
		/// <returns>Returns `true` if file wrote successfully.</returns>
		public static bool SaveBinaryFile( byte[] data, string fullPath, bool isCloud, bool backupOld, out string status ) {
			if( backupOld && FileUtilities.Exists( fullPath, isCloud ) ) {
				FileUtilities.Copy( fullPath, fullPath + ".bak", isCloud );
			}

			//string jsonStr = JsonConvert.SerializeObject( obj, obj.GetType(), jsonSettings );		JsonSerializerSettings jsonSettings

			if( isCloud ) {
				if( SocialAPI.Cloud != null ) {
					status = "Cloud API not available.";
					return false;
				}

				using( Stream memStream = (Stream)new MemoryStream() ) {
					StreamHelpers.ToStream( data, memStream );

					SocialAPI.Cloud.Write( fullPath, ( (MemoryStream)memStream ).ToArray() );
				}
			} else {
				using( Stream fileStream = (Stream)new FileStream( fullPath, FileMode.Create, FileAccess.Write ) ) {
					StreamHelpers.ToStream( data, fileStream );
				}
			}

			status = "Success.";
			return true;
		}


		////

		/// <summary>
		/// Loads a binary file into the given object type.
		/// </summary>
		/// <param name="fullPath">System path to the file.</param>
		/// <param name="isCloud">Stores the file on the cloud.</param>
		/// <returns>A byte array of the input file's data. `null` if file does not exist, or error.</returns>
		public static byte[] LoadBinaryFile( string fullPath, bool isCloud ) {
			string status;
			byte[] result = FileHelpers.LoadBinaryFile( fullPath, isCloud, out status );

			if( result == null ) {
				LogHelpers.Alert( status );
			}

			return result;
		}

		/// <summary>
		/// Loads a binary file into the given object type.
		/// </summary>
		/// <param name="fullPath">System path to the file.</param>
		/// <param name="isCloud">Stores the file on the cloud.</param>
		/// <param name="status">Reports specific information on IO result's status.</param>
		/// <returns>A byte array of the input file's data. `null` if file does not exist, or error.</returns>
		public static byte[] LoadBinaryFile( string fullPath, bool isCloud, out string status ) {
			if( !FileUtilities.Exists( fullPath, isCloud ) ) {
				status = "Binary file " + fullPath + " does not exist.";
				return null;
			}

			byte[] buf = FileUtilities.ReadAllBytes( fullPath, isCloud );
			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				status = "Invalid binary file.";
				return null;
			}

			using( var memStream = new MemoryStream( buf ) ) {
				status = "Success.";
				return StreamHelpers.FromStreamToBytes( memStream );
				//string jsonStr = StreamHelpers.FromStream( memStream );
				//return JsonConvert.DeserializeObject<T>( jsonStr, jsonSettings );
			}
		}
	}
}
