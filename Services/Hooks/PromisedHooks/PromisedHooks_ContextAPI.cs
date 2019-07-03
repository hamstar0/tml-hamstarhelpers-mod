using HamstarHelpers.Helpers.Debug;
using System;


namespace HamstarHelpers.Services.PromisedHooks {
	public partial class PromisedHooks {
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.PostModLoadLock ) {
					mymod.Promises.PostModLoadPromises.Add( action );
				}
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			lock( PromisedHooks.ModUnloadLock ) {
				mymod.Promises.ModUnloadPromises.Add( action );
			}
		}


		////////////////

		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.WorldLoadOnceLock ) {
					mymod.Promises.WorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.PostWorldLoadOnceLock ) {
					mymod.Promises.PostWorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.WorldUnloadOnceLock ) {
					mymod.Promises.WorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldUnloadOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.PostWorldUnloadOnceLock ) {
					mymod.Promises.PostWorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddWorldInPlayOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;
			
			if( mymod.Promises.WorldInPlayPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.WorldInPlayOnceLock ) {
					mymod.Promises.WorldInPlayOncePromises.Add( action );
				}
			}
		}

		public static void AddSafeWorldLoadOncePromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( PromisedHooks.SafeWorldLoadOnceLock ) {
					mymod.Promises.SafeWorldLoadOncePromises.Add( action );
				}
			}
		}


		////////////////

		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.WorldLoadEachLock ) {
				mymod.Promises.WorldLoadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.PostWorldLoadEachLock ) {
				mymod.Promises.PostWorldLoadEachPromises.Add( action );
			}
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.WorldUnloadEachLock ) {
				mymod.Promises.WorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldUnloadEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.PostWorldUnloadEachLock ) {
				mymod.Promises.PostWorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddWorldInPlayEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;
			
			if( mymod.Promises.WorldInPlayPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.WorldInPlayEachLock ) {
				mymod.Promises.WorldInPlayEachPromises.Add( action );
			}
		}

		public static void AddSafeWorldLoadEachPromise( Action action ) {
			var mymod = ModHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( PromisedHooks.SafeWorldLoadEachLock ) {
				mymod.Promises.SafeWorldLoadEachPromises.Add( action );
			}
		}
	}
}
