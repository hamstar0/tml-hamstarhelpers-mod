using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.XnaHelpers {
	public static class XnaColorHelpers {
		public static Color Add( Color c1, int amt, bool also_alpha ) {
			byte cr = (byte)MathHelper.Clamp( (int)c1.R + amt, 0, 255 );
			byte cg = (byte)MathHelper.Clamp( (int)c1.G + amt, 0, 255 );
			byte cb = (byte)MathHelper.Clamp( (int)c1.B + amt, 0, 255 );
			byte ca = c1.A;

			if( also_alpha ) {
				ca = (byte)MathHelper.Clamp( (int)c1.A + amt, 0, 255 );
			}

			return new Color( cr, cg, cb, ca );
		}
		
		public static Color Add( Color c1, Color c2, bool also_alpha ) {
			int r = (int)c1.R + (int)c2.R;
			int g = (int)c1.G + (int)c2.G;
			int b = (int)c1.B + (int)c2.B;

			byte cr = (byte)MathHelper.Clamp( r, 0, 255 );
			byte cg = (byte)MathHelper.Clamp( g, 0, 255 );
			byte cb = (byte)MathHelper.Clamp( b, 0, 255 );
			byte ca = c1.A;

			if( also_alpha ) {
				int a = (int)c1.A + (int)c2.A;
				ca = (byte)MathHelper.Clamp( a, 0, 255 );
			}

			return new Color( cr, cg, cb, ca );
		}



		[System.Obsolete( "use XnaColorHelpers.Mul( Color c1, Color c2 )", true )]
		public static Color BlendInto( Color to, Color from ) {
			return XnaColorHelpers.Mul( to, from );
		}
		
		[System.Obsolete( "use Color.Multiply( Color value, float scale )", true )]
		public static Color Mul( Color value, float scale ) {
			return Color.Multiply( value, scale );
		}
		
		public static Color Mul( Color c1, Color c2 ) {
			byte r = (byte)( (float)c1.R * ((float)c2.R / 255f) );
			byte g = (byte)( (float)c1.G * ((float)c2.G / 255f) );
			byte b = (byte)( (float)c1.B * ((float)c2.B / 255f) );
			byte a = (byte)( (float)c1.A * ((float)c2.A / 255f) );

			return new Color( r, g, b, a );
		}


		/*public static Color BlendIntoByAlpha( Color c1, Color c2 ) {
			float scale = (float)c2.A / 255f;
			float nscale = 1f - scale;

			byte r = (byte)Math.Min( 255, ((float)c1.R * nscale) + ((float)c2.R * scale) );
			byte g = (byte)Math.Min( 255, ((float)c1.G * nscale) + ((float)c2.G * scale) );
			byte b = (byte)Math.Min( 255, ((float)c1.B * nscale) + ((float)c2.B * scale) );
			//byte a = (byte)Math.Min( 255, (((int)to.A + (int)from.A) / 2) );
			byte a = c1.A;

			return new Color( r, g, b, a );
		}*/


		/*public static Color ShiftColors( Color color, float amt, bool also_alpha ) {
			//float scale = XnaColorHelpers.Brightness( color, also_alpha ) * 255f;
			float scale = (float)color.R + (float)color.G + (float)color.B;
			if( also_alpha ) { scale = (scale + (float)color.A) / 4f; }
			else { scale /= 3f; }

			if( scale == 0 ) { return color; }

			float r_scale = (float)color.R / scale;
			float g_scale = (float)color.G / scale;
			float b_scale = (float)color.B / scale;

			float r = (float)color.R + (amt * r_scale);
			float g = (float)color.G + (amt * g_scale);
			float b = (float)color.B + (amt * b_scale);

			color.R = (byte)MathHelper.Clamp( r, 0f, 255f );
			color.G = (byte)MathHelper.Clamp( g, 0f, 255f );
			color.B = (byte)MathHelper.Clamp( b, 0f, 255f );

			if( also_alpha ) {
				float a_scale = (float)color.A / scale;
				float a = (float)color.A + (amt * a_scale);

				color.A = (byte)MathHelper.Clamp( a, 0f, 255f );
			}

			return color;
		}*/


		public static float Brightness( Color c ) {
			float r = (float)c.R / 255f;
			float g = (float)c.G / 255f;
			float b = (float)c.B / 255f;

			return (0.299f * r) + (0.587f * g) + (0.114f * b);
			//return (r + r + r + b + g + g + g + g) >> 3;
		}
	}
}
