using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers {
	public partial class ModMetaDataManager {
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
