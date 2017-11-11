using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	public interface ExtendedModData {
		string GithubUserName { get; }
		string GithubProjectName { get; }

		string ConfigFileRelativePath { get; }
	}


	
	////////////////

	static class _ExtendedModManagerLoader {
		public static void Load() {
			ExtendedModManager.LoadMods();
		}
		public static void Unload() {
			ExtendedModManager.StaticInit();
		}
	}
	

	public static class ExtendedModManager {
		public static ISet<Mod> ExtendedMods = new HashSet<Mod>();


		////////////////

		static ExtendedModManager() {
			ExtendedModManager.StaticInit();
		}

		internal static void StaticInit() {
			ExtendedModManager.ExtendedMods = new HashSet<Mod>();
		}


		////////////////

		public static bool HasGithub( Mod mod ) {
			if( !(mod is ExtendedModData) ) { return false; }
			return ExtendedModManager.HasGithub( (ExtendedModData)mod );
		}
		public static bool HasGithub( ExtendedModData ext_mod ) {
			return !string.IsNullOrEmpty( ext_mod.GithubUserName ) && !string.IsNullOrEmpty( ext_mod.GithubProjectName );
		}

		public static bool HasConfig( Mod mod ) {
			if( !(mod is ExtendedModData) ) { return false; }
			return ExtendedModManager.HasConfig( (ExtendedModData)mod );
		}
		public static bool HasConfig( ExtendedModData ext_mod ) {
			return !string.IsNullOrEmpty( ext_mod.ConfigFileRelativePath );
		}

		////////////////

		internal static void LoadMods() {
			ExtendedModManager.StaticInit();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod is ExtendedModData ) {
					ExtendedModManager.ExtendedMods.Add( mod );
				}
			}
		}
	}
}
