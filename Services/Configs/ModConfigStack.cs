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
		/// Downward merges the stack of a given config type. Includes the default ModConfig.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedConfigs<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.CachedMergedConfigs.ContainsKey( configType ) ) {
				return (T)configStack.CachedMergedConfigs[ configType ];
			}

			T baseConfig = (T)ModContent.GetInstance<T>().Clone();
			T mergedConfigs = ModConfigStack.GetMergedConfigStacks<T>();
			
			ConfigHelpers.MergeConfigs( baseConfig, mergedConfigs );
			//ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfigs, baseConfig );

			configStack.CachedMergedConfigs[configType] = mergedConfigs;

			return mergedConfigs;
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Excludes the default ModConfig itself.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedConfigStacks<T>() where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

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
		public static void SetConfig<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
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
		public static void SetAndMergeConfig<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			var configStack = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			if( configStack.ConfigStacks.ContainsKey(configType) ) {
				ConfigHelpers.MergeConfigs( (T)configStack.ConfigStacks[configType], config );
			} else {
				configStack.ConfigStacks.Set2DSorted( configType, stackHeight, config );
			}

			configStack.CachedMergedConfigs.Remove( configType );
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

			configStack.CachedMergedConfigs.Remove( configType );
		}

		/// <summary>
		/// Clears any cached merged stacks of the given config. Used when a config (or any of its stack) changes outside
		/// of SetConfig or SetAndMergeConfig.
		/// </summary>
		/// <param name="configType"></param>
		public static void Uncache( Type configType ) {
			var configStack = (ModConfigStack)TmlHelpers.SafelyGetInstance( typeof(ModConfigStack) );

			configStack.CachedMergedConfigs.Remove( configType );
		}
	}
}
