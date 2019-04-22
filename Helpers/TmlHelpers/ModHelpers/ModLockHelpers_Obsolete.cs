using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Services.ModHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public partial class ModLockHelpers {
		[Obsolete("use ModLockService", true)]
		public static bool IsWorldLocked() {
			return ModLockService.IsWorldLocked();
		}

		[Obsolete( "use ModLockService", true )]
		public static bool IsModMismatchFound() {
			return ModLockService.IsModMismatchFound();
		}

		[Obsolete( "use ModLockService", true )]
		public static void LockWorld() {
			ModLockService.LockWorld();
		}

		[Obsolete( "use ModLockService", true )]
		public static void UnlockWorld() {
			ModLockService.UnlockWorld();
		}

		[Obsolete( "use `ModLockHelpers.GetLoadedModsByAuthor`", true )]
		public static IDictionary<string, ISet<Mod>> GetModsByAuthor() {
			return ModListHelpers.GetLoadedModsByAuthor();
		}

		[Obsolete( "use `ModLockHelpers.GetLoadedModsByBuildInfo`", true )]
		public static IDictionary<Services.Tml.BuildPropertiesEditor, Mod> GetModsByBuildInfo() {
			return ModListHelpers.GetLoadedModsByBuildInfo();
		}

		[Obsolete( "use `MenuModHelper.LoadMenuModDownloads`", true )]
		public static void PromptModDownloads( string packTitle, List<string> modNames ) {
			MenuModHelper.LoadMenuModDownloads( packTitle, modNames );
		}
	}
}
