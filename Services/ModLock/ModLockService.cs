using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Mods;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.ModHelpers {
	public partial class ModLockService {
		public static bool IsWorldLocked() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = mymod.GetModWorld<ModHelpersWorld>();

			return modlock.WorldModLocks.ContainsKey( modworld.ObsoleteId2 );
		}

		public static bool IsModMismatchFound() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;

			if( modlock.MissingModNames.Count > 0 ) { return true; }
			if( mymod.Config.WorldModLockMinimumOnly && modlock.ExtraModNames.Count > 0 ) { return true; }

			return false;
		}


		public static void LockWorld() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = mymod.GetModWorld<ModHelpersWorld>();

			IEnumerable<Mod> allMods = ModListHelpers.GetAllLoadedModsPreferredOrder();
			ISet<string> modNames = new HashSet<string>();

			foreach( Mod mod in allMods ) {
				modNames.Add( mod.Name );
			}

			modlock.WorldModLocks[ modworld.ObsoleteId2 ] = modNames;

			modlock.ScanMods( modworld );

			if( mymod.Config.WorldModLockMinimumOnly ) {
				Main.NewText( "Your world now requires exactly these mods: " + string.Join( ", ", modNames ) );
			} else {
				Main.NewText( "Your world now requires at least these mods: " + string.Join( ", ", modNames ) );
			}
		}

		public static void UnlockWorld() {
			var mymod = ModHelpersMod.Instance;
			var modlock = mymod.ModLock;
			var modworld = mymod.GetModWorld<ModHelpersWorld>();

			modlock.WorldModLocks.Remove( modworld.ObsoleteId2 );
			modlock.MismatchBroadcastMade = false;

			modlock.ScanMods( modworld );

			Main.NewText( "Your world now has no mod requirement." );
		}
	}
}
