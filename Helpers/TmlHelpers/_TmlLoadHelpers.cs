using HamstarHelpers.DebugHelpers;
using System;


namespace HamstarHelpers.TmlHelpers {
	[Obsolete("use TmlHelpers.LoadHelpers", true)]
	public partial class TmlLoadHelpers {
		public static bool IsModLoaded() {
			return LoadHelpers.LoadHelpers.IsModLoaded();
		}
		public static bool IsWorldLoaded() {
			return LoadHelpers.LoadHelpers.IsWorldLoaded();
		}
		public static bool IsWorldBeingPlayed() {
			return LoadHelpers.LoadHelpers.IsWorldBeingPlayed();
		}
		public static bool IsWorldSafelyBeingPlayed() {
			return LoadHelpers.LoadHelpers.IsWorldSafelyBeingPlayed();
		}
		public static void AddPostModLoadPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostModLoadPromise( action );
		}
		public static void AddModUnloadPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddModUnloadPromise( action );
		}
		public static void AddWorldLoadOncePromise( Action action ) {
			LoadHelpers.LoadHelpers.AddWorldLoadOncePromise( action );
		}
		public static void AddWorldLoadEachPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddWorldLoadEachPromise( action );
		}
		public static void AddPostWorldLoadOncePromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostWorldLoadOncePromise( action );
		}
		public static void AddPostWorldLoadEachPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostWorldLoadEachPromise( action );
		}
		public static void AddWorldUnloadOncePromise( Action action ) {
			LoadHelpers.LoadHelpers.AddWorldUnloadOncePromise( action );
		}
		public static void AddWorldUnloadEachPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddWorldUnloadEachPromise( action );
		}
		public static void AddPostWorldUnloadOncePromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostWorldUnloadOncePromise( action );
		}
		public static void AddPostWorldUnloadEachPromise( Action action ) {
			LoadHelpers.LoadHelpers.AddPostWorldUnloadEachPromise( action );
		}
		public static void AddCustomPromise( string name, Func<bool> action ) {
			LoadHelpers.LoadHelpers.AddCustomPromise( name, action );
		}
		public static void TriggerCustomPromise( string name ) {
			LoadHelpers.LoadHelpers.TriggerCustomPromise( name );
		}
		public static void ClearCustomPromise( string name ) {
			LoadHelpers.LoadHelpers.ClearCustomPromise( name );
		}
	}
}
