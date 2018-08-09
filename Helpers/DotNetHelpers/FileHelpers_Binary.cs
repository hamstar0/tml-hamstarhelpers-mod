using Newtonsoft.Json;
using System.IO;
using Terraria.Social;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static partial class FileHelpers {
		public static bool SaveBinaryFile<T>( T obj, string full_path, bool is_cloud, bool backup_old, JsonSerializerSettings json_settings )
				where T : class {
			if( backup_old && FileUtilities.Exists( full_path, is_cloud ) ) {
				FileUtilities.Copy( full_path, full_path + ".bak", is_cloud );
			}

			string json_str = JsonConvert.SerializeObject( obj, obj.GetType(), json_settings );

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


		public static T LoadBinaryFile<T>( string full_path, bool is_cloud, JsonSerializerSettings json_settings )
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
				return JsonConvert.DeserializeObject<T>( json_str, json_settings );
			}
		}


		////////////////

		public static bool SaveBinaryFile<T>( T obj, string full_path, bool is_cloud, bool backup_old )
				where T : class {
			return FileHelpers.SaveBinaryFile<T>( obj, full_path, is_cloud, backup_old, new JsonSerializerSettings() );
		}


		public static T LoadBinaryFile<T>( string full_path, bool is_cloud )
				where T : class {
			return FileHelpers.LoadBinaryFile<T>( full_path, is_cloud, new JsonSerializerSettings() );
		}
	}
}
