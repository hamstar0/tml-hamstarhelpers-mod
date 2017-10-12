using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.DotNetHelpers {
	[System.Obsolete( "use XnaHelpers.XnaHelpers", true )]
	public static class XnaHelpers {
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle notrect ) {
			HamstarHelpers.XnaHelpers.XnaHelpers.ScanRectangleWithout( scanner, rect, notrect );
		}
	}
}
