using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.PromisedHooks {
	/// <summary>
	/// Provides delegate-based hooks for supplying actions that are performed upon events or conditions being satisfied. Actions are guaranteed
	/// to run when the conditions come to pass, or whenever the action for already-active conditions is supplied (whichever comes first; useful
	/// for asynchronous code). Preset hooks exist for major game loading and unloading events, but custom hooks may also be defined.
	/// </summary>
	public partial class PromisedHooks {
		private static object PostModLoadLock = new object();
		private static object ModUnloadLock = new object();
		private static object WorldLoadOnceLock = new object();
		private static object WorldLoadEachLock = new object();
		private static object PostWorldLoadOnceLock = new object();
		private static object PostWorldLoadEachLock = new object();
		private static object WorldUnloadOnceLock = new object();
		private static object WorldUnloadEachLock = new object();
		private static object PostWorldUnloadOnceLock = new object();
		private static object PostWorldUnloadEachLock = new object();
		private static object WorldInPlayOnceLock = new object();
		private static object WorldInPlayEachLock = new object();
		private static object SafeWorldLoadOnceLock = new object();
		private static object SafeWorldLoadEachLock = new object();
		 private static object ValidatedPromiseLock = new object();
	}
}
