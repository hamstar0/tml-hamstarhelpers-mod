using HamstarHelpers.Classes.Loadable;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Supplies a method for programmatically applying config settings changes (internally as an overlaying stack) without
	/// affecting the user's own ModConfig. Must be subclassed.
	/// </summary>
	public abstract partial class StackableModConfig : ModConfig, ILoadable {
		private IDictionary<Type, IDictionary<int, StackableModConfig>> ConfigStacks
			= new Dictionary<Type, IDictionary<int, StackableModConfig>>();
		private IDictionary<Type, StackableModConfig> CachedMergedConfigs
			= new Dictionary<Type, StackableModConfig>();



		////////////////

		/// @private
		void ILoadable.OnModsLoad() { }
		/// @private
		void ILoadable.OnPostModsLoad() { }
		/// @private
		void ILoadable.OnModsUnload() { }
		/// @private


		////////////////

		public override void OnChanged() {
			StackableModConfig.Uncache( this.GetType() );
		}
	}
}
