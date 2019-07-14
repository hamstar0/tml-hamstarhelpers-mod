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
	public partial class ModCustomDataFileHelpers {
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
			ModCustomDataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = ModCustomDataFileHelpers.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataStr = FileHelpers.LoadTextFile( fullPath, false );

				if( dataStr != null ) {
					return JsonConvert.DeserializeObject<T>( dataStr, jsonSettings );
				} else {
					return null;
				}
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load json file " + fileNameNoExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load json file " + fileNameNoExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new HamstarException( "From "+fileNameNoExt+" ("+typeof(T).Name+")", e );
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
			return ModCustomDataFileHelpers.LoadJson<T>( mod, fileNameNoExt, new JsonSerializerSettings() );
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
		public static T LoadBinaryJson<T>( Mod mod, string fileNameWithExt, JsonSerializerSettings jsonSettings ) where T : class {
			ModCustomDataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = ModCustomDataFileHelpers.GetFullPath( mod, fileNameWithExt );
				byte[] dataBytes = FileHelpers.LoadBinaryFile( fullPath, false );
				string dataJson = System.Text.Encoding.UTF8.GetString( dataBytes );

				if( dataBytes != null ) {
					return JsonConvert.DeserializeObject<T>( dataJson, jsonSettings );
				} else {
					return null;
				}
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to load binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load binary file " + fileNameWithExt + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new HamstarException( "From " + fileNameWithExt + " (" + typeof( T ).Name + ")", e );
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
			return ModCustomDataFileHelpers.LoadBinaryJson<T>( mod, fileNameHasExt, new JsonSerializerSettings() );
		}
	}
}
