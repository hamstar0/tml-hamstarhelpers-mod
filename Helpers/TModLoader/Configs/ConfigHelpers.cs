﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.DotNET.Reflection;
using Newtonsoft.Json;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.TModLoader.Configs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to configs (ModConfig, primarily).
	/// </summary>
	public partial class ConfigHelpers {
		/// <summary>
		/// Syncs to everyone. Use with caution.
		/// </summary>
		/// <param name="config"></param>
		/// <returns>`false` if sync is diabled (client-only config).</returns>
		public static bool SyncConfig( ModConfig config ) {	//untested
			if( config.Mode != ConfigScope.ServerSide ) {
				return false;
			}

			string json = JsonConvert.SerializeObject( config, ConfigManager.serializerSettings );

			var requestChanges = (ModPacket)Activator.CreateInstance( typeof( ModPacket ),
				ReflectionHelpers.MostAccess,
				null,
				new object[] { MessageID.InGameChangeConfig },
				null
			);

			if( Main.netMode == 2 ) {
				requestChanges.Write( true );
				requestChanges.Write( "ConfigHelpers.SyncConfig syncing..." );
			}

			requestChanges.Write( config.mod.Name );
			requestChanges.Write( config.Name );
			requestChanges.Write( json );

			requestChanges.Send();

			return true;
		}


		/// <summary>
		/// Combines one ModConfig instance into another, prioritizing non-default values of the latter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="to">Config to push changed field/property values to. Any existing values are overridden, where changes found.</param>
		/// <param name="fro">Config to pull field/property values from. Only pulls non-default (changed) values.</param>
		public static void MergeConfigs<T>( T to, T fro ) where T : ModConfig {
			var configType = typeof(T);
			T template = JsonConvert.DeserializeObject<T>( "{}" );
			/*T template = (T)Activator.CreateInstance(
				configType,
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);*/
			if( template == null ) {
				throw new ModHelpersException( "Could generate template for ModConfig "+configType.Name );
			}

			object froVal, tempVal;

			foreach( MemberInfo memb in typeof(T).GetMembers( BindingFlags.Public | BindingFlags.Instance ) ) {
				if( memb.MemberType == MemberTypes.Property ) {
					var prop = (PropertyInfo)memb;

					if( !prop.CanWrite || !prop.CanRead ) {
						continue;
					}
					if( prop.GetCustomAttribute<JsonIgnoreAttribute>() != null ) {
						continue;
					}
				} else if( memb.MemberType != MemberTypes.Field ) {
					continue;
				}

				if( !ReflectionHelpers.Get( fro, memb.Name, out froVal ) ) {
					throw new ModHelpersException( "Could retrieve member "+memb.Name+" from 'fro' instance of "+configType.Name );
				}
				if( !ReflectionHelpers.Get( template, memb.Name, out tempVal ) ) {
					throw new ModHelpersException( "Could retrieve member "+memb.Name+" from template instance of "+configType.Name );
				}

				if( froVal != tempVal ) {
					if( !ReflectionHelpers.Set( to, memb.Name, froVal ) ) {
						throw new ModHelpersException( "Could set merged field/property "+memb.Name+" for "+configType.Name );
					}
				}
			}
		}
	}
}
