using HamstarHelpers.Helpers;
using HamstarHelpers.Utilities.Config;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.MiscHelpers {
	public class DataFileHelpers {
		public static string BaseFolder { get { return "Mod Specific Data"; } }



		////////////////

		public static string GetRelativeDirectoryPath( Mod mod ) {
			return DataFileHelpers.BaseFolder + Path.DirectorySeparatorChar + mod.Name;
		}

		public static string GetFullDirectoryPath( Mod mod ) {
			return Main.SavePath + Path.DirectorySeparatorChar + DataFileHelpers.GetRelativeDirectoryPath(mod);
		}


		private static void PrepareDir( Mod mod, string name ) {
			string rel_dir = DataFileHelpers.GetRelativeDirectoryPath( mod );
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

		public static T LoadJson<T>( Mod mod, string name_only, out bool success ) where T : class {
			DataFileHelpers.PrepareDir( mod, name_only );

			string rel_dir = DataFileHelpers.GetRelativeDirectoryPath( mod );
			success = false;

			try {
				var json_file = new JsonConfig<T>( name_only + ".json", rel_dir );
				success = json_file.LoadFile();

				return json_file.Data;
			} catch( IOException e ) {
				string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
				throw new IOException( "Failed to load json file " + name_only + " at " + full_dir, e );
			}
		}

		public static T LoadBinary<T>( Mod mod, string name_only, out bool success ) where T : class {
			DataFileHelpers.PrepareDir( mod, name_only );

			string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );
			success = false;

			try {
				return FileHelpers.LoadBinaryFile<T>( full_dir, false );
			} catch( IOException e ) {
				throw new IOException( "Failed to load binary file "+name_only+" at " + full_dir, e );
			}
		}

		////////////////
		
		public static void SaveAsJson<T>( Mod mod, string name_only, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod, name_only );

			string rel_dir = DataFileHelpers.GetRelativeDirectoryPath( mod );

			try {
				var json_file = new JsonConfig<T>( name_only + ".json", rel_dir, data );
				json_file.SaveFile();
			} catch( IOException e ) {
				throw new IOException( "Failed to save json file " + name_only + " at " + rel_dir, e );
			}
		}

		public static void SaveAsBinary<T>( Mod mod, string name_only, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod, name_only );

			string full_dir = DataFileHelpers.GetFullDirectoryPath( mod );

			try {
				FileHelpers.SaveBinaryFile<T>( data, full_dir, false, false );
			} catch( IOException e ) {
				throw new IOException( "Failed to save binary file " + name_only + " at " + full_dir, e );
			}
		}
	}
}
