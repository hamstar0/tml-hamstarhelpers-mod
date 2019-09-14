using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Helpers.XNA {
	/// <summary>
	/// Assorted static "helper" functions pertaining to XNA's Color struct. 
	/// </summary>
	public static partial class XNAColorHelpers {
		/// <summary>
		/// Gets the difference between 2 colors (including alpha channel).
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		[Obsolete("use SubtractRGBA", true)]
		public static Color DifferenceRGBA( Color c1, Color c2 ) {
			return XNAColorHelpers.SubtractRGBA( c1, c2 );
		}
		/// <summary>
		/// Get the difference between 2 colors (minus alpha channel).
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		[Obsolete( "use SubtractRGBA", true )]
		public static Color DifferenceRGB( Color c1, Color c2 ) {
			return XNAColorHelpers.SubtractRGB( c1, c2 );
		}
	}
}
