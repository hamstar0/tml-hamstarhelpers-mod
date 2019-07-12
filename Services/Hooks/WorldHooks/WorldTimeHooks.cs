using System;


namespace HamstarHelpers.Services.Hooks.WorldHooks {
	public partial class WorldTimeHooks {
		public static void AddDayHook( string name, Action callback ) {
			ModHelpersMod.Instance.WorldTimeHooks.DayHooks[name] = callback;
		}

		public static void AddNightHook( string name, Action callback ) {
			ModHelpersMod.Instance.WorldTimeHooks.NightHooks[name] = callback;
		}
	}
}
