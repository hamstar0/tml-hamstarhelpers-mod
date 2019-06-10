using HamstarHelpers.Helpers.XNA;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Misc {
	public class PaintHelpers {
		private static IDictionary<int, byte> CachedMatches = new Dictionary<int, byte>();	// Static; shouldn't expect to change



		////////////////
		
		public static byte GetNearestPaintType( Color color ) {
			int colorCode = (((int)color.R >> 3) << 3) + (((int)color.G >> 3) << 11) + (((int)color.B >> 3) << 19 );

			if( PaintHelpers.CachedMatches.ContainsKey( colorCode ) ) {
				return PaintHelpers.CachedMatches[ colorCode ];
			}

			float minVal = 9999;
			int minIdx = 0;

			for( int i=1; i<=30; i++ ) {
				Color compare = WorldGen.paintColor( i );
				Color diff = XnaColorHelpers.DifferenceRGB( color, compare );
				float diffAmt = Math.Abs( XnaColorHelpers.SumRGB( diff ) );

				if( minVal > diffAmt ) {
					minVal = diffAmt;
					minIdx = i;
				}
			}

			PaintHelpers.CachedMatches[ colorCode ] = (byte)minIdx;

			return (byte)minIdx;
		}
	}
}
