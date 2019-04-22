using HamstarHelpers.Components.Errors;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public partial class ModMetaDataManager {
		private static PropertyInfo GetGithubUserNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubUserName", BindingFlags.Static | BindingFlags.Public );
		}

		private static PropertyInfo GetGitubProjectNameProp( Mod mod ) {
			return mod.GetType().GetProperty( "GithubProjectName", BindingFlags.Static | BindingFlags.Public );
		}

		private static PropertyInfo GetConfigFilePathProp( Mod mod ) {
			return mod.GetType().GetProperty( "ConfigFileRelativePath", BindingFlags.Static | BindingFlags.Public );
		}

		private static MethodInfo GetConfigFileLoadMethod( Mod mod ) {
			return mod.GetType().GetMethod( "ReloadConfigFromFile", BindingFlags.Static | BindingFlags.Public );
		}

		private static MethodInfo GetConfigDefaultsResetMethod( Mod mod ) {
			return mod.GetType().GetMethod( "ResetConfigFromDefaults", BindingFlags.Static | BindingFlags.Public );
		}
		
		////////////////

		public static bool DetectGithub( Mod mod ) {
			if( ModMetaDataManager.GetGithubUserNameProp( mod ) == null ) { return false; }
			if( ModMetaDataManager.GetGitubProjectNameProp( mod ) == null ) { return false; }
			return true;
		}

		public static bool DetectConfig( Mod mod ) {
			if( ModMetaDataManager.GetConfigFilePathProp( mod ) == null ) { return false; }
			if( ModMetaDataManager.GetConfigFileLoadMethod( mod ) == null ) { return false; }
			return true;
		}

		public static bool DetectConfigDefaultsReset( Mod mod ) {
			if( ModMetaDataManager.GetConfigDefaultsResetMethod( mod ) == null ) { return false; }
			return true;
		}


		////////////////

		public static bool HasGithub( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			return self.GithubMods.ContainsKey( mod.Name );
		}
		public static bool HasConfig( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			return self.ConfigMods.ContainsKey( mod.Name );
		}
		public static bool HasConfigDefaultsReset( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			return self.ConfigDefaultsResetMods.ContainsKey( mod.Name );
		}

		////////////////

		public static string GetConfigRelativePath( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			if( !self.ConfigMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo configPathField = ModMetaDataManager.GetConfigFilePathProp( mod );
			return (string)configPathField.GetValue( null );
		}

		/*public static void SetConfigRelativePath( Mod mod, string path ) {
			if( !ExtendedModManager.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new HamstarException( "Not a recognized configurable mod." );
			}

			FieldInfo configPathField = mod.GetType().GetField( "ConfigFileRelativePath", BindingFlags.Static | BindingFlags.Public );
			configPathField.SetValue( null, path );
		}*/

		public static void ReloadConfigFromFile( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			if( !self.ConfigMods.ContainsKey( mod.Name ) ) {
				throw new HamstarException( "Not a recognized configurable mod." );
			}

			MethodInfo configReloadMethod = ModMetaDataManager.GetConfigFileLoadMethod( mod );
			configReloadMethod.Invoke( null, new object[] { } );
		}

		public static void ResetDefaultsConfig( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			if( !self.ConfigDefaultsResetMods.ContainsKey( mod.Name ) ) {
				throw new HamstarException( "Not a recognized config resetable mod." );
			}

			MethodInfo configDefaultsMethod = ModMetaDataManager.GetConfigDefaultsResetMethod( mod );
			configDefaultsMethod.Invoke( null, new object[] { } );
		}

		////////////////

		public static string GetGithubUserName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitUserProp = ModMetaDataManager.GetGithubUserNameProp( mod );
			return (string)gitUserProp.GetValue( null );
		}

		public static string GetGithubProjectName( Mod mod ) {
			var self = ModHelpersMod.Instance.ModMetaDataMngr;
			if( !self.GithubMods.ContainsKey( mod.Name ) ) { return null; }

			PropertyInfo gitProjProp = ModMetaDataManager.GetGitubProjectNameProp( mod );
			return (string)gitProjProp.GetValue( null );
		}
	}
}
