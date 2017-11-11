using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
	static class _ExtendedModManagerLoader {
		public static void Load() {
			ExtendedModManager.LoadMods();
		}
		public static void Unload() {
			ExtendedModManager.StaticInit();
		}
	}


	public static class ExtendedModManager {
		internal static IDictionary<string, Mod> GithubMods;
		internal static IDictionary<string, Mod> ConfigMods;


		////////////////

		static ExtendedModManager() {
			ExtendedModManager.StaticInit();
		}

		internal static void StaticInit() {
			ExtendedModManager.GithubMods = new Dictionary<string, Mod>();
			ExtendedModManager.ConfigMods = new Dictionary<string, Mod>();
		}


		////////////////

		internal static void LoadMods() {
			ExtendedModManager.StaticInit();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( ExtendedModManager.DetectGithub( mod ) ) {
					ExtendedModManager.GithubMods[mod.Name] = mod;
				}
				if( ExtendedModManager.DetectConfig( mod ) ) {
					ExtendedModManager.ConfigMods[mod.Name] = mod;
				}
			}
		}


		////////////////

		private static bool DetectGithub( Mod mod ) {
			FieldInfo git_user_field = mod.GetType().GetField( "GithubUserName" );
			if( git_user_field == null ) { return false; }
			FieldInfo git_proj_field = mod.GetType().GetField( "GithubProjectName" );
			if( git_proj_field == null ) { return false; }

			return true;
		}

		public static bool DetectConfig( Mod mod ) {
			FieldInfo config_path_field = mod.GetType().GetField( "ConfigFileRelativePath" );
			if( config_path_field == null ) { return false; }
			MethodInfo config_reload_method = mod.GetType().GetMethod( "ReloadConfigFromFile" );
			if( config_reload_method == null ) { return false; }

			return true;
		}

		////////////////

		public static bool HasGithub( Mod mod ) {
			return ExtendedModManager.GithubMods.ContainsKey( mod.Name );
		}
		public static bool HasConfig( Mod mod ) {
			return ExtendedModManager.ConfigMods.ContainsKey( mod.Name );
		}

		////////////////

		public static string GetConfigRelativePath( Mod mod ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) { return null; }

			FieldInfo config_path_field = mod.GetType().GetField( "ConfigFileRelativePath" );
			return (string)config_path_field.GetValue( mod );
		}

		/*public static void SetConfigRelativePath( Mod mod, string path ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			FieldInfo config_path_field = mod.GetType().GetField( "ConfigFileRelativePath" );
			config_path_field.SetValue( mod, path );
		}*/

		public static void ReloadConfigFromFile( Mod mod ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			MethodInfo config_reload_method = mod.GetType().GetMethod( "ReloadConfigFromFile" );
			config_reload_method.Invoke( mod, new object[] { } );
		}

		public static void ReloadAllConfigsFromFile() {
			foreach( var kv in ExtendedModManager.ConfigMods ) {
				ExtendedModManager.ReloadConfigFromFile( kv.Value );
			}
		}

		////////////////
		
		public static string GetGithubUserName( Mod mod ) {
			if( !ExtendedModManager.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			FieldInfo git_user_field = mod.GetType().GetField( "GithubUserName" );
			return (string)git_user_field.GetValue( mod );
		}

		public static string GetGithubProjectName( Mod mod ) {
			if( !ExtendedModManager.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			FieldInfo git_proj_field = mod.GetType().GetField( "GithubProjectName" );
			return (string)git_proj_field.GetValue( mod );
		}
	}
}
