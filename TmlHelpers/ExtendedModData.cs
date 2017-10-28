using System.Collections.Generic;
using Terraria.ModLoader;

namespace HamstarHelpers.TmlHelpers {
	public interface ExtendedModData {
		string GithubUrl { get; }
	}

	
	////////////////

	static class _ExtendedModManagerLoader {
		public static void Load() {
			ExtendedModManager.LoadMods();
		}
		public static void Unload() {
			ExtendedModManager.StaticInit();
		}
	}
	

	public static class ExtendedModManager {
		public static ISet<Mod> ExtendedMods = new HashSet<Mod>();


		////////////////

		static ExtendedModManager() {
			ExtendedModManager.StaticInit();
		}

		internal static void StaticInit() {
			ExtendedModManager.ExtendedMods = new HashSet<Mod>();
		}

		////////////////

		internal static void LoadMods() {
			ExtendedModManager.StaticInit();

			foreach( Mod mod in ModLoader.LoadedMods ) {
				if( mod is ExtendedModData ) {
					ExtendedModManager.ExtendedMods.Add( mod );
				}
			}
		}
	}
}
