using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.TmlHelpers {
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

		internal IDictionary<string, Mod> GithubMods;
		internal IDictionary<string, Mod> ConfigMods;
		internal IDictionary<string, Mod> ConfigDefaultsResetMods;


		////////////////

		internal ModMetaDataManager() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();
		}


		////////////////

		internal void OnPostSetupContent() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( ModMetaDataManager.DetectGithub( mod ) ) {
					this.GithubMods[mod.Name] = mod;
				}
				if( ModMetaDataManager.DetectConfig( mod ) ) {
					this.ConfigMods[mod.Name] = mod;
				}
				if( ModMetaDataManager.DetectConfigDefaultsReset( mod ) ) {
					this.ConfigDefaultsResetMods[mod.Name] = mod;
				}
			}
		}
	}
}
