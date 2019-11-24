using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Supplies a method for programmatically applying config settings changes (internally as an overlaying stack) without
	/// affecting the user's own ModConfig.
	/// </summary>
	public partial class ModConfigStack : ILoadable {
		/// <summary>
		/// Gets either the merged stack of configs (not including the default), or else the default config (raw).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetStackedOrDefaultConfig<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			IDictionary<int, StackableModConfig> configsOf;
			if( !configStack.ConfigStacks.TryGetValue(configType, out configsOf) || configsOf.Count == 0 ) {
				return (T)ModContent.GetInstance<T>();
			}

			return ModConfigStack.GetMergedStackedConfigs<T>();
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Includes the default ModConfig.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedConfigs<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.CachedMergedDefaultAndStackConfigs.ContainsKey( configType ) ) {
				return (T)configStack.CachedMergedDefaultAndStackConfigs[ configType ];
			}

			T baseConfig = (T)ModContent.GetInstance<T>().Clone();
			T mergedConfigs = ModConfigStack.GetMergedStackedConfigs<T>();
			
			ConfigHelpers.MergeConfigs( baseConfig, mergedConfigs );
			//ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfigs, baseConfig );

			configStack.CachedMergedDefaultAndStackConfigs[configType] = mergedConfigs;

			return mergedConfigs;
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Excludes the default ModConfig itself.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedStackedConfigs<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.CachedMergedStackConfigs.ContainsKey( configType ) ) {
				return (T)configStack.CachedMergedStackConfigs[configType];
			}

			IDictionary<int, StackableModConfig> configsOf;
			if( !configStack.ConfigStacks.TryGetValue(configType, out configsOf) ) {
				configsOf = new Dictionary<int, StackableModConfig>();
				configStack.ConfigStacks[ configType ] = configsOf;
			}

			var mergedConfig = (T)Activator.CreateInstance(
				configType,
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);
			if( mergedConfig == null ) {
				throw new ModHelpersException( "Could not generate merge base for ModConfig "+configType.Name );
			}

			foreach( ModConfig entry in configsOf.Values.Reverse() ) {
				//if( entry.IsMerging ) {
				//	ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfig, entry.Config );
				//} else {
				//	ConfigHelpers.MergeConfigs( mergedConfig, entry.Config );
				//}
				ConfigHelpers.MergeConfigs( mergedConfig, (T)entry );
			}

			configStack.CachedMergedStackConfigs[configType] = mergedConfig;

			return mergedConfig;
		}

		////

		/// <summary>
		/// Gets a config on the stack at the specified height (if present).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stackHeight"></param>
		/// <returns></returns>
		public static T GetConfigAt<T>( int stackHeight ) where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
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
		public static void SetStackedConfig<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof(T);

			configStack.ConfigStacks.Set2DSorted( configType, stackHeight, config );

			configStack.CachedMergedStackConfigs.Remove( configType );
			configStack.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}


		/// <summary>
		/// Sets a config to exist on the stack at the specified height, keeping any existing values that aren't overlapping.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		public static void SetStackedConfigChangesOnly<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.ConfigStacks.ContainsKey(configType) ) {
				ConfigHelpers.MergeConfigs( (T)configStack.ConfigStacks[configType], config );
			} else {
				configStack.ConfigStacks.Set2DSorted( configType, stackHeight, config );
			}

			configStack.CachedMergedStackConfigs.Remove( configType );
			configStack.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}


		////////////////

		/// <summary>
		/// Clears any cached merged stacks of the given config. Used when a config (or any of its stack) changes outside
		/// of SetConfig or SetAndMergeConfig.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void Uncache<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			configStack.CachedMergedStackConfigs.Remove( configType );
			configStack.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}

		/// <summary>
		/// Clears any cached merged stacks of the given config. Used when a config (or any of its stack) changes outside
		/// of SetConfig or SetAndMergeConfig.
		/// </summary>
		/// <param name="configType"></param>
		public static void Uncache( Type configType ) {
			var configStack = (ModConfigStack)TmlHelpers.SafelyGetInstance( typeof(ModConfigStack) );

			configStack.CachedMergedStackConfigs.Remove( configType );
			configStack.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}
	}
}
