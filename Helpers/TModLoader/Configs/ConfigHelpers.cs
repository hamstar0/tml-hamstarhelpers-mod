using HamstarHelpers.Classes.Errors;
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
		/// <param name="to"></param>
		/// <param name="fro"></param>
		/// <returns></returns>
		public static bool MergeConfigs<T>( T to, T fro ) where T : ModConfig {
			T template = (T)Activator.CreateInstance(
				typeof(T),
				ReflectionHelpers.MostAccess,
				null,
				new object[] { },
				null
			);
			if( template == null ) {
				return false;
			}

			object toVal, froVal, tempVal;

			foreach( MemberInfo memb in typeof(T).GetMembers() ) {
				if( !ReflectionHelpers.Get( to, memb.Name, out toVal ) ) {
					return false;
				}
				if( !ReflectionHelpers.Get( fro, memb.Name, out froVal ) ) {
					return false;
				}
				if( !ReflectionHelpers.Get( template, memb.Name, out tempVal ) ) {
					return false;
				}

				if( froVal != tempVal ) {
					ReflectionHelpers.Set( to, memb.Name, fro );
				}
			}

			return true;
		}
	}
}
