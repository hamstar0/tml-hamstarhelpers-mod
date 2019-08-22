using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	/// <summary>
	/// Supplies a handy way to "animate" (lerp between) colors to make animations. Adjustable.
	/// </summary>
	public partial class AnimatedColors {
		/// <summary>
		/// Color animation preset for alert-type effects.
		/// </summary>
		public static AnimatedColors Alert => ModHelpersMod.Instance.AnimatedColors.Alert;
		/// <summary>
		/// Color animation preset for glowing ember-like effect.
		/// </summary>
		public static AnimatedColors Ember => ModHelpersMod.Instance.AnimatedColors.Ember;
		/// <summary>
		/// Color animation preset to make a strobe-like effect.
		/// </summary>
		public static AnimatedColors Strobe => ModHelpersMod.Instance.AnimatedColors.Strobe;
		/// <summary>
		/// Color animation preset to resemble fire.
		/// </summary>
		public static AnimatedColors Fire => ModHelpersMod.Instance.AnimatedColors.Fire;
		/// <summary>
		/// Color animation preset of something water-like.
		/// </summary>
		public static AnimatedColors Water => ModHelpersMod.Instance.AnimatedColors.Water;
		/// <summary>
		/// Color animation preset of something air-themed.
		/// </summary>
		public static AnimatedColors Air => ModHelpersMod.Instance.AnimatedColors.Air;
		/// <summary>
		/// Color animation preset of something etherial.
		/// </summary>
		public static AnimatedColors Ether => ModHelpersMod.Instance.AnimatedColors.Ether;
		/// <summary>
		/// Color animation preset of something disco-like.
		/// </summary>
		public static AnimatedColors Disco => ModHelpersMod.Instance.AnimatedColors.Disco;
		/// <summary>
		/// Color animation preset of something disco-like at high speed.
		/// </summary>
		public static AnimatedColors DiscoFast => ModHelpersMod.Instance.AnimatedColors.DiscoFast;



		////////////////

		/// <summary>
		/// Creates an animated color object.
		/// </summary>
		/// <param name="durationPerColor">Ticks of time to spend between each color.</param>
		/// <param name="colors">Sequence of colors to loop through.</param>
		/// <returns></returns>
		public static AnimatedColors Create( int durationPerColor, Color[] colors ) {
			return AnimatedColors.Create( ModHelpersMod.Instance.AnimatedColors, durationPerColor, colors );
		}

		internal static AnimatedColors Create( AnimatedColorsManager mngr, int durationPerColor, Color[] colors ) {
			var def = new AnimatedColors( durationPerColor, colors );

			mngr.AddAnimation( def );

			return def;
		}
	}
}
