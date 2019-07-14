using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.LoadHooks {
	/// <summary>
	/// Allows supplying delegate-based hooks that are called for custom events. Actions are guaranteed to run when the
	/// conditions are set, or when they come to pass. Preset hooks exist for major game loading and unloading events,
	/// but custom hooks may also be defined.
	/// </summary>
	public partial class LoadHooks {
		private static object PostModLoadHookLock = new object();
		private static object ModUnloadHookLock = new object();
		private static object WorldLoadOnceHookLock = new object();
		private static object WorldLoadEachHookLock = new object();
		private static object PostWorldLoadOnceHookLock = new object();
		private static object PostWorldLoadEachHookLock = new object();
		private static object WorldUnloadOnceHookLock = new object();
		private static object WorldUnloadEachHookLock = new object();
		private static object PostWorldUnloadOnceHookLock = new object();
		private static object PostWorldUnloadEachHookLock = new object();
		private static object WorldInPlayOnceHookLock = new object();
		private static object WorldInPlayEachHookLock = new object();
		private static object SafeWorldLoadOnceHookLock = new object();
		private static object SafeWorldLoadEachHookLock = new object();
		private static object ValidatedHookLock = new object();
	}
}
