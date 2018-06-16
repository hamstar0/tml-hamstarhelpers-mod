using System;


namespace HamstarHelpers.Utilities.Timers {
	[Obsolete( "use Services.Timers", true )]
	public class Timers {
		[Obsolete( "use Services.Timers.SetTimer", true )]
		public static void SetTimer( string name, int tick_duration, Func<bool> action ) {
			Services.Timers.Timers.SetTimer( name, tick_duration, action );
		}
		[Obsolete( "use Services.Timers.GetTimerTickDuration", true )]
		public static int GetTimerTickDuration( string name ) {
			return Services.Timers.Timers.GetTimerTickDuration( name );
		}
		[Obsolete( "use Services.Timers.UnsetTimer", true )]
		public static void UnsetTimer( string name ) {
			Services.Timers.Timers.UnsetTimer( name );
		}
		[Obsolete( "use Services.Timers.ResetAll", true )]
		public static void ResetAll() {
			Services.Timers.Timers.ResetAll();
		}
	}
}
