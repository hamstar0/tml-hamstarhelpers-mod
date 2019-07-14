using System;


namespace HamstarHelpers.Services.Hooks.WorldHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for world-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class WorldTimeHooks {
		/// <summary>
		/// Declares an action to run when day begins.
		/// </summary>
		/// <param name="hookName"></param>
		/// <param name="callback"></param>
		/// <returns>`false` if the given hook has already been claimed.</returns>
		public static bool AddDayHook( string hookName, Action callback ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.WorldTimeHooks.DayHooks.ContainsKey(hookName) ) {
				return false;
			}

			mymod.WorldTimeHooks.DayHooks[hookName] = callback;

			return true;
		}

		/// <summary>
		/// Declares an action to run when day begins.
		/// </summary>
		/// <param name="hookName"></param>
		/// <param name="callback"></param>
		/// <returns>`false` if the given hook has already been claimed.</returns>
		public static bool AddNightHook( string hookName, Action callback ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.WorldTimeHooks.NightHooks.ContainsKey( hookName ) ) {
				return false;
			}

			mymod.WorldTimeHooks.NightHooks[hookName] = callback;

			return true;
		}
	}
}
