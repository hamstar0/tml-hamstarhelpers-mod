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

			PropertyInfo configPathField = ModMetaDataManager.GetConfigFilePathProp( mod );
			return (string)configPathField.GetValue( null );
		}

		/*public static void SetConfigRelativePath( Mod mod, string path ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			FieldInfo configPathField = mod.GetType().GetField( "ConfigFileRelativePath", BindingFlags.Static | BindingFlags.Public );
			configPathField.SetValue( null, path );
		}*/

		public static void ReloadConfigFromFile( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized configurable mod." );
			}

			MethodInfo configReloadMethod = ModMetaDataManager.GetConfigFileLoadMethod( mod );
			configReloadMethod.Invoke( null, new object[] { } );
		}
		
		public static void ResetDefaultsConfig( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.ConfigDefaultsResetMods.ContainsKey( mod.Name ) ) {
				throw new Exception( "Not a recognized config resetable mod." );
			}

			MethodInfo configDefaultsMethod = ModMetaDataManager.GetConfigDefaultsResetMethod( mod );
			configDefaultsMethod.Invoke( null, new object[] { } );
		}

		////////////////

		public static string GetGithubUserName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitUserProp = ModMetaDataManager.GetGithubUserNameProp( mod );
			return (string)gitUserProp.GetValue( null );
		}

		public static string GetGithubProjectName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataManager;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitProjProp = ModMetaDataManager.GetGitubProjectNameProp( mod );
			return (string)gitProjProp.GetValue( null );
		}
	}
}
