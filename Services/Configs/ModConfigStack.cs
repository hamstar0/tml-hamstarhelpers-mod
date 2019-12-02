using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.TModLoader.Configs;
using Newtonsoft.Json;
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
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			IDictionary<int, StackableModConfig> configsOf;
			if( !cs.ConfigStacks.TryGetValue(configType, out configsOf) || configsOf.Count == 0 ) {
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
			return (T)ModConfigStack.GetMergedConfigsForType( typeof(T) );
		}

		internal static StackableModConfig GetMergedConfigsForType( Type configType ) {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();

			if( cs.CachedMergedDefaultAndStackConfigs.ContainsKey( configType ) ) {
				return (StackableModConfig)cs.CachedMergedDefaultAndStackConfigs[ configType ];
			}

			StackableModConfig configSingleton;
			bool success = ReflectionHelpers.RunMethod<StackableModConfig>(	//ModContent.GetInstance<T>();
				classType: typeof( ModContent ),
				instance: null,
				methodName: "GetInstance",
				args: new object[] { },
				generics: new Type[] { configType },
				out configSingleton
			);
			if( !success ) {
				throw new ModHelpersException( "Could not get StackableModConfig of "+configType.Name );
			}

			var baseConfig = (StackableModConfig)ConfigManager.GeneratePopulatedClone( configSingleton );
			//T baseConfig = (T)ModContent.GetInstance<T>().Clone();
			StackableModConfig stackMergedConfigs = ModConfigStack.GetMergedStackedConfigsForType( configType );
			
			ConfigHelpers.MergeConfigsForType( configType, baseConfig, stackMergedConfigs );
			//ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfigs, baseConfig );

			cs.CachedMergedDefaultAndStackConfigs[configType] = baseConfig;

			return baseConfig;
		}


		/// <summary>
		/// Downward merges the stack of a given config type. Excludes the default ModConfig itself.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetMergedStackedConfigs<T>() where T : StackableModConfig {
			return (T)ModConfigStack.GetMergedStackedConfigsForType( typeof(T) );
		}

		internal static StackableModConfig GetMergedStackedConfigsForType( Type configType ) {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();

			if( cs.CachedMergedStackConfigs.ContainsKey( configType ) ) {
				return cs.CachedMergedStackConfigs[configType];
			}

			IDictionary<int, StackableModConfig> configsOf;
			if( !cs.ConfigStacks.TryGetValue(configType, out configsOf) ) {
				configsOf = new Dictionary<int, StackableModConfig>();
				cs.ConfigStacks[ configType ] = configsOf;
			}

			var newDefaultConfig = (StackableModConfig)Activator.CreateInstance(
				configType,
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);
			if( newDefaultConfig == null ) {
				throw new ModHelpersException( "Could not generate merge base for ModConfig "+configType.Name );
			}

			JsonConvert.PopulateObject( "{}", newDefaultConfig, ConfigManager.serializerSettings );

			foreach( (int stackPos, ModConfig entry) in configsOf.Reverse() ) {
				//if( entry.IsMerging ) {
				//	ConfigHelpers.MergeConfigsAndTheirCollections( mergedConfig, entry.Config );
				//} else {
				//	ConfigHelpers.MergeConfigs( mergedConfig, entry.Config );
				//}
				ConfigHelpers.MergeConfigsForType( configType, newDefaultConfig, (StackableModConfig)entry );
			}

			cs.CachedMergedStackConfigs[configType] = newDefaultConfig;

			return newDefaultConfig;
		}

		////

		/// <summary>
		/// Gets a config on the stack at the specified height (if present).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stackHeight"></param>
		/// <returns></returns>
		public static T GetConfigAt<T>( int stackHeight ) where T : StackableModConfig {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			return (T)cs.ConfigStacks.Get2DOrDefault( configType, stackHeight );
		}


		////////////////

		/// <summary>
		/// Sets a config to exist on the stack at the specified height.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		public static void SetStackedConfig<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof(T);

			cs.ConfigStacks.Set2DSorted( configType, stackHeight, config );

			cs.CachedMergedStackConfigs.Remove( configType );
			cs.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}


		/// <summary>
		/// Sets a config to exist on the stack at the specified height. Any values of an existing config at the given stack
		/// height are preserved if they do not overlap with the latest changes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="config"></param>
		/// <param name="stackHeight"></param>
		public static void SetStackedConfigChangesOnly<T>( T config, int stackHeight = 100 ) where T : StackableModConfig {
			ModConfigStack.SetStackedConfigChangesOnlyForType( typeof(T), config, stackHeight );
		}

		internal static void SetStackedConfigChangesOnlyForType( Type configType, StackableModConfig config, int stackHeight = 100 ) {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			StackableModConfig existingConfigAt;

			if( cs.ConfigStacks.TryGetValue2D(configType, stackHeight, out existingConfigAt) ) {
				ConfigHelpers.MergeConfigsForType( configType, existingConfigAt, config );
			} else {
				cs.ConfigStacks.Set2DSorted( configType, stackHeight, config );
			}

			cs.CachedMergedStackConfigs.Remove( configType );
			cs.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}


		////////////////

		/// <summary>
		/// Clears any cached merged stacks of the given config. Used when a config (or any of its stack) changes outside
		/// of SetConfig or SetAndMergeConfig.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void Uncache<T>() where T : StackableModConfig {
			var cs = TmlHelpers.SafelyGetInstance<ModConfigStack>();
			var configType = typeof( T );

			cs.CachedMergedStackConfigs.Remove( configType );
			cs.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}

		/// <summary>
		/// Clears any cached merged stacks of the given config. Used when a config (or any of its stack) changes outside
		/// of SetConfig or SetAndMergeConfig.
		/// </summary>
		/// <param name="configType"></param>
		public static void Uncache( Type configType ) {
			var cs = (ModConfigStack)TmlHelpers.SafelyGetInstance( typeof(ModConfigStack) );

			cs.CachedMergedStackConfigs.Remove( configType );
			cs.CachedMergedDefaultAndStackConfigs.Remove( configType );
		}
	}
}
