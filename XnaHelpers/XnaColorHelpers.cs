using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.XnaHelpers {
	public static class XnaColorHelpers {
		/*public static Color Add( Color color, float amt, bool also_alpha ) {
			float high = also_alpha ? ( color.R > color.G ?
					(color.R > color.B ? (color.R > color.A ? color.R : color.A) : (color.B > color.A ? color.B : color.A)) :
					(color.G > color.B ? (color.G > color.A ? color.G : color.A) : (color.B > color.A ? color.B : color.A)) )
				: ( color.R > color.G ?
					(color.R > color.B ? color.R : color.B) :
					(color.G > color.B ? color.G : color.B) );

			float r_scale = (float)color.R / high;
			float g_scale = (float)color.G / high;
			float b_scale = (float)color.B / high;
			float a_scale = (float)color.A / high;

			color.R = (byte)MathHelper.Clamp( color.R + (amt * r_scale), 0, 255 );
			color.G = (byte)MathHelper.Clamp( color.G + (amt * g_scale), 0, 255 );
			color.B = (byte)MathHelper.Clamp( color.B + (amt * b_scale), 0, 255 );
			if( also_alpha ) {
				color.A = (byte)MathHelper.Clamp( color.A + (amt * a_scale), 0, 255 );
			}

			return color;
		}*/	// <- plz return brain to owner!

		public static Color Add( Color c1, int amt, bool also_alpha ) {
			byte cr = (byte)Math.Min( 255, c1.R + amt );
			byte cg = (byte)Math.Min( 255, c1.G + amt );
			byte cb = (byte)Math.Min( 255, c1.B + amt );
			byte ca = c1.A;

			if( also_alpha ) {
				ca = (byte)Math.Min( 255, c1.A + amt );
			}

			return new Color( cr, cg, cb, ca );
		}


		public static Color Add( Color c1, Color c2, bool also_alpha ) {
			float scale = (float)c2.A / 255f;

			byte cr = (byte)(c1.R + ((float)c2.R * scale));
			byte cg = (byte)(c1.G + ((float)c2.G * scale));
			byte cb = (byte)(c1.B + ((float)c2.B * scale));
			byte ca = c1.A;

			if( also_alpha ) {
				ca += (byte)( (float)(255 - c1.A) * scale );
			}

			return new Color( cr, cg, cb, ca );
		}

		public static Color Mul( Color color, float amt ) {
			color.R = (byte)MathHelper.Clamp( (float)color.R * amt, 0, 255 );
			color.G = (byte)MathHelper.Clamp( (float)color.G * amt, 0, 255 );
			color.B = (byte)MathHelper.Clamp( (float)color.B * amt, 0, 255 );
			color.A = (byte)MathHelper.Clamp( (float)color.A * amt, 0, 255 );
			return color;
		}


		public static Color BlendInto( Color to, Color from ) {
			float r = (float)from.R / 255f;
			float g = (float)from.G / 255f;
			float b = (float)from.B / 255f;
			float a = (float)from.A / 255f;
			float na = 1f - a;
			
			to.R = (byte)Math.Min( 255, ((float)to.R * na) + (r * a) );
			to.G = (byte)Math.Min( 255, ((float)to.G * na) + (g * a) );
			to.B = (byte)Math.Min( 255, ((float)to.B * na) + (b * a) );

			return to;
		}


		public static float Brightness( Color c, bool also_alpha ) {
			float scale = (float)c.A / 255f;
			float r = (float)c.R * scale;
			float g = (float)c.G * scale;
			float b = (float)c.B * scale;
			float a = also_alpha ? scale : 1f;
			return ( (0.299f * r) + (0.587f * g) + (0.114f * b) ) * a;
			//return (r + r + r + b + g + g + g + g) >> 3;
		}
	}
}
