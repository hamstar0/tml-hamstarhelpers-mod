using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;


namespace HamstarHelpers.TmlHelpers {
	[Obsolete("use Promises or LoadHelpers (respectively)", true)]
	public partial class TmlLoadHelpers {
		public static bool IsModLoaded() {
			return LoadHelpers.IsModLoaded();
		}
		public static bool IsWorldLoaded() {
			return LoadHelpers.IsWorldLoaded();
		}
		public static bool IsWorldBeingPlayed() {
			return LoadHelpers.IsWorldBeingPlayed();
		}
		public static bool IsWorldSafelyBeingPlayed() {
			return LoadHelpers.IsWorldSafelyBeingPlayed();
		}
		public static void AddPostModLoadPromise( Action action ) {
			Promises.AddPostModLoadPromise( action );
		}
		public static void AddModUnloadPromise( Action action ) {
			Promises.AddModUnloadPromise( action );
		}
		public static void AddWorldLoadOncePromise( Action action ) {
			Promises.AddWorldLoadOncePromise( action );
		}
		public static void AddWorldLoadEachPromise( Action action ) {
			Promises.AddWorldLoadEachPromise( action );
		}
		public static void AddPostWorldLoadOncePromise( Action action ) {
			Promises.AddPostWorldLoadOncePromise( action );
		}
		public static void AddPostWorldLoadEachPromise( Action action ) {
			Promises.AddPostWorldLoadEachPromise( action );
		}
		public static void AddWorldUnloadOncePromise( Action action ) {
			Promises.AddWorldUnloadOncePromise( action );
		}
		public static void AddWorldUnloadEachPromise( Action action ) {
			Promises.AddWorldUnloadEachPromise( action );
		}
		public static void AddPostWorldUnloadOncePromise( Action action ) {
			Promises.AddPostWorldUnloadOncePromise( action );
		}
		public static void AddPostWorldUnloadEachPromise( Action action ) {
			Promises.AddPostWorldUnloadEachPromise( action );
		}
		public static void AddCustomPromise( string name, Func<bool> action ) {
			Promises.AddCustomPromise( name, action );
		}
		public static void TriggerCustomPromise( string name ) {
			Promises.TriggerCustomPromise( name );
		}
		public static void ClearCustomPromise( string name ) {
			Promises.ClearCustomPromise( name );
		}
	}
}
