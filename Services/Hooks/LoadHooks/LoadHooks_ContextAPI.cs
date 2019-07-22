using HamstarHelpers.Helpers.Debug;
using System;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// <summary>
	/// Allows defining load hooks. These are of a set of presets corresponding to important game code "load" events.
	/// </summary>
	public partial class LoadHooks {
		/// <summary>
		/// Declares an action to run after mods are loaded (PostSetupContent, PostAddRecipes, AddRecipeGroups).
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostModLoadHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.PostModLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostModLoadHookLock ) {
					mymod.LoadHooks.PostModLoadHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run as mods are unloading.
		/// </summary>
		/// <param name="action"></param>
		public static void AddModUnloadHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			lock( LoadHooks.ModUnloadHookLock ) {
				mymod.LoadHooks.ModUnloadHooks.Add( action );
			}
		}


		////////////////

		/// <summary>
		/// Declares an action to run as the current world loads. Action does not run for subsequent world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldLoadOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldLoadOnceHookLock ) {
					mymod.LoadHooks.WorldLoadOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run after the current world loads. Action does not run for subsequent world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldLoadOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostWorldLoadOnceHookLock ) {
					mymod.LoadHooks.PostWorldLoadOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run as the current world unloads. Action does not run for subsequent world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldUnloadOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldUnloadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldUnloadOnceHookLock ) {
					mymod.LoadHooks.WorldUnloadOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run after the current world unloads. Action does not run for subsequent world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldUnloadOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.PostWorldUnloadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.PostWorldUnloadOnceHookLock ) {
					mymod.LoadHooks.PostWorldUnloadOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run once the current world is in play. Action does not run for subsequent worlds.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldInPlayOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldInPlayHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.WorldInPlayOnceHookLock ) {
					mymod.LoadHooks.WorldInPlayOnceHooks.Add( action );
				}
			}
		}

		/// <summary>
		/// Declares an action to run after the current world is "safely" loaded (waits a few seconds to help avoid confusing
		/// errors). Action does not run for subsequent worlds.
		/// </summary>
		/// <param name="action"></param>
		public static void AddSafeWorldLoadOnceHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.SafeWorldLoadHookConditionsMet ) {
				action();
			} else {
				lock( LoadHooks.SafeWorldLoadOnceHookLock ) {
					mymod.LoadHooks.SafeWorldLoadOnceHooks.Add( action );
				}
			}
		}


		////////////////

		/// <summary>
		/// Declares an action to run as the current world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldLoadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldLoadEachHookLock ) {
				mymod.LoadHooks.WorldLoadEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run after the current world loads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldLoadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldLoadEachHookLock ) {
				mymod.LoadHooks.PostWorldLoadEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run as the current world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldUnloadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldUnloadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldUnloadEachHookLock ) {
				mymod.LoadHooks.WorldUnloadEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run after the current world unloads.
		/// </summary>
		/// <param name="action"></param>
		public static void AddPostWorldUnloadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.PostWorldUnloadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldUnloadEachHookLock ) {
				mymod.LoadHooks.PostWorldUnloadEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run once the current world is in play.
		/// </summary>
		/// <param name="action"></param>
		public static void AddWorldInPlayEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldInPlayHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldInPlayEachHookLock ) {
				mymod.LoadHooks.WorldInPlayEachHooks.Add( action );
			}
		}

		/// <summary>
		/// Declares an action to run after the current world is "safely" loaded (waits a few seconds to help avoid confusing
		/// errors).
		/// </summary>
		/// <param name="action"></param>
		public static void AddSafeWorldLoadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.SafeWorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.SafeWorldLoadEachHookLock ) {
				mymod.LoadHooks.SafeWorldLoadEachHooks.Add( action );
			}
		}
	}
}
