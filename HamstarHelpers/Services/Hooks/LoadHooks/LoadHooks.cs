using HamstarHelpers.Libraries.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.LoadHooks {
	/// <summary>
	/// Allows defining load hooks. These are of a set of hooks corresponding to important game code "load" events.
	/// </summary>
	public partial class LoadHooks {
		private readonly static object PostModLoadHookLock = new object();
		private readonly static object ModUnloadHookLock = new object();
		private readonly static object WorldLoadOnceHookLock = new object();
		private readonly static object WorldLoadEachHookLock = new object();
		private readonly static object PostWorldLoadOnceHookLock = new object();
		private readonly static object PostWorldLoadEachHookLock = new object();
		private readonly static object WorldUnloadOnceHookLock = new object();
		private readonly static object WorldUnloadEachHookLock = new object();
		private readonly static object PostWorldUnloadOnceHookLock = new object();
		private readonly static object PostWorldUnloadEachHookLock = new object();
		private readonly static object WorldInPlayOnceHookLock = new object();
		private readonly static object WorldInPlayEachHookLock = new object();
		private readonly static object SafeWorldLoadOnceHookLock = new object();
		private readonly static object SafeWorldLoadEachHookLock = new object();
	}
}
