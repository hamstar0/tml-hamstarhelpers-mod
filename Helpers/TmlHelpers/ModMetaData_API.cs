using System;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public partial class ModMetaDataManager {
		public static bool HasGithub( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			return self.GithubMods.ContainsKey( mod.Name );
		}
		public static bool HasConfig( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			return self.ConfigMods.ContainsKey( mod.Name );
		}
		public static bool HasConfigDefaultsReset( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			return self.ConfigDefaultsResetMods.ContainsKey( mod.Name );
		}

		////////////////

		public static string GetConfigRelativePath( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.ConfigMods.ContainsKey( mod.Name ) ) { return null; }

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
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			MethodInfo config_reload_method = ModMetaDataManager.GetConfigFileLoadMethod( mod );
			config_reload_method.Invoke( null, new object[] { } );
		}
		
		public static void ResetDefaultsConfig( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.ConfigDefaultsResetMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized config resetable mod." );
			}

			MethodInfo config_defaults_method = ModMetaDataManager.GetConfigDefaultsResetMethod( mod );
			config_defaults_method.Invoke( null, new object[] { } );
		}

		////////////////

		public static string GetGithubUserName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo git_user_prop = ModMetaDataManager.GetGithubUserNameProp( mod );
			return (string)git_user_prop.GetValue( null );
		}

		public static string GetGithubProjectName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo git_proj_prop = ModMetaDataManager.GetGitubProjectNameProp( mod );
			return (string)git_proj_prop.GetValue( null );
		}
	}
}
