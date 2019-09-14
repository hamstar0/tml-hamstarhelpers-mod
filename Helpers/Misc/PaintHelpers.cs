using HamstarHelpers.Helpers.XNA;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to game music.
	/// </summary>
	public class PaintHelpers {
		private static IDictionary<int, byte> CachedMatches = new Dictionary<int, byte>();  // Static; shouldn't expect to change



		////////////////

		/// <summary>
		/// Gets a paint type of the color with the nearest approximation of the given color.
		/// </summary>
		/// <param name="color"></param>
		/// <returns>Internal paint id (not item id).</returns>
		public static byte GetNearestPaintType( Color color ) {
			int colorCode = (((int)color.R >> 3) << 3) +
				(((int)color.G >> 3) << 11) +
				(((int)color.B >> 3) << 19 );

			if( PaintHelpers.CachedMatches.ContainsKey( colorCode ) ) {
				return PaintHelpers.CachedMatches[ colorCode ];
			}

			float minVal = 9999;
			int minIdx = 0;

			for( int i=1; i<=30; i++ ) {
				Color compare = WorldGen.paintColor( i );
				Color dist = XNAColorHelpers.DistanceRGB( color, compare );
				float distAmt = Math.Abs( XNAColorHelpers.SumRGB( dist ) );

				if( minVal > distAmt ) {
					minVal = distAmt;
					minIdx = i;
				}
			}

			PaintHelpers.CachedMatches[ colorCode ] = (byte)minIdx;

			return (byte)minIdx;
		}
	}
}
