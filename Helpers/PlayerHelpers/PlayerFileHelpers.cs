using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Utilities;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerFileHelpers {
		public static void ErasePlayer( PlayerFileData data ) {
			try {
				FileUtilities.Delete( data.Path, data.IsCloudSave );
				FileUtilities.Delete( data.Path + ".bak", data.IsCloudSave );

				bool cloud_save = data.IsCloudSave;
				string path = Path.ChangeExtension( data.Path, ".tplr" );

				FileUtilities.Delete( path, cloud_save );
				FileUtilities.Delete( path + ".bak", cloud_save );
			
				string dir_path = data.Path.Substring( 0, data.Path.Length - 4 );

				if( Directory.Exists( dir_path ) ) {
					Directory.Delete( dir_path, true );
				}

				Main.LoadPlayers();

				LogHelpers.Log( "Player " + data.Name + " deleted." );
			} catch( Exception e ) {
				LogHelpers.Log( "PlayerFileHelpers.ErasePlayer - Path: " + data.Path + " - " + e.ToString() );
			}
		}
	}
}
