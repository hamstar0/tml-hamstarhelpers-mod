using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	public partial class AnimatedColors {
		public static AnimatedColors Alert { get { return ModHelpersMod.Instance.AnimatedColors.Alert; } }
		public static AnimatedColors Strobe { get { return ModHelpersMod.Instance.AnimatedColors.Strobe; } }
		public static AnimatedColors Fire { get { return ModHelpersMod.Instance.AnimatedColors.Fire; } }
		public static AnimatedColors Water { get { return ModHelpersMod.Instance.AnimatedColors.Water; } }
		public static AnimatedColors Air { get { return ModHelpersMod.Instance.AnimatedColors.Air; } }
		public static AnimatedColors Ether { get { return ModHelpersMod.Instance.AnimatedColors.Ether; } }
		public static AnimatedColors Disco { get { return ModHelpersMod.Instance.AnimatedColors.Disco; } }
		public static AnimatedColors DiscoFast { get { return ModHelpersMod.Instance.AnimatedColors.DiscoFast; } }



		////////////////

		public static AnimatedColors Create( int durationPerColor, Color[] colors ) {
			return AnimatedColors.Create( ModHelpersMod.Instance.AnimatedColors, durationPerColor, colors );
		}

		internal static AnimatedColors Create( AnimatedColorsManager mngr, int durationPerColor, Color[] colors ) {
			var def = new AnimatedColors( durationPerColor, colors );

			mngr.Defs.Add( def );

			return def;
		}
	}
}
