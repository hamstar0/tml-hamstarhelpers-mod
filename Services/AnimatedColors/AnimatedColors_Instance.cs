using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	/// <summary>
	/// Supplies a simple, handy way to "animate" (lerp between) colors to make animations. Adjustable.
	/// </summary>
	public partial class AnimatedColors {
		/// <summary>
		/// Duration (in ticks) to spend between each color.
		/// </summary>
		public float ColorDuration { get; private set; }
		/// <summary>
		/// Percent of duration elapsed between any given 2 colors.
		/// </summary>
		public float Progress { get; private set; }
		/// <summary>
		/// Which color in the cycle is being animated.
		/// </summary>
		public int CyclePosition { get; private set; }
		/// @private
		public Color CurrentColor { get; private set; }
		/// <summary>
		/// Colors sequence to animate with.
		/// </summary>
		public IReadOnlyList<Color> Colors { get; private set; }



		////////////////

		/// <summary></summary>
		/// <param name="colorDuration"></param>
		/// <param name="colors"></param>
		public AnimatedColors( int colorDuration, Color[] colors ) {
			this.ColorDuration = colorDuration;
			this.CyclePosition = 0;
			this.Progress = 0;
			this.Colors = new List<Color>( colors ).AsReadOnly();
		}

		internal void AdvanceColor() {
			Color c1 = this.Colors[ this.CyclePosition ];
			Color c2 = this.Colors[ (this.CyclePosition + 1) % this.Colors.Count];

			this.CurrentColor = Color.Lerp( c1, c2, this.Progress / this.ColorDuration );

			this.Progress += 1;
			if( this.Progress >= this.ColorDuration ) {
				this.Progress = 0;

				this.CyclePosition += 1;
				if( this.CyclePosition >= this.Colors.Count ) { this.CyclePosition = 0; }
			}
		}
	}
}
