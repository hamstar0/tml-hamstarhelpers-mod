using System;


namespace HamstarHelpers.Helpers.DebugHelpers {
	public static partial class DebugHelpers {
		[Obsolete( "use DebugHelpers.Print", true )]
		public static void SetDisplay( string msgLabel, string msg, int duration ) {
			DebugHelpers.Print( msgLabel, msg, duration );
		}
	}
}
