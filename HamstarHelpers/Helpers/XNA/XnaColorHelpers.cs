using System;
using HamstarHelpers.Helpers.Misc;
using Microsoft.Xna.Framework;


namespace HamstarHelpers.Helpers.XNA {
	/// <summary>
	/// Assorted static "helper" functions pertaining to XNA's Color struct. 
	/// </summary>
	public static partial class XNAColorHelpers {
		/// <summary>
		/// Adds to the RGB(A) of a color by a fixed amount.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="amt"></param>
		/// <param name="alsoAlpha"></param>
		/// <returns></returns>
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
		
		/// <summary>
		/// Adds 2 colors together directly.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <param name="alsoAlpha"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Multiplies 2 colors together, as if RGB(A) were percent values.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color Mul( Color c1, Color c2 ) {
			byte r = (byte)( (float)c1.R * ((float)c2.R / 255f) );
			byte g = (byte)( (float)c1.G * ((float)c2.G / 255f) );
			byte b = (byte)( (float)c1.B * ((float)c2.B / 255f) );
			byte a = (byte)( (float)c1.A * ((float)c2.A / 255f) );

			return new Color( r, g, b, a );
		}

		////////////////

		/// <summary>
		/// Adds one color to another so as to make it glow by that amount.
		/// </summary>
		/// <param name="baseColor"></param>
		/// <param name="glowColor"></param>
		/// <param name="alsoAlpha"></param>
		/// <returns></returns>
		public static Color AddGlow( Color baseColor, Color glowColor, bool alsoAlpha ) {
			float r2 = (float)glowColor.R / 255f;
			float g2 = (float)glowColor.G / 255f;
			float b2 = (float)glowColor.B / 255f;

			float glowR = (float)(255 - baseColor.R) * r2;
			float glowG = (float)(255 - baseColor.G) * g2;
			float glowB = (float)(255 - baseColor.B) * b2;

			var color = new Color(
				baseColor.R + (byte)glowR,
				baseColor.G + (byte)glowG,
				baseColor.B + (byte)glowB,
				baseColor.A
			);

			if( alsoAlpha ) {
				float a2 = (float)glowColor.A / 255f;
				float glowA = (float)( 255 - baseColor.A ) * a2;

				color.A = (byte)(baseColor.A + (byte)glowA);
			}

			return color;
		}


		////////////////

		/// <summary>
		/// Returns a percent value gauging the brightness of a given color (according to the human eye).
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static float BrightnessRGB( Color c ) {
			float r = (float)c.R / 255f;
			float g = (float)c.G / 255f;
			float b = (float)c.B / 255f;

			return (0.299f * r) + (0.587f * g) + (0.114f * b);
			//return (r + r + r + b + g + g + g + g) >> 3;
		}


		/// <summary>
		/// Subtracts 1 color from another as much as it will allow (including alpha channel).
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color SubtractRGBA( Color c1, Color c2 ) {
			return new Color(
				Math.Max( (int)c1.R - (int)c2.R, 0 ),
				Math.Max( (int)c1.G - (int)c2.G, 0 ),
				Math.Max( (int)c1.B - (int)c2.B, 0 ),
				Math.Max( (int)c1.A - (int)c2.A, 0 )
			);
		}
		/// <summary>
		/// Subtracts 1 color from another as much as it will allow (minus alpha channel).
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color SubtractRGB( Color c1, Color c2 ) {
			return new Color(
				Math.Max( (int)c1.R - (int)c2.R, 0 ),
				Math.Max( (int)c1.G - (int)c2.G, 0 ),
				Math.Max( (int)c1.B - (int)c2.B, 0 )
			);
		}


		/// <summary>
		/// Produces a color representing how far apart each RGBA channel is between 2 given colors.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color DistanceRGBA( Color c1, Color c2 ) {
			return new Color(
				Math.Abs( (int)c1.R - (int)c2.R ),
				Math.Abs( (int)c1.G - (int)c2.G ),
				Math.Abs( (int)c1.B - (int)c2.B ),
				Math.Abs( (int)c1.A - (int)c2.A )
			);
		}
		/// <summary>
		/// Produces a color representing how far apart each RGB channel is between 2 given colors.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color DistanceRGB( Color c1, Color c2 ) {
			return new Color(
				Math.Abs( (int)c1.R - (int)c2.R ),
				Math.Abs( (int)c1.G - (int)c2.G ),
				Math.Abs( (int)c1.B - (int)c2.B )
			);
		}


		/// <summary>
		/// Adds together a color's RGBA.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int SumRGBA( Color c ) {
			return (int)c.R + (int)c.G + (int)c.B + (int)c.A;
		}
		/// <summary>
		/// Adds together a color's RGB.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static int SumRGB( Color c ) {
			return (int)c.R + (int)c.G + (int)c.B;
		}

		/// <summary>
		/// Gets the averaged value of a color's RGBA.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static float AvgRGBA( Color c ) {
			return (float)XNAColorHelpers.SumRGBA(c) * 0.25f;
		}
		/// <summary>
		/// Gets the averaged value of a color's RGB.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static float AvgRGB( Color c ) {
			return (float)XNAColorHelpers.SumRGB( c ) / 3f;
		}

		
		/// <summary>
		/// Shifts (lerps) a given color into another by the amount of the other's opacity.
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static Color FlattenColor( Color c1, Color c2 ) {
			Color lerped = Color.Lerp( c1, c2, (float)c2.A / 255f );
			lerped.A = 255;
			return lerped;
		}


		////////////////

		/// <summary>
		/// Renders a color as a hex code string.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string RenderHex( Color color ) {
			return MiscHelpers.RenderColorHex( color );
		}
	}
}
