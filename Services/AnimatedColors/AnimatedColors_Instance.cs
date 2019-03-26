using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	public partial class AnimatedColors {
		public float ColorDuration { get; private set; }
		public float Progress { get; private set; }
		public int CyclePosition { get; private set; }
		public Color[] Colors { get; private set; }
		public Color CurrentColor { get; private set; }



		////////////////

		public AnimatedColors( int colorDuration, Color[] colors ) {
			this.ColorDuration = colorDuration;
			this.CyclePosition = 0;
			this.Progress = 0;
			this.Colors = colors;
		}

		internal void AdvanceColor() {
			Color c1 = this.Colors[ this.CyclePosition ];
			Color c2 = this.Colors[ (this.CyclePosition + 1) % this.Colors.Length ];

			this.CurrentColor = Color.Lerp( c1, c2, this.Progress / this.ColorDuration );

			this.Progress += 1;
			if( this.Progress >= this.ColorDuration ) {
				this.Progress = 0;

				this.CyclePosition += 1;
				if( this.CyclePosition >= this.Colors.Length ) { this.CyclePosition = 0; }
			}
		}
	}
}
