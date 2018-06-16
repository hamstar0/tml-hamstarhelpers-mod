using System;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
	public static class SystemHelpers {
		[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
		public static TimeSpan TimeStamp() {
			return HamstarHelpers.DotNetHelpers.SystemHelpers.TimeStamp();
		}
		[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
		public static long TimeStampInSeconds() {
			return HamstarHelpers.DotNetHelpers.SystemHelpers.TimeStampInSeconds();
		}
		[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
		public static string ConvertDecimalToRadix( long number, int radix ) {
			return HamstarHelpers.DotNetHelpers.SystemHelpers.ConvertDecimalToRadix( number, radix );
		}
		[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
		public static string ComputeSHA256Hash( string str ) {
			return HamstarHelpers.DotNetHelpers.SystemHelpers.ComputeSHA256Hash( str );
		}
		[Obsolete( "use DotNetHelpers.SystemHelpers", true )]
		public static void OpenUrl( string url ) {
			HamstarHelpers.DotNetHelpers.SystemHelpers.OpenUrl( url );
		}
	}
}
