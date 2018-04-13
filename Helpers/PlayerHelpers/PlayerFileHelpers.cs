using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Utilities;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerFileHelpers {
		public static void ErasePlayer( int i ) {
			PlayerFileData data = Main.PlayerList[i];

			try {
				FileUtilities.Delete( data.Path, data.IsCloudSave );
				FileUtilities.Delete( data.Path + ".bak", data.IsCloudSave );
			} catch { }

			bool cloud_save = data.IsCloudSave;
			string path = Path.ChangeExtension( data.Path, ".tplr" );

			FileUtilities.Delete( path, cloud_save );
			FileUtilities.Delete( path + ".bak", cloud_save );

			try {
				string dir_path = data.Path.Substring( 0, data.Path.Length - 4 );

				if( Directory.Exists( dir_path ) ) {
					Directory.Delete( dir_path, true );
				}

				Main.LoadPlayers();
			} catch { }
		}
	}
}
