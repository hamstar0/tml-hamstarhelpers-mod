using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Supplies a method for programmatically applying config settings changes (internally as an overlaying stack) without affecting
	/// the user's own ModConfig.
	/// </summary>
	public partial class ModConfigStack : ILoadable {
		/// <summary>
		/// Downward merges the stack of a given config type. Includes the default ModConfig.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configType"></param>
		/// <returns></returns>
		public static T GetMergedConfigs<T>( Type configType ) where T : ModConfig {
			T baseConfig = ModContent.GetInstance<T>();
			T mergedConfigs = ModConfigStack.GetMergedConfigStacks<T>( configType );

			ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfigs, baseConfig );
			return mergedConfigs;
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Excludes the default ModConfig itself.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configType"></param>
		/// <returns></returns>
		public static T GetMergedConfigStacks<T>( Type configType ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			IDictionary<int, (bool IsMerging, ModConfig Config)> configsOf;
			if( !configStack.ConfigStacks.TryGetValue(configType, out configsOf) ) {
				return null;
			}

			var mergedConfig = (ModConfig)Activator.CreateInstance(
				configType,
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);
			if( mergedConfig == null ) {
				return null;
			}

			foreach( (bool IsMerging, ModConfig Config) entry in configsOf.Values.Reverse() ) {
				if( entry.IsMerging ) {
					ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfig, entry.Config );
				} else {
					ConfigHelpers.MergeConfigs( mergedConfig, entry.Config );
				}
			}

			return (T)mergedConfig;
		}

		////

		/// <summary>
		/// Gets a config on the stack at the specified height (if present).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="configType"></param>
		/// <param name="stackHeight"></param>
		/// <param name="isMerging">Indicates this config should try to merge its collection settings up the stack (instead of
		///		overriding).</param>
		/// <returns></returns>
		public static T GetConfigAt<T>( Type configType, int stackHeight, out bool isMerging ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			(bool IsMerging, ModConfig Config)? entry = configStack.ConfigStacks.Get2DOrDefault( configType, stackHeight );
			if( !entry.HasValue ) {
				isMerging = false;
				return null;
			}

			isMerging = entry.Value.IsMerging;
			return (T)entry.Value.Config;
		}


		////////////////

		/// <summary>
		/// Sets a config to exist on the stack at the specified height.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		/// <param name="isMerging">Indicates this config should try to merge its collection settings up the stack (instead of
		///		overriding).</param>
		public static void SetConfig<T>( T config, int stackHeight = 100, bool isMerging = false ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			configStack.ConfigStacks.Set2DSorted( typeof(T), stackHeight, (isMerging, config) );
		}
	}
}
