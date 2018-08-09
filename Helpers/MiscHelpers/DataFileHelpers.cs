using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DotNetHelpers;
using Newtonsoft.Json;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public partial class DataFileHelpers {
		public static string BaseFolder { get { return "Mod Specific Data"; } }



		////////////////

		public static string GetRelativeDirectoryPath( Mod mod ) {
			return DataFileHelpers.BaseFolder + Path.DirectorySeparatorChar + mod.Name;
		}

		public static string GetFullDirectoryPath( Mod mod ) {
			return Main.SavePath + Path.DirectorySeparatorChar + DataFileHelpers.GetRelativeDirectoryPath(mod);
		}

		public static string GetFullPath( Mod mod, string file_name_has_ext ) {
			return DataFileHelpers.GetFullDirectoryPath( mod ) + Path.DirectorySeparatorChar + file_name_has_ext;
		}

		////////////////

		private static void PrepareDir( Mod mod ) {
			string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
			
			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + DataFileHelpers.BaseFolder );
				Directory.CreateDirectory( full_dir );
			} catch( IOException e ) {
				throw new IOException( "Failed to prepare directory: " + full_dir, e );
			}
		}


		////////////////

		public static T LoadJson<T>( Mod mod, string file_name_no_ext, JsonSerializerSettings json_settings, out bool success ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string rel_dir = DataFileHelpers.GetRelativeDirectoryPath( mod );
			success = false;

			try {
				var json_file = new JsonConfig<T>( file_name_no_ext + ".json", rel_dir, json_settings );
				success = json_file.LoadFile();

				return json_file.Data;
			} catch( IOException e ) {
				string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
				throw new IOException( "Failed to load json file " + file_name_no_ext + " at " + full_dir, e );
			}
		}

		public static T LoadBinary<T>( Mod mod, string file_name_has_ext, bool is_cloud, JsonSerializerSettings json_settings ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string full_path = DataFileHelpers.GetFullPath( mod, file_name_has_ext );

			try {
				return FileHelpers.LoadBinaryFile<T>( full_path, is_cloud, json_settings );
			} catch( IOException e ) {
				string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
				throw new IOException( "Failed to load binary file "+file_name_has_ext+" at " + full_dir, e );
			}
		}

		////////////////
		
		public static void SaveAsJson<T>( Mod mod, string file_name_no_ext, JsonSerializerSettings json_settings, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string rel_dir = DataFileHelpers.GetRelativeDirectoryPath( mod );

			try {
				var json_file = new JsonConfig<T>( file_name_no_ext + ".json", rel_dir, data, json_settings );
				json_file.SaveFile();
			} catch( IOException e ) {
				throw new IOException( "Failed to save json file " + file_name_no_ext + " at " + rel_dir, e );
			}
		}

		public static void SaveAsBinary<T>( Mod mod, string file_name_has_ext, bool is_cloud, JsonSerializerSettings json_settings, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string full_path = DataFileHelpers.GetFullPath( mod, file_name_has_ext );

			try {
				FileHelpers.SaveBinaryFile<T>( data, full_path, is_cloud, false, json_settings );
			} catch( IOException e ) {
				string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
				throw new IOException( "Failed to save binary file " + file_name_has_ext + " at " + full_dir, e );
			}
		}


		////////////////

		public static T LoadJson<T>( Mod mod, string file_name_no_ext, out bool success ) where T : class {
			return DataFileHelpers.LoadJson<T>( mod, file_name_no_ext, new JsonSerializerSettings(), out success );
		}

		public static T LoadBinary<T>( Mod mod, string file_name_has_ext, bool is_cloud ) where T : class {
			return DataFileHelpers.LoadBinary<T>( mod, file_name_has_ext, is_cloud, new JsonSerializerSettings() );
		}

		////////////////

		public static void SaveAsJson<T>( Mod mod, string file_name_no_ext, T data ) where T : class {
			DataFileHelpers.SaveAsJson<T>( mod, file_name_no_ext, new JsonSerializerSettings(), data );
		}

		public static void SaveAsBinary<T>( Mod mod, string file_name_has_ext, bool is_cloud, T data ) where T : class {
			DataFileHelpers.SaveAsBinary<T>( mod, file_name_has_ext, is_cloud, new JsonSerializerSettings(), data );
		}
	}
}
