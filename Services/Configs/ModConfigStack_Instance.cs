using HamstarHelpers.Classes.Loadable;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Supplies a method for programmatically applying config settings changes (internally as an overlaying stack) without
	/// affecting the user's own ModConfig.
	/// </summary>
	public partial class ModConfigStack : ILoadable {
		private IDictionary<Type, IDictionary<int, StackableModConfig>> ConfigStacks
			= new Dictionary<Type, IDictionary<int, StackableModConfig>>();
		private IDictionary<Type, StackableModConfig> CachedMergedStackConfigs
			= new Dictionary<Type, StackableModConfig>();
		private IDictionary<Type, StackableModConfig> CachedMergedDefaultAndStackConfigs
			= new Dictionary<Type, StackableModConfig>();



		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }
		/// @private
		void ILoadable.OnPostModsLoad() { }
		/// @private
		void ILoadable.OnModsUnload() { }
		/// @private
	}
}
