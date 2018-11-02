using HamstarHelpers.Helpers.TmlHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.Helpers.WorldHelpers {
	public partial class WorldStateHelpers {
		public readonly static int VanillaDayDuration = 54000;
		public readonly static int VanillaNightDuration = 32400;



		////////////////

		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}


		////////////////

		public static int GetElapsedPlayTime() {
			return (int)(ModHelpersMod.Instance.WorldStateHelpers.TicksElapsed / 60);
		}

		public static int GetElapsedHalfDays() {
			return ModHelpersMod.Instance.WorldStateHelpers.HalfDaysElapsed;
		}

		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / (double)WorldStateHelpers.VanillaDayDuration;
			} else {
				return Main.time / (double)WorldStateHelpers.VanillaNightDuration;
			}
		}


		////////////////
		
		public static void AddDayHook( string name, Action callback ) {
			ModHelpersMod.Instance.WorldStateHelpers.DayHooks[name] = callback;
		}

		public static void AddNightHook( string name, Action callback ) {
			ModHelpersMod.Instance.WorldStateHelpers.NightHooks[name] = callback;
		}
	}
}
