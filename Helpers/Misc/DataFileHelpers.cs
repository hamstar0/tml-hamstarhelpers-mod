using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Misc {
	public partial class DataFileHelpers {
		public static string BaseFolder => "Mod Specific Data";



		////////////////

		public static string GetRelativeDirectoryPath( Mod mod ) {
			return DataFileHelpers.BaseFolder + Path.DirectorySeparatorChar + mod.Name;
		}

		public static string GetFullDirectoryPath( Mod mod ) {
			return Main.SavePath + Path.DirectorySeparatorChar + DataFileHelpers.GetRelativeDirectoryPath(mod);
		}

		public static string GetFullPath( Mod mod, string fileNameHasExt ) {
			return DataFileHelpers.GetFullDirectoryPath( mod ) + Path.DirectorySeparatorChar + fileNameHasExt;
		}

		////////////////

		private static void PrepareDir( Mod mod ) {
			string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
			
			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + DataFileHelpers.BaseFolder );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to prepare directory: " + fullDir+" - "+e.ToString() );
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
			}
		}


		////////////////

		public static T LoadJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings, out bool success ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string relDir = DataFileHelpers.GetRelativeDirectoryPath( mod );
			success = false;

			try {
				var jsonFile = new JsonConfig<T>( fileNameNoExt + ".json", relDir, jsonSettings );
				success = jsonFile.LoadFile();

				return jsonFile.Data;
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load json file " + fileNameNoExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load json file " + fileNameNoExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new HamstarException( "From "+fileNameNoExt+" ("+typeof(T).Name+")", e );
			}
		}

		public static T LoadBinary<T>( Mod mod, string fileNameHasExt, bool isCloud, JsonSerializerSettings jsonSettings ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string fullPath = DataFileHelpers.GetFullPath( mod, fileNameHasExt );

			try {
				return FileHelpers.LoadBinaryFile<T>( fullPath, isCloud, jsonSettings );
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load binary file " + fileNameHasExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load binary file "+fileNameHasExt+" at " + fullDir, e );
			}
		}

		////////////////
		
		public static void SaveAsJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string relDir = DataFileHelpers.GetRelativeDirectoryPath( mod );

			if( data == null ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - Data is null." );
				return;
			}

			try {
				var jsonFile = new JsonConfig<T>( fileNameNoExt + ".json", relDir, data, jsonSettings );
				jsonFile.SaveFile();
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + fileNameNoExt + " at " + relDir, e );
			}
		}

		public static void SaveAsBinary<T>( Mod mod, string fileNameHasExt, bool isCloud, JsonSerializerSettings jsonSettings, T data ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			string fullPath = DataFileHelpers.GetFullPath( mod, fileNameHasExt );

			if( data == null ) {
				LogHelpers.Warn( "Failed to save binary file " + fullPath + " - Data is null." );
				return;
			}

			try {
				FileHelpers.SaveBinaryFile<T>( data, fullPath, isCloud, false, jsonSettings );
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to save binary file " + fileNameHasExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to save binary file " + fileNameHasExt + " at " + fullDir, e );
			}
		}


		////////////////

		public static T LoadJson<T>( Mod mod, string fileNameNoExt, out bool success ) where T : class {
			return DataFileHelpers.LoadJson<T>( mod, fileNameNoExt, new JsonSerializerSettings(), out success );
		}

		public static T LoadBinary<T>( Mod mod, string fileNameHasExt, bool isCloud ) where T : class {
			return DataFileHelpers.LoadBinary<T>( mod, fileNameHasExt, isCloud, new JsonSerializerSettings() );
		}

		////////////////

		public static void SaveAsJson<T>( Mod mod, string fileNameNoExt, T data ) where T : class {
			DataFileHelpers.SaveAsJson<T>( mod, fileNameNoExt, new JsonSerializerSettings(), data );
		}

		public static void SaveAsBinary<T>( Mod mod, string fileNameHasExt, bool isCloud, T data ) where T : class {
			DataFileHelpers.SaveAsBinary<T>( mod, fileNameHasExt, isCloud, new JsonSerializerSettings(), data );
		}
	}
}
