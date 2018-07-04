using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.MiscHelpers {
	public class PaintHelpers {
		private static IDictionary<int, byte> CachedMatches = new Dictionary<int, byte>();



		////////////////

		public static byte GetNearestPaintType( Color color ) {
			int color_code = (((int)color.R >> 3) << 3) + (((int)color.G >> 3) << 11) + (((int)color.B >> 3) << 19 );

			if( PaintHelpers.CachedMatches.ContainsKey( color_code ) ) {
				return PaintHelpers.CachedMatches[ color_code ];
			}

			float min_val = 9999;
			int min_idx = 0;

			for( int i=1; i<=30; i++ ) {
				Color compare = WorldGen.paintColor( i );
				Color diff = XnaColorHelpers.DifferenceRGB( color, compare );
				float diff_amt = Math.Abs( XnaColorHelpers.SumRGB( diff ) );

				if( min_val > diff_amt ) {
					min_val = diff_amt;
					min_idx = i;
				}
			}

			PaintHelpers.CachedMatches[ color_code ] = (byte)min_idx;

			return (byte)min_idx;
		}
	}
}
