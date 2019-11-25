using System;
using Terraria;


namespace HamstarHelpers.Helpers.World {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the current world's state.
	/// </summary>
	public partial class WorldStateHelpers {
		/// <summary>
		/// Tick duration of a daytime period.
		/// </summary>
		public readonly static int VanillaDayDuration = 54000;
		/// <summary>
		/// Tick duration of a nighttime period.
		/// </summary>
		public readonly static int VanillaNightDuration = 32400;



		////////////////

		/// <summary>
		/// Indicates if an invasion is in session.
		/// </summary>
		/// <returns></returns>
		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}


		////////////////

		/// <summary>
		/// Returns elapsed time having played the current world (current session).
		/// </summary>
		/// <returns></returns>
		public static int GetElapsedPlayTime() {
			return (int)(ModHelpersMod.Instance.WorldStateHelpers.TicksElapsed / 60);
		}

		/// <summary>
		/// Returns elapsed "half" days (day and night cycles; not actual halfway points of full day cycles).
		/// </summary>
		/// <returns></returns>
		public static int GetElapsedHalfDays() {
			return ModHelpersMod.Instance.WorldStateHelpers.HalfDaysElapsed;
		}

		/// <summary>
		/// Returns percent of day or night completed.
		/// </summary>
		/// <returns></returns>
		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / (double)WorldStateHelpers.VanillaDayDuration;
			} else {
				return Main.time / (double)WorldStateHelpers.VanillaNightDuration;
			}
		}
	}
}
