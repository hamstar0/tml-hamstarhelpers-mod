using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.WorldHelpers {
	public static class WorldHelpers {
		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}


		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / 54000d;
			} else {
				return Main.time / 32400d;
			}
		}


		////////////////

		internal static IDictionary<string, Action> DayHooks = new Dictionary<string, Action>();
		internal static IDictionary<string, Action> NightHooks = new Dictionary<string, Action>();

		public static void AddDayHook( string name, Action callback ) {
			WorldHelpers.DayHooks[name] = callback;
		}

		public static void AddNightHook( string name, Action callback ) {
			WorldHelpers.NightHooks[name] = callback;
		}
	}
}
