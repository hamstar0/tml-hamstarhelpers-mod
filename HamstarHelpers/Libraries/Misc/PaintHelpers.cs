using HamstarHelpers.Libraries.XNA;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Libraries.Misc {
	/// <summary>
	/// Assorted static "helper" functions pertaining to game music.
	/// </summary>
	public class PaintLibraries {
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

			if( PaintLibraries.CachedMatches.ContainsKey( colorCode ) ) {
				return PaintLibraries.CachedMatches[ colorCode ];
			}

			float minVal = 9999;
			int minIdx = 0;

			for( int i=1; i<=30; i++ ) {
				Color compare = WorldGen.paintColor( i );
				Color dist = XNAColorLibraries.DistanceRGB( color, compare );
				float distAmt = Math.Abs( XNAColorLibraries.SumRGB( dist ) );

				if( minVal > distAmt ) {
					minVal = distAmt;
					minIdx = i;
				}
			}

			PaintLibraries.CachedMatches[ colorCode ] = (byte)minIdx;

			return (byte)minIdx;
		}
	}
}
