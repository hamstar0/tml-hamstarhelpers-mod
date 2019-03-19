using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.ModHelpers;
using System;

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
	}
}
