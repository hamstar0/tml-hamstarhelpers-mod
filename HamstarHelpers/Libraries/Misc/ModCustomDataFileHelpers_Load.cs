using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Libraries.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to file IO for mod custom data.
	/// </summary>
	public partial class ModCustomDataFileLibraries {
		/// <summary>
		/// Loads a custom data JSON file of a given mod.
		/// </summary>
		/// <typeparam name="T">Object type to deserialize from JSON into.</typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameNoExt"></param>
		/// <param name="jsonSettings"></param>
		/// <returns></returns>
		public static T LoadJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings )
				where T : class {
			try {
				ModCustomDataFileLibraries.PrepareDir( mod );

				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataStr = FileLibraries.LoadTextFile( fullPath, false );

				if( dataStr != null ) {
					return JsonConvert.DeserializeObject<T>( dataStr, jsonSettings );
				} else {
					LogLibraries.Alert( "No json file "+fileNameNoExt+"." );
					return null;
				}
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileLibraries.GetFullDirectoryPath( mod );
				LogLibraries.Warn( "Failed to load json file " + fileNameNoExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load json file " + fileNameNoExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new ModHelpersException( "From "+fileNameNoExt+" ("+typeof(T).Name+")", e );
			}
		}

		/// <summary>
		/// Loads a custom data JSON file of a given mod.
		/// </summary>
		/// <typeparam name="T">Object type to deserialize from JSON into.</typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameNoExt"></param>
		/// <returns></returns>
		public static T LoadJson<T>( Mod mod, string fileNameNoExt ) where T : class {
			return ModCustomDataFileLibraries.LoadJson<T>( mod, fileNameNoExt, new JsonSerializerSettings() );
		}


		////////////////

		/// <summary>
		/// Loads a binary custom data JSON file of a given mod.
		/// </summary>
		/// <typeparam name="T">Object type to deserialize from JSON into.</typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameWithExt"></param>
		/// <param name="jsonSettings"></param>
		/// <returns></returns>
		public static T LoadBinaryJson<T>( Mod mod, string fileNameWithExt, JsonSerializerSettings jsonSettings )
				where T : class {
			try {
				ModCustomDataFileLibraries.PrepareDir( mod );

				string fullPath = ModCustomDataFileLibraries.GetFullPath( mod, fileNameWithExt );
				byte[] dataBytes = FileLibraries.LoadBinaryFile( fullPath, false );
				if( dataBytes == null ) {
					return null;
				}

				string dataJson = System.Text.Encoding.UTF8.GetString( dataBytes );

				if( dataBytes != null ) {
					return JsonConvert.DeserializeObject<T>( dataJson, jsonSettings );
				} else {
					LogLibraries.Alert( "No json file " + fileNameWithExt + "." );
					return null;
				}
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileLibraries.GetFullDirectoryPath( mod );
				LogLibraries.Warn( "Failed to load binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load binary file " + fileNameWithExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new ModHelpersException( "From " + fileNameWithExt + " (" + typeof( T ).Name + ")", e );
			}
		}

		/// <summary>
		/// Loads a binary custom data JSON file of a given mod.
		/// </summary>
		/// <typeparam name="T">Object type to deserialize from JSON into.</typeparam>
		/// <param name="mod"></param>
		/// <param name="fileNameHasExt"></param>
		/// <returns></returns>
		public static T LoadBinaryJson<T>( Mod mod, string fileNameHasExt ) where T : class {
			return ModCustomDataFileLibraries.LoadBinaryJson<T>( mod, fileNameHasExt, new JsonSerializerSettings() );
		}
	}
}
