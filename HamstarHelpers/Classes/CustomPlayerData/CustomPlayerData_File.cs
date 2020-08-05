using System;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.Misc;


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
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileHelpers.BaseFolder );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to prepare directory: " + fullDir + " - " + e.ToString() );
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
					dataStr = FileHelpers.LoadTextFile( fullPath, false );
				} else {
					fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".dat" );
					byte[] dataBytes = FileHelpers.LoadBinaryFile( fullPath, false, out _ );
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
				LogHelpers.Warn( "Failed to load file " + playerUid + " at " + fullDir + " - " + e.ToString() );
				throw new IOException( "Failed to load file " + playerUid + " at " + fullDir, e );
			} catch( Exception e ) {
				throw new ModHelpersException( "Failed to load file " + playerUid, e );
			}
		}


		////

		private static bool SaveFileData( string className, string playerUid, object data ) {
			if( data == null ) {
				LogHelpers.Warn( "Failed to save file " + playerUid + " - Data is null." );
				return false;
			}

			CustomPlayerData.PrepareDir( className );

			string relDir = CustomPlayerData.GetRelativeDirectoryPath( className );

			if( data == null ) {
				LogHelpers.Warn( "Failed to save json file " + playerUid + " at " + relDir + " - Data is null." );
				return false;
			}

			try {
				if( ModHelpersConfig.Instance.CustomPlayerDataAsText ) {
					string fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".json" );
					string dataJson = JsonConvert.SerializeObject( data, new JsonSerializerSettings() );

					return FileHelpers.SaveTextFile( dataJson, fullPath, false, true );
				} else {
					string _;
					string fullPath = CustomPlayerData.GetFullPath( className, playerUid + ".json" );
					string dataJson = JsonConvert.SerializeObject( data, new JsonSerializerSettings() );

					byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes( dataJson );

					return FileHelpers.SaveBinaryFile( dataBytes, fullPath, false, true, out _ );
				}
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to save json file " + playerUid + " at " + relDir + " - " + e.ToString() );
				throw new IOException( "Failed to save json file " + playerUid + " at " + relDir, e );
			}
		}
	}
}
