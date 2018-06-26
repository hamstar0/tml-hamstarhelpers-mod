using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;


namespace HamstarHelpers.TmlHelpers {
	public partial class TmlLoadHelpers {
		[Obsolete( "use LoadHelpers.IsModLoaded", true )]
		public static bool IsLoaded() {
			return LoadHelpers.IsModLoaded();
		}

		[Obsolete( "use Promises.AddPostModLoadPromise", true )]
		public static void AddPostLoadPromise( Action action ) {
			Promises.AddPostModLoadPromise( action );
		}

		[Obsolete( "use Promises.AddPostModLoadPromise", true )]
		public static void AddPostGameLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.Promises.PostModLoadPromises.Add( action );
			}
		}

		[Obsolete( "use Promises.AddWorldLoadOncePromise or AddWorldLoadEachPromise", true )]
		public static void AddWorldLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.Promises.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.Promises.WorldLoadOncePromises.Add( action );
			}
		}
	}
}
