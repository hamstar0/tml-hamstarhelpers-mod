using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to file IO for mod custom data.
	/// </summary>
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

		public static T LoadJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings )
				where T : class {
			DataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = DataFileHelpers.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataStr = FileHelpers.LoadTextFile( fullPath, false );

				if( dataStr != null ) {
					return JsonConvert.DeserializeObject<T>( dataStr, jsonSettings );
				} else {
					return null;
				}
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load json file " + fileNameNoExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load json file " + fileNameNoExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new HamstarException( "From "+fileNameNoExt+" ("+typeof(T).Name+")", e );
			}
		}

		public static T LoadJson<T>( Mod mod, string fileNameNoExt ) where T : class {
			return DataFileHelpers.LoadJson<T>( mod, fileNameNoExt, new JsonSerializerSettings() );
		}


		////////////////

		public static bool SaveAsJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings, bool overrides, T data )
				where T : class {
			DataFileHelpers.PrepareDir( mod );

			string relDir = DataFileHelpers.GetRelativeDirectoryPath( mod );

			if( data == null ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - Data is null." );
				return false;
			}

			try {
				string fullPath = DataFileHelpers.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );

				return FileHelpers.SaveTextFile( dataJson, fullPath, false, !overrides );
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + fileNameNoExt + " at " + relDir, e );
			}
		}

		public static bool SaveAsJson<T>( Mod mod, string fileNameNoExt, bool overrides, T data ) where T : class {
			return DataFileHelpers.SaveAsJson<T>( mod, fileNameNoExt, new JsonSerializerSettings(), overrides, data );
		}


		////////////////
		
		public static T LoadBinaryJson<T>( Mod mod, string fileNameWithExt, JsonSerializerSettings jsonSettings ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = DataFileHelpers.GetFullPath( mod, fileNameWithExt );
				byte[] dataBytes = FileHelpers.LoadBinaryFile( fullPath, false );
				string dataJson = System.Text.Encoding.UTF8.GetString( dataBytes );

				if( dataBytes != null ) {
					return JsonConvert.DeserializeObject<T>( dataJson, jsonSettings );
				} else {
					return null;
				}
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load binary file " + fileNameWithExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new HamstarException( "From " + fileNameWithExt + " (" + typeof( T ).Name + ")", e );
			}
		}

		public static T LoadBinaryJson<T>( Mod mod, string fileNameHasExt ) where T : class {
			return DataFileHelpers.LoadBinaryJson<T>( mod, fileNameHasExt, new JsonSerializerSettings() );
		}


		////////////////

		public static void SaveAsBinaryJson<T>( Mod mod, string fileNameWithExt, JsonSerializerSettings jsonSettings, bool overrides,
				T data ) where T : class {
			DataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = DataFileHelpers.GetFullPath( mod, fileNameWithExt );

				if( data == null ) {
					LogHelpers.Warn( "Failed to save binary file " + fullPath + " - Data is null." );
					return;
				}

				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );
				byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes( dataJson );

				FileHelpers.SaveBinaryFile( dataBytes, fullPath, false, !overrides );
			} catch( IOException e ) {
				string fullDir = DataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to save binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to save binary file " + fileNameWithExt + " at " + fullDir, e );
			}
		}

		public static void SaveAsBinaryJson<T>( Mod mod, string fileNameWithExt, bool overrides, T data ) where T : class {
			DataFileHelpers.SaveAsBinaryJson<T>( mod, fileNameWithExt, new JsonSerializerSettings(), overrides, data );
		}
	}
}
