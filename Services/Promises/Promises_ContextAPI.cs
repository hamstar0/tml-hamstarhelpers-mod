using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostModLoadLock ) {
					mymod.Promises.PostModLoadPromises.Add( action );
				}
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			lock( Promises.ModUnloadLock ) {
				mymod.Promises.ModUnloadPromises.Add( action );
			}
		}


		////////////////

		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.WorldLoadOnceLock ) {
					mymod.Promises.WorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostWorldLoadOnceLock ) {
					mymod.Promises.PostWorldLoadOncePromises.Add( action );
				}
			}
		}

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.WorldUnloadOnceLock ) {
					mymod.Promises.WorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddPostWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			} else {
				lock( Promises.PostWorldUnloadOnceLock ) {
					mymod.Promises.PostWorldUnloadOncePromises.Add( action );
				}
			}
		}

		public static void AddSafePostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.SafeWorldLoadOnceLock ) {
				mymod.Promises.SafeWorldLoadOncePromises.Add( action );
			}
		}


		////////////////

		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.WorldLoadEachLock ) {
				mymod.Promises.WorldLoadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.PostWorldLoadEachLock ) {
				mymod.Promises.PostWorldLoadEachPromises.Add( action );
			}
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.WorldUnloadEachLock ) {
				mymod.Promises.WorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddPostWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.PostWorldUnloadEachLock ) {
				mymod.Promises.PostWorldUnloadEachPromises.Add( action );
			}
		}

		public static void AddSafeWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.SafeWorldLoadPromiseConditionsMet ) {
				action();
			}
			lock( Promises.SafeWorldLoadEachLock ) {
				mymod.Promises.SafeWorldLoadEachPromises.Add( action );
			}
		}
	}
}
