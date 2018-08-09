using System;


namespace HamstarHelpers.Helpers.DebugHelpers {
	public static partial class DebugHelpers {
		[Obsolete( "use DebugHelpers.Print", true )]
		public static void SetDisplay( string msg_label, string msg, int duration ) {
			DebugHelpers.Print( msg_label, msg, duration );
		}
	}
}
