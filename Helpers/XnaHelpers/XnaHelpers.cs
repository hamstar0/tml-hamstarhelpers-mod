using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.XnaHelpers {
	public static class XnaHelpers {
		public static void ScanRectangleWithout( Func<int, int, bool> scanner, Rectangle rect, Rectangle notrect ) {
			int i, j;

			for( i=rect.X; i<(rect.X+rect.Width); i++ ) {
				for( j=rect.Y; j<(rect.Y+rect.Height); j++ ) {
					if( i > notrect.X && i <= (notrect.X+notrect.Width) ) {
						if( j > notrect.Y && j <= (notrect.Y + notrect.Height) ) {
							i = notrect.X + notrect.Width;
							if( i >= (rect.X+rect.Width) ) { break; }
						}
					}
					
					if( !scanner(i, j) ) { return; }
				}
			}
		}
	}
}
