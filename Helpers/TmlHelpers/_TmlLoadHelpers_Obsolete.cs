using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.TmlHelpers {
	public partial class TmlLoadHelpers {
		[Obsolete( "use TmlLoadHelpers.IsModLoaded", true )]
		public static bool IsLoaded() {
			return LoadHelpers.LoadHelpers.IsModLoaded();
		}

		[Obsolete( "use TmlLoadHelpers.AddPostModLoadPromise", true )]
		public static void AddPostLoadPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostModLoadPromise( action );
		}

		[Obsolete( "use TmlLoadHelpers.AddPostModLoadPromise", true )]
		public static void AddPostGameLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.PostModLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.LoadHelpers.PostModLoadPromises.Add( action );
			}
		}

		[Obsolete( "use TmlLoadHelpers.AddWorldLoadOncePromise or AddWorldLoadEachPromise", true )]
		public static void AddWorldLoadPromise( Action action ) {
			var mymod = HamstarHelpersMod.Instance;

			if( mymod.LoadHelpers.WorldLoadPromiseConditionsMet ) {
				action();
			} else {
				mymod.LoadHelpers.WorldLoadOncePromises.Add( action );
			}
		}
	}
}
