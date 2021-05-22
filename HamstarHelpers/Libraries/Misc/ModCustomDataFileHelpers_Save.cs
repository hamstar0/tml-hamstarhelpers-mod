using System;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;


namespace HamstarHelpers.Libraries.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to file IO for mod custom data.
	/// </summary>
	public partial class ModCustomDataFileLibraries {
		/// <summary>
		/// Saves a custom mod data JSON file.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameNoExt"></param>
		/// <param name="jsonSettings"></param>
		/// <param name="overrides">Replaces any existing files.</param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool SaveAsJson<T>(
				Mod mod,
				string fileNameNoExt,
				JsonSerializerSettings jsonSettings,
				bool overrides, T data )
				where T : class {
			string relDir = ModCustomDataFileLibraries.GetRelativeDirectoryPath( mod );

			if( data == null ) {
				LogLibraries.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - Data is null." );
				return false;
			}

			try {
				ModCustomDataFileLibraries.PrepareDir( mod );

				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );

				return FileLibraries.SaveTextFile( dataJson, fullPath, false, !overrides );
			} catch( IOException e ) {
				LogLibraries.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + fileNameNoExt + " at " + relDir, e );
			}
		}

		/// <summary>
		/// Saves a custom mod data JSON file.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameNoExt"></param>
		/// <param name="overrides">Replaces any existing files.</param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool SaveAsJson<T>( Mod mod, string fileNameNoExt, bool overrides, T data ) where T : class {
			return ModCustomDataFileLibraries.SaveAsJson<T>( mod, fileNameNoExt, new JsonSerializerSettings(), overrides, data );
		}


		////////////////

		/// <summary>
		/// Saves a custom mod data JSON file in binary form.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameWithExt"></param>
		/// <param name="jsonSettings"></param>
		/// <param name="overrides">Replaces any existing files.</param>
		/// <param name="data"></param>
		public static void SaveAsBinaryJson<T>(
				Mod mod,
				string fileNameWithExt,
				JsonSerializerSettings jsonSettings,
				bool overrides,
				T data ) where T : class {
			if( data == null ) {
				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameWithExt );
				LogLibraries.Warn( "Failed to save binary file " + fullPath + " - Data is null." );
				return;
			}

			try {
				ModCustomDataFileLibraries.PrepareDir( mod );

				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameWithExt );

				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );
				byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes( dataJson );

				FileLibraries.SaveBinaryFile( dataBytes, fullPath, false, !overrides );
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileLibraries.GetFullDirectoryPath( mod );
				LogLibraries.Warn( "Failed to save binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to save binary file " + fileNameWithExt + " at " + fullDir, e );
			}
		}

		/// <summary>
		/// Saves a custom mod data JSON file in binary form.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameWithExt"></param>
		/// <param name="overrides">Replaces any existing files.</param>
		/// <param name="data"></param>
		public static void SaveAsBinaryJson<T>( Mod mod, string fileNameWithExt, bool overrides, T data ) where T : class {
			ModCustomDataFileLibraries.SaveAsBinaryJson<T>( mod, fileNameWithExt, new JsonSerializerSettings(), overrides, data );
		}


		////////////////

		/// <summary>
		/// Saves a custom mod data JSON file in binary form.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="fileNameWithExt"></param>
		/// <param name="overrides">Replaces any existing files.</param>
		/// <param name="data"></param>
		public static void SaveAsBinary(
				Mod mod,
				string fileNameWithExt,
				bool overrides,
				byte[] data ) {
			if( data == null ) {
				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameWithExt );
				LogLibraries.Warn( "Failed to save binary file " + fullPath + " - Data is null." );
				return;
			}

			try {
				ModCustomDataFileLibraries.PrepareDir( mod );

				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameWithExt );

				FileLibraries.SaveBinaryFile( data, fullPath, false, !overrides );
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileLibraries.GetFullDirectoryPath( mod );
				LogLibraries.Warn( "Failed to save binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to save binary file " + fileNameWithExt + " at " + fullDir, e );
			}
		}
	}
}
