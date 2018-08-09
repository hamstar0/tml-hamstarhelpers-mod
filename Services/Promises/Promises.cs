using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
		public static void AddPostModLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.Promises.PostModLoadPromises.Add( action );
			}
		}

		public static void AddModUnloadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			mymod.Promises.ModUnloadPromises.Add( action );
		}

		////////////////

		public static void AddWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;
			
			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.Promises.WorldLoadOncePromises.Add( action );
			}
		}
		
		public static void AddWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.WorldLoadEachPromises.Add( action );
		}

		public static void AddPostWorldLoadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.PostWorldLoadOncePromises.Add( action );
		}

		public static void AddPostWorldLoadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.PostWorldLoadEachPromises.Add( action );
		}

		////////////////

		public static void AddWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.WorldUnloadOncePromises.Add( action );
		}

		public static void AddWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.WorldUnloadEachPromises.Add( action );
		}


		public static void AddPostWorldUnloadOncePromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.PostWorldUnloadOncePromises.Add( action );
		}

		public static void AddPostWorldUnloadEachPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostWorldUnloadPromiseConditionsMet ) {
				action();
			}
			mymod.Promises.PostWorldUnloadEachPromises.Add( action );
		}
	}
}
