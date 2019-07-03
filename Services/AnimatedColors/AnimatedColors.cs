using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	/// <summary>
	/// Supplies a simple, handy way to "animate" (lerp between) colors over time. Adjustable.
	/// </summary>
	public partial class AnimatedColors {
		public static AnimatedColors Alert => ModHelpersMod.Instance.AnimatedColors.Alert;
		public static AnimatedColors Strobe => ModHelpersMod.Instance.AnimatedColors.Strobe;
		public static AnimatedColors Fire => ModHelpersMod.Instance.AnimatedColors.Fire;
		public static AnimatedColors Water => ModHelpersMod.Instance.AnimatedColors.Water;
		public static AnimatedColors Air => ModHelpersMod.Instance.AnimatedColors.Air;
		public static AnimatedColors Ether => ModHelpersMod.Instance.AnimatedColors.Ether;
		public static AnimatedColors Disco => ModHelpersMod.Instance.AnimatedColors.Disco;
		public static AnimatedColors DiscoFast => ModHelpersMod.Instance.AnimatedColors.DiscoFast;



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
