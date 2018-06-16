using Microsoft.Xna.Framework;
using System;


namespace HamstarHelpers.Utilities.AnimatedColor {
	[Obsolete( "use Services.AnimatedColors.AnimatedColors", true)]
	public class AnimatedColors {
		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Alert { get { return HamstarHelpersMod.Instance.AnimatedColors.Alert; } }
		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Strobe { get { return HamstarHelpersMod.Instance.AnimatedColors.Strobe; } }
		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Fire { get { return HamstarHelpersMod.Instance.AnimatedColors.Fire; } }
		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Water { get { return HamstarHelpersMod.Instance.AnimatedColors.Water; } }
		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Air { get { return HamstarHelpersMod.Instance.AnimatedColors.Air; } }


		////////////////

		public static HamstarHelpers.Services.AnimatedColor.AnimatedColors Create( int duration_per_color, Color[] colors ) {
			return HamstarHelpers.Services.AnimatedColor.AnimatedColors.Create( HamstarHelpersMod.Instance.AnimatedColors, duration_per_color, colors );
		}
	}
}
