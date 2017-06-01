using Terraria;


namespace HamstarHelpers.WorldHelpers {
	public static class WorldHelpers {
		public static bool IsBeingInvaded() {
			return Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0;
		}


		public static double GetDayOrNightPercentDone() {
			if( Main.dayTime ) {
				return Main.time / 54000.0d;
			} else {
				return Main.time / 32400.0d;
			}
		}
	}
}
