using System;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET;
using HamstarHelpers.Libraries.Misc;


namespace HamstarHelpers.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		/// <summary></summary>
		public static string BaseFolder => "Player" + Path.DirectorySeparatorChar + "ModHelpers";



		////////////////

		private static void PrepareDir( string className ) {
			string fullDir = CustomPlayerData.GetFullDirectoryPath( className );

			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileLibraries.BaseFolder );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				LogLibraries.Warn( "Failed to prepare directory: " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
			}
		}

		private static string GetRelativeDirectoryPath( string className ) {
			return CustomPlayerData.BaseFolder + Path.DirectorySeparatorChar + className;
		}

		private static string GetFullDirectoryPath( string className ) {
			return Main.SavePath + Path.DirectorySeparatorChar + CustomPlayerData.GetRelativeDirectoryPath( className );
		}

		private static string GetFullPath( string className, string fileNameWithExt ) {
			return CustomPlayerData.GetFullDirectoryPath( className ) + Path.DirectorySeparatorChar + fileNameWithExt;
		}


		////////////////

		private static object LoadFileData( string className, string playerUid ) {
			string dataStr, fullPath;
			string _;

			CustomPlayerData.PrepareDir( className );

			try {
				if( ModHelpersConfig.Instance.CustomPlayerDataAsText ) {
					fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".json" );
					dataStr = FileLibraries.LoadTextFile( fullPath, false );
				} else {
					fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".dat" );
					byte[] dataBytes = FileLibraries.LoadBinaryFile( fullPath, false, out _ );
					if( dataBytes == null ) {
						return null;
					}

					dataStr = System.Text.Encoding.UTF8.GetString( dataBytes );
				}

				if( dataStr != null ) {
					return JsonConvert.DeserializeObject( dataStr, new JsonSerializerSettings() );
				} else {
					return null;
				}
			} catch( IOException e ) {
				string fullDir = CustomPlayerData.GetFullDirectoryPath( className );
				LogLibraries.Warn( "Failed to load file " + playerUid + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load file " + playerUid + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new ModHelpersException( "Failed to load file " + playerUid, e );
			}
		}


		////

		private static bool SaveFileData( string className, string playerUid, object data ) {
			if( data == null ) {
				LogLibraries.Warn( "Failed to save file " + playerUid + " - Data is null." );
				return false;
			}

			CustomPlayerData.PrepareDir( className );

			string relDir = CustomPlayerData.GetRelativeDirectoryPath( className );

			if( data == null ) {
				LogLibraries.Warn( "Failed to save json file " + playerUid + " at " + relDir + " - Data is null." );
				return false;
			}

			try {
				if( ModHelpersConfig.Instance.CustomPlayerDataAsText ) {
					string fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".json" );
					string dataJson = JsonConvert.SerializeObject( data, new JsonSerializerSettings() );
					
					return FileLibraries.SaveTextFile( dataJson, fullPath, false, true );
				} else {
					string _;
					string fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".dat" );
					string dataJson = JsonConvert.SerializeObject( data, new JsonSerializerSettings() );
//LogHelpers.Log( "SAVE FILE DATA: "+dataJson);

					byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes( dataJson );

					return FileLibraries.SaveBinaryFile( dataBytes, fullPath, false, true, out _ );
				}
			} catch( IOException e ) {
				LogLibraries.Warn( "Failed to save json file " + playerUid + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + playerUid + " at " + relDir, e );
			}
		}
	}
}
