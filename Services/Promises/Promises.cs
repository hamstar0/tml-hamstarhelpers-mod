using HamstarHelpers.Helpers.Debug;
using System;
using Terraria;


namespace HamstarHelpers.Services.Promises {
	public partial class Promises {
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
