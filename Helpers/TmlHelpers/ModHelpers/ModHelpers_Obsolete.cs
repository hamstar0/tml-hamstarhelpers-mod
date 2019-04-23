using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public static partial class ModHelpers {
		[Obsolete( "use `ModListHelpers.GetAllLoadedModsPreferredOrder()`", true )]
		public static IEnumerable<Mod> GetAllMods() {
			return ModListHelpers.GetAllLoadedModsPreferredOrder();
		}

		[Obsolete( "use `ModListHelpers.GetAllLoadedModsPreferredOrder()`", true )]
		public static IEnumerable<Mod> GetAllPlayableModsPreferredOrder() {
			return ModListHelpers.GetAllLoadedModsPreferredOrder();
		}

		[Obsolete( "use `MainMenuHelpers.LoadMenuModDownloads()`", true )]
		public static void PromptModDownloads( string packTitle, List<string> modNames ) {
			MainMenuHelpers.LoadMenuModDownloads( packTitle, modNames );
		}
	}
}
