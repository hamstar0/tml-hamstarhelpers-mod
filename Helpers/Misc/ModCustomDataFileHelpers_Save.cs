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
		public static bool SaveAsJson<T>( Mod mod, string fileNameNoExt, JsonSerializerSettings jsonSettings, bool overrides, T data )
				where T : class {
			ModCustomDataFileHelpers.PrepareDir( mod );

			string relDir = ModCustomDataFileHelpers.GetRelativeDirectoryPath( mod );

			if( data == null ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - Data is null." );
				return false;
			}

			try {
				string fullPath = ModCustomDataFileHelpers.GetFullPath( mod, fileNameNoExt + ".json" );
				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );

				return FileHelpers.SaveTextFile( dataJson, fullPath, false, !overrides );
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to save json file " + fileNameNoExt + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + fileNameNoExt + " at " + relDir, e );
			}
		}

		public static bool SaveAsJson<T>( Mod mod, string fileNameNoExt, bool overrides, T data ) where T : class {
			return ModCustomDataFileHelpers.SaveAsJson<T>( mod, fileNameNoExt, new JsonSerializerSettings(), overrides, data );
		}


		////////////////
		
		public static void SaveAsBinaryJson<T>( Mod mod, string fileNameWithExt, JsonSerializerSettings jsonSettings, bool overrides,
				T data ) where T : class {
			ModCustomDataFileHelpers.PrepareDir( mod );

			try {
				string fullPath = ModCustomDataFileHelpers.GetFullPath( mod, fileNameWithExt );

				if( data == null ) {
					LogHelpers.Warn( "Failed to save binary file " + fullPath + " - Data is null." );
					return;
				}

				string dataJson = JsonConvert.SerializeObject( data, jsonSettings );
				byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes( dataJson );

				FileHelpers.SaveBinaryFile( dataBytes, fullPath, false, !overrides );
			} catch( IOException e ) {
				string fullDir = ModCustomDataFileHelpers.GetFullDirectoryPath( mod );
				LogHelpers.Warn( "Failed to save binary file " + fileNameWithExt + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to save binary file " + fileNameWithExt + " at " + fullDir, e );
			}
		}

		public static void SaveAsBinaryJson<T>( Mod mod, string fileNameWithExt, bool overrides, T data ) where T : class {
			ModCustomDataFileHelpers.SaveAsBinaryJson<T>( mod, fileNameWithExt, new JsonSerializerSettings(), overrides, data );
		}
	}
}
