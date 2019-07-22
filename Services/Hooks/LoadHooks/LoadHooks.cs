using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// <summary>
	/// Allows defining load hooks. These are of a set of presets corresponding to important game code "load" events.
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
	}
}
