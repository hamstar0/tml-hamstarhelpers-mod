﻿using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.ModHelpers {
	/// @private
	public partial class ModFeaturesHelpers {
		internal IDictionary<string, Mod> GithubMods;
		internal IDictionary<string, Mod> ConfigMods;
		internal IDictionary<string, Mod> ConfigDefaultsResetMods;


		////////////////

		internal ModFeaturesHelpers() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();
		}


		////////////////

		internal void OnPostModsLoad() {
			this.GithubMods = new Dictionary<string, Mod>();
			this.ConfigMods = new Dictionary<string, Mod>();
			this.ConfigDefaultsResetMods = new Dictionary<string, Mod>();

			foreach( Mod mod in ModLoader.Mods ) {
				if( ModFeaturesHelpers.DetectGithub( mod ) ) {
					this.GithubMods[mod.Name] = mod;
				}
			}
		}
	}
}
