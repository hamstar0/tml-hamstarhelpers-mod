using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.XNA {
	public static partial class XnaColorHelpers {
		public static Color Add( Color c1, int amt, bool alsoAlpha ) {
			byte cr = (byte)MathHelper.Clamp( (int)c1.R + amt, 0, 255 );
			byte cg = (byte)MathHelper.Clamp( (int)c1.G + amt, 0, 255 );
			byte cb = (byte)MathHelper.Clamp( (int)c1.B + amt, 0, 255 );
			byte ca = c1.A;

			if( alsoAlpha ) {
				ca = (byte)MathHelper.Clamp( (int)c1.A + amt, 0, 255 );
			}

			return new Color( cr, cg, cb, ca );
		}
		
		public static Color Add( Color c1, Color c2, bool alsoAlpha ) {
			int r = (int)c1.R + (int)c2.R;
			int g = (int)c1.G + (int)c2.G;
			int b = (int)c1.B + (int)c2.B;

			byte cr = (byte)MathHelper.Clamp( r, 0, 255 );
			byte cg = (byte)MathHelper.Clamp( g, 0, 255 );
			byte cb = (byte)MathHelper.Clamp( b, 0, 255 );
			byte ca = c1.A;

			if( alsoAlpha ) {
				int a = (int)c1.A + (int)c2.A;
				ca = (byte)MathHelper.Clamp( a, 0, 255 );
			}

			return new Color( cr, cg, cb, ca );
		}
		
		
		public static Color Mul( Color c1, Color c2 ) {
			byte r = (byte)( (float)c1.R * ((float)c2.R / 255f) );
			byte g = (byte)( (float)c1.G * ((float)c2.G / 255f) );
			byte b = (byte)( (float)c1.B * ((float)c2.B / 255f) );
			byte a = (byte)( (float)c1.A * ((float)c2.A / 255f) );

			return new Color( r, g, b, a );
		}

		
		public static float BrightnessRGB( Color c ) {
			float r = (float)c.R / 255f;
			float g = (float)c.G / 255f;
			float b = (float)c.B / 255f;

			return (0.299f * r) + (0.587f * g) + (0.114f * b);
			//return (r + r + r + b + g + g + g + g) >> 3;
		}


		public static Color DifferenceRGBA( Color c1, Color c2 ) {
			return new Color(
				Math.Abs((int)c1.R - (int)c2.R),
				Math.Abs((int)c1.G - (int)c2.G),
				Math.Abs((int)c1.B - (int)c2.B),
				Math.Abs((int)c1.A - (int)c2.A)
			);
		}
		public static Color DifferenceRGB( Color c1, Color c2 ) {
			return new Color(
				Math.Abs( (int)c1.R - (int)c2.R ),
				Math.Abs( (int)c1.G - (int)c2.G ),
				Math.Abs( (int)c1.B - (int)c2.B )
			);
		}

		public static int SumRGBA( Color c ) {
			return (int)c.R + c.G + (int)c.B + (int)c.A;
		}
		public static int SumRGB( Color c ) {
			return (int)c.R + c.G + (int)c.B;
		}

		public static float AvgRGBA( Color c ) {
			return (float)XnaColorHelpers.SumRGBA(c) * 0.25f;
		}
		public static float AvgRGB( Color c ) {
			return (float)XnaColorHelpers.SumRGB( c ) / 3f;
		}

		
		public static Color FlattenColor( Color bg, Color c ) {
			Color lerped = Color.Lerp( bg, c, (float)c.A / 255f );
			lerped.A = 255;
			return lerped;
		}
	}
}
