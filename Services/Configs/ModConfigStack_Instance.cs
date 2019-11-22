using HamstarHelpers.Classes.Loadable;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Supplies a method for programmatically applying config settings changes (internally as an overlaying stack) without affecting
	/// the user's own ModConfig.
	/// </summary>
	public partial class ModConfigStack : ILoadable {
		private IDictionary<Type, IDictionary<int, StackableModConfig>> ConfigStacks
			= new Dictionary<Type, IDictionary<int, StackableModConfig>>();
		private IDictionary<Type, StackableModConfig> CachedMergedConfigs
			= new Dictionary<Type, StackableModConfig>();



		////////////////

		/// @private
		public void OnModsLoad() { }
		/// @private
		public void OnPostModsLoad() { }
		/// @private
		public void OnModsUnload() { }
	}
}
