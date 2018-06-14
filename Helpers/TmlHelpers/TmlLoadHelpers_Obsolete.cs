using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.TmlHelpers {
	public partial class TmlLoadHelpers {
		[Obsolete( "use TmlLoadHelpers.IsModLoaded", true )]
		public static bool IsLoaded() {
			return TmlLoadHelpers.IsModLoaded();
		}

		[Obsolete( "use TmlLoadHelpers.AddPostModLoadPromise", true )]
		public static void AddPostLoadPromise( Action action ) {
			TmlLoadHelpers.AddPostModLoadPromise( action );
		}

		[Obsolete( "use TmlLoadHelpers.AddPostModLoadPromise", true )]
		public static void AddPostGameLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.PostModLoadPromises.Add( action );
			}
		}

		[Obsolete( "use TmlLoadHelpers.AddWorldLoadOncePromise or AddWorldLoadEachPromise", true )]
		public static void AddWorldLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.TmlLoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.TmlLoadHelpers.WorldLoadOncePromises.Add( action );
			}
		}
	}
}
