﻿using HamstarHelpers.Helpers.Debug;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Utilities;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player file data (.plr, .tplr).
	/// </summary>
	public class PlayerFileHelpers {
		/// <summary>
		/// Erases a given player file.
		/// </summary>
		/// <param name="data"></param>
		public static void ErasePlayer( PlayerFileData data ) {
			try {
				FileUtilities.Delete( data.Path, data.IsCloudSave );
				FileUtilities.Delete( data.Path + ".bak", data.IsCloudSave );

				bool cloudSave = data.IsCloudSave;
				string path = Path.ChangeExtension( data.Path, ".tplr" );

				FileUtilities.Delete( path, cloudSave );
				FileUtilities.Delete( path + ".bak", cloudSave );
			
				string dirPath = data.Path.Substring( 0, data.Path.Length - 4 );

				if( Directory.Exists( dirPath ) ) {
					Directory.Delete( dirPath, true );
				}

				Main.LoadPlayers();

				LogHelpers.Log( "Player " + data.Name + " deleted." );
			} catch( Exception e ) {
				LogHelpers.Log( "PlayerFileHelpers.ErasePlayer - Path: " + data.Path + " - " + e.ToString() );
			}
		}
	}
}
