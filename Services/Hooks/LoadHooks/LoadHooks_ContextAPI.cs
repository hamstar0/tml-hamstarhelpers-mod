using HamstarHelpers.Helpers.Debug;
using System;


namespace HamstarHelpers.Services.LoadHooks {
	public partial class LoadHooks {
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

		public static void AddModUnloadHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			lock( LoadHooks.ModUnloadHookLock ) {
				mymod.LoadHooks.ModUnloadHooks.Add( action );
			}
		}


		////////////////

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

		public static void AddWorldLoadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldLoadEachHookLock ) {
				mymod.LoadHooks.WorldLoadEachHooks.Add( action );
			}
		}

		public static void AddPostWorldLoadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldLoadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldLoadEachHookLock ) {
				mymod.LoadHooks.PostWorldLoadEachHooks.Add( action );
			}
		}

		public static void AddWorldUnloadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldUnloadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldUnloadEachHookLock ) {
				mymod.LoadHooks.WorldUnloadEachHooks.Add( action );
			}
		}

		public static void AddPostWorldUnloadEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.PostWorldUnloadHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.PostWorldUnloadEachHookLock ) {
				mymod.LoadHooks.PostWorldUnloadEachHooks.Add( action );
			}
		}

		public static void AddWorldInPlayEachHook( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.LoadHooks.WorldInPlayHookConditionsMet ) {
				action();
			}
			lock( LoadHooks.WorldInPlayEachHookLock ) {
				mymod.LoadHooks.WorldInPlayEachHooks.Add( action );
			}
		}

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
