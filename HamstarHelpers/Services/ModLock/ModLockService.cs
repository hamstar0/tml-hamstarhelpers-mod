using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader.Mods;


namespace HamstarHelpers.Services.ModHelpers {
	/// <summary>
	/// Provides a way to lock the given current loaded mods with a given world. May also be accessed in-game via. the
	/// Mod Helpers control panel.
	/// </summary>
	public partial class ModLockService {
		/// <summary>
		/// Indicates if the current world is "locked".
		/// </summary>
		/// <returns></returns>
		public static bool IsWorldLocked() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = ModContent.GetInstance<ModHelpersWorld>();

			return modlock.WorldModLocks.ContainsKey( modworld.ObsoleteId2 );
		}

		/// <summary>
		/// Indicates if the current mod loadout does not match the current world's "locked" set (or exceeds it, if
		/// the `WorldModLockMininmumOnly` setting is `true`).
		/// </summary>
		/// <returns></returns>
		public static bool IsModMismatchFound() {
			if( !ModHelpersConfig.Instance.WorldModLockEnable ) {
				return false;
			}

			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;

			if( modlock.MissingModNames.Count > 0 ) {
				return true;
			}
			if( ModHelpersConfig.Instance.WorldModLockMinimumOnly && modlock.ExtraModNames.Count > 0 ) {
				return true;
			}

			return false;
		}


		/// <summary>
		/// Locks the current world.
		/// </summary>
		public static void LockWorld() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = ModContent.GetInstance<ModHelpersWorld>();

			IEnumerable<Mod> allMods = ModListLibraries.GetAllLoadedModsPreferredOrder();
			ISet<string> modNames = new HashSet<string>();

			foreach( Mod mod in allMods ) {
				modNames.Add( mod.Name );
			}

			modlock.WorldModLocks[ modworld.ObsoleteId2 ] = modNames;

			modlock.ScanMods( modworld );

			if( ModHelpersConfig.Instance.WorldModLockMinimumOnly ) {
				Main.NewText( "Your world now requires exactly these mods: " + string.Join( ", ", modNames ) );
			} else {
				Main.NewText( "Your world now requires at least these mods: " + string.Join( ", ", modNames ) );
			}
		}

		/// <summary>
		/// Unlocks the current world.
		/// </summary>
		public static void UnlockWorld() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = ModContent.GetInstance<ModHelpersWorld>();

			modlock.WorldModLocks.Remove( modworld.ObsoleteId2 );
			modlock.MismatchBroadcastMade = false;

			modlock.ScanMods( modworld );

			Main.NewText( "Your world now has no mod requirement." );
		}
	}
}
