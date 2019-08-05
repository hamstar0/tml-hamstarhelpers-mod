using HamstarHelpers.Helpers.DotNET.Reflection;
using Newtonsoft.Json;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.TModLoader.Configs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to configs (ModConfig, primarily).
	/// </summary>
	public class ConfigHelpers {
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


		/*public static ModConfig LoadConfig( Type modConfigType ) {
			var config = Activator.CreateInstance( modConfigType ) as ModConfig;
			if( config == null ) {
				throw new TypeInitializationException( modConfigType.FullName, new Exception( "Not a ModConfig subclass." ) );
			}

			object _;
			ReflectionHelpers.RunMethod( modConfigType, config, "Load", new object[] { config }, out _ );

			return config;
		}

		public static void ResetConfig( ModConfig config ) {
			object _;
			ReflectionHelpers.RunMethod( config.GetType(), config, "Reset", new object[] { config }, out _ );
		}

		public static void SaveConfig( ModConfig config ) {
			object _;
			ReflectionHelpers.RunMethod( config.GetType(), config, "Save", new object[] { config }, out _ );
		}


		////////////////

		public static T GetValue( ModConfig config, string fieldOrPropertyName ) {

		}*/
	}
}
