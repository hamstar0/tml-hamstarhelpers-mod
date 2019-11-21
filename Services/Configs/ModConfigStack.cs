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
		/// <returns></returns>
		public static T GetMergedConfigs<T>() where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.CachedMergedConfigs.ContainsKey( configType ) ) {
				return (T)configStack.CachedMergedConfigs[ configType ];
			}

			T baseConfig = ModContent.GetInstance<T>();
			T mergedConfigs = ModConfigStack.GetMergedConfigStacks<T>();

			ConfigHelpers.MergeConfigs( mergedConfigs, baseConfig );
			//ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfigs, baseConfig );

			configStack.CachedMergedConfigs[configType] = mergedConfigs;

			return mergedConfigs;
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Excludes the default ModConfig itself.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedConfigStacks<T>() where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			var configType = typeof( T );

			IDictionary<int, ModConfig> configsOf;
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

			foreach( ModConfig entry in configsOf.Values.Reverse() ) {
				//if( entry.IsMerging ) {
				//	ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfig, entry.Config );
				//} else {
				//	ConfigHelpers.MergeConfigs( mergedConfig, entry.Config );
				//}
				ConfigHelpers.MergeConfigs( mergedConfig, entry );
			}

			return (T)mergedConfig;
		}

		////

		/// <summary>
		/// Gets a config on the stack at the specified height (if present).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stackHeight"></param>
		/// <returns></returns>
		public static T GetConfigAt<T>( int stackHeight ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			var configType = typeof( T );

			return (T)configStack.ConfigStacks.Get2DOrDefault( configType, stackHeight );
		}


		////////////////

		/// <summary>
		/// Sets a config to exist on the stack at the specified height.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		public static void SetConfig<T>( T config, int stackHeight = 100 ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			var configType = typeof(T);

			configStack.ConfigStacks.Set2DSorted( configType, stackHeight, config );
			configStack.CachedMergedConfigs.Remove( configType );
		}


		/// <summary>
		/// Sets a config to exist on the stack at the specified height, merging if collisions.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		public static void SetAndMergeConfig<T>( T config, int stackHeight = 100 ) where T : ModConfig {
			var configStack = ModContent.GetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.ConfigStacks.ContainsKey(configType) ) {
				ConfigHelpers.MergeConfigs( (T)configStack.ConfigStacks[configType], config );
			} else {
				configStack.ConfigStacks.Set2DSorted( configType, stackHeight, config );
			}

			configStack.CachedMergedConfigs.Remove( configType );
		}
	}
}
