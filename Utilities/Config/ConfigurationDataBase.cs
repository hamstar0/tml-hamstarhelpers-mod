using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Utilities.Config {
	public interface ConfigurableMod {
		JsonConfig<object> Config { get; }
	}




	static class _ConfigurableModManagerLoader {
		public static void Load() {
			ConfigurableModManager.LoadMods();
		}
		public static void Unload() {
			ConfigurableModManager.StaticInit();
		}
	}
	

	public static class ConfigurableModManager {
		public static IList<ConfigurableMod> Mods = new List<ConfigurableMod>();


		////////////////

		static ConfigurableModManager() {
			ConfigurableModManager.StaticInit();
		}

		internal static void StaticInit() {
			ConfigurableModManager.Mods = new List<ConfigurableMod>();
		}

		////////////////

		internal static void LoadMods() {
			ConfigurableModManager.StaticInit();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				ConfigurableMod cmod = mod as ConfigurableMod;
				if( cmod != null ) {
					ConfigurableModManager.Mods.Add( cmod );
				}
			}
		}
	}



	public class ConfigurationDataBase { }
}
