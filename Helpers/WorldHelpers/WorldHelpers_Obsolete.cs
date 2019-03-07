using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.WorldHelpers {
	public partial class WorldHelpers {
		[Obsolete( "use WorldStateHelpers.VanillaDayDuration", true )]
		public readonly static int VanillaDayDuration = 54000;

		[Obsolete( "use WorldStateHelpers.VanillaNightDuration", true )]
		public readonly static int VanillaNightDuration = 32400;
		

		[Obsolete( "use WorldStateHelpers.IsBeingInvaded()", true )]
		public static bool IsBeingInvaded() {
			return WorldStateHelpers.IsBeingInvaded();
		}

		[Obsolete( "use WorldStateHelpers.GetElapsedPlayTime()", true )]
		public static int GetElapsedPlayTime() {
			return WorldStateHelpers.GetElapsedPlayTime();
		}

		[Obsolete( "use WorldStateHelpers.GetElapsedHalfDays()", true )]
		public static int GetElapsedHalfDays() {
			return WorldStateHelpers.GetElapsedHalfDays();
		}

		[Obsolete( "use WorldStateHelpers.GetDayOrNightPercentDone()", true )]
		public static double GetDayOrNightPercentDone() {
			return WorldStateHelpers.GetDayOrNightPercentDone();
		}

		[Obsolete( "use WorldStateHelpers.AddDayHook( string, Action )", true )]
		public static void AddDayHook( string name, Action callback ) {
			WorldStateHelpers.AddDayHook( name, callback );
		}

		[Obsolete( "use WorldStateHelpers.AddNightHook( string, Action )", true )]
		public static void AddNightHook( string name, Action callback ) {
			WorldStateHelpers.AddNightHook( name, callback );
		}
	}
}
