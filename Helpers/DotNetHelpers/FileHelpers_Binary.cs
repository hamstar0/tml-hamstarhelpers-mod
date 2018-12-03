using Newtonsoft.Json;
using System.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static partial class FileHelpers {
		public static bool SaveBinaryFile<T>( T obj, string fullPath, bool isCloud, bool backupOld, JsonSerializerSettings jsonSettings )
				where T : class {
			if( backupOld && FileUtilities.Exists( fullPath, isCloud ) ) {
				FileUtilities.Copy( fullPath, fullPath + ".bak", isCloud );
			}

			string jsonStr = JsonConvert.SerializeObject( obj, obj.GetType(), jsonSettings );

			if( isCloud ) {
				if( SocialAPI.Cloud != null ) { return false; }

				using( Stream memStream = (Stream)new MemoryStream() ) {
					FileHelpers.ToStream( jsonStr, memStream );
					SocialAPI.Cloud.Write( fullPath, ( (MemoryStream)memStream ).ToArray() );
				}
			} else {
				using( Stream fileStream = (Stream)new FileStream( fullPath, FileMode.Create, FileAccess.Write ) ) {
					FileHelpers.ToStream( jsonStr, fileStream );
				}
			}

			return true;
		}


		public static T LoadBinaryFile<T>( string fullPath, bool isCloud, JsonSerializerSettings jsonSettings )
				where T : class {
			if( !FileUtilities.Exists( fullPath, isCloud ) ) {
				return null;
			}

			byte[] buf = FileUtilities.ReadAllBytes( fullPath, isCloud );
			if( buf.Length < 1 || buf[0] != 0x1F || buf[1] != 0x8B ) {
				return null;
			}

			using( var memStream = new MemoryStream( buf ) ) {
				string jsonStr = FileHelpers.FromStream( memStream );
				return JsonConvert.DeserializeObject<T>( jsonStr, jsonSettings );
			}
		}


		////////////////

		public static bool SaveBinaryFile<T>( T obj, string fullPath, bool isCloud, bool backupOld )
				where T : class {
			return FileHelpers.SaveBinaryFile<T>( obj, fullPath, isCloud, backupOld, new JsonSerializerSettings() );
		}


		public static T LoadBinaryFile<T>( string fullPath, bool isCloud )
				where T : class {
			return FileHelpers.LoadBinaryFile<T>( fullPath, isCloud, new JsonSerializerSettings() );
		}
	}
}
