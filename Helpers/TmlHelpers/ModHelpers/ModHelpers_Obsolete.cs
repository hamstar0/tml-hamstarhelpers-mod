using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public static partial class ModHelpers {
		[Obsolete( "use ModListHelpers.GetAllLoadedModsPreferredOrder()", true )]
		public static IEnumerable<Mod> GetAllMods() {
			return ModHelpers.GetAllPlayableModsPreferredOrder();
		}

		[Obsolete( "use ModListHelpers.GetAllLoadedModsPreferredOrder()", true )]
		public static IEnumerable<Mod> GetAllPlayableModsPreferredOrder() {
			return ModListHelpers.GetAllLoadedModsPreferredOrder();
		}

		[Obsolete( "use ModListHelpers.PromptModDownloads()", true )]
		public static void PromptModDownloads( string packTitle, List<string> modNames ) {
			ModListHelpers.PromptModDownloads( packTitle, modNames );
		}
	}
}
