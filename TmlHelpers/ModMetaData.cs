using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	static class _ModMetaDataManagerLoader {
		public static void Load() {
			ModMetaDataManager.LoadMods();
		}
		public static void Unload() {
			ModMetaDataManager.StaticInit();
		}
	}



	public static class ModMetaDataManager {
		internal static IDictionary<string, Mod> GithubMods;
		internal static IDictionary<string, Mod> ConfigMods;


		////////////////

		static ModMetaDataManager() {
			ModMetaDataManager.StaticInit();
		}

		internal static void StaticInit() {
			ModMetaDataManager.GithubMods = new Dictionary<string, Mod>();
			ModMetaDataManager.ConfigMods = new Dictionary<string, Mod>();
		}


		////////////////

		internal static void LoadMods() {
			ModMetaDataManager.StaticInit();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( ModMetaDataManager.DetectGithub( mod ) ) {
					ModMetaDataManager.GithubMods[mod.Name] = mod;
				}
				if( ModMetaDataManager.DetectConfig( mod ) ) {
					ModMetaDataManager.ConfigMods[mod.Name] = mod;
				}
			}
		}


		////////////////

		private static PropertyInfo GetGithubUserNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubUserName", BindingFlags.Static | BindingFlags.Public );
		}
		private static PropertyInfo GetGitubProjectNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubProjectName", BindingFlags.Static | BindingFlags.Public );
		}

		private static PropertyInfo GetConfigFilePathProp( Mod mod ) {
			return mod.GetType().GetProperty( "ConfigFileRelativePath", BindingFlags.Static | BindingFlags.Public );
		}
		private static MethodInfo GetConfigReloadMethod( Mod mod ) {
			return mod.GetType().GetMethod( "ReloadConfigFromFile", BindingFlags.Static | BindingFlags.Public );
		}

		////////////////

		private static bool DetectGithub( Mod mod ) {
			if( ModMetaDataManager.GetGithubUserNameProp( mod ) == null ) { return false; }
			if( ModMetaDataManager.GetGitubProjectNameProp( mod ) == null ) { return false; }
			return true;
		}

		public static bool DetectConfig( Mod mod ) {
			if( ModMetaDataManager.GetConfigFilePathProp( mod ) == null ) { return false; }
			if( ModMetaDataManager.GetConfigReloadMethod( mod ) == null ) { return false; }
			return true;
		}

		////////////////

		public static bool HasGithub( Mod mod ) {
			return ModMetaDataManager.GithubMods.ContainsKey( mod.Name );
		}
		public static bool HasConfig( Mod mod ) {
			return ModMetaDataManager.ConfigMods.ContainsKey( mod.Name );
		}

		////////////////

		public static string GetConfigRelativePath( Mod mod ) {
			if( !ModMetaDataManager.ConfigMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo config_path_field = ModMetaDataManager.GetConfigFilePathProp( mod );
			return (string)config_path_field.GetValue( null );
		}

		/*public static void SetConfigRelativePath( Mod mod, string path ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			FieldInfo config_path_field = mod.GetType().GetField( "ConfigFileRelativePath", BindingFlags.Static | BindingFlags.Public );
			config_path_field.SetValue( null, path );
		}*/

		public static void ReloadConfigFromFile( Mod mod ) {
			if( !ModMetaDataManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			MethodInfo config_reload_method = ModMetaDataManager.GetConfigReloadMethod( mod );
			config_reload_method.Invoke( null, new object[] { } );
		}

		////////////////
		
		public static string GetGithubUserName( Mod mod ) {
			if( !ModMetaDataManager.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo git_user_prop = ModMetaDataManager.GetGithubUserNameProp( mod );
			return (string)git_user_prop.GetValue( null );
		}

		public static string GetGithubProjectName( Mod mod ) {
			if( !ModMetaDataManager.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo git_proj_prop = ModMetaDataManager.GetGitubProjectNameProp( mod );
			return (string)git_proj_prop.GetValue( null );
		}
	}
}
