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
		public static string BaseFolder => "Mod Specific Data";



		////////////////

		public static string GetRelativeDirectoryPath( Mod mod ) {
			return ModCustomDataFileHelpers.BaseFolder + Path.DirectorySeparatorChar + mod.Name;
		}

		public static string GetFullDirectoryPath( Mod mod ) {
			return Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileHelpers.GetRelativeDirectoryPath(mod);
		}

		public static string GetFullPath( Mod mod, string fileNameHasExt ) {
			return ModCustomDataFileHelpers.GetFullDirectoryPath( mod ) + Path.DirectorySeparatorChar + fileNameHasExt;
		}

		////////////////

		private static void PrepareDir( Mod mod ) {
			string fullDir = ModCustomDataFileHelpers.GetFullDirectoryPath( mod );
			
			try {
				Directory.CreateDirectory( Main.SavePath );
				Directory.CreateDirectory( Main.SavePath + Path.DirectorySeparatorChar + ModCustomDataFileHelpers.BaseFolder );
				Directory.CreateDirectory( fullDir );
			} catch( IOException e ) {
				LogHelpers.Warn( "Failed to prepare directory: " + fullDir+" - "+e.ToString() );
				throw new IOException( "Failed to prepare directory: " + fullDir, e );
			}
		}
	}
}
