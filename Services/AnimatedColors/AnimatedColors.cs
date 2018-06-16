using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace HamstarHelpers.Services.AnimatedColor {
	public class AnimatedColors {
		public static AnimatedColors Alert { get { return HamstarHelpersMod.Instance.AnimatedColors.Alert; } }
		public static AnimatedColors Strobe { get { return HamstarHelpersMod.Instance.AnimatedColors.Strobe; } }
		public static AnimatedColors Fire { get { return HamstarHelpersMod.Instance.AnimatedColors.Fire; } }
		public static AnimatedColors Water { get { return HamstarHelpersMod.Instance.AnimatedColors.Water; } }
		public static AnimatedColors Air { get { return HamstarHelpersMod.Instance.AnimatedColors.Air; } }


		////////////////

		public static AnimatedColors Create( int duration_per_color, Color[] colors ) {
			return AnimatedColors.Create( HamstarHelpersMod.Instance.AnimatedColors, duration_per_color, colors );
		}

		internal static AnimatedColors Create( AnimatedColorsManager mngr, int duration_per_color, Color[] colors ) {
			var def = new AnimatedColors( duration_per_color, colors );

			mngr.Defs.Add( def );

			return def;
		}


		////////////////

		public float ColorDuration { get; private set; }
		public float Progress { get; private set; }
		public int CyclePosition { get; private set; }
		public Color[] Colors { get; private set; }
		public Color CurrentColor { get; private set; }


		////////////////

		public AnimatedColors( int color_duration, Color[] colors ) {
			this.ColorDuration = color_duration;
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


	

	class AnimatedColorsManager {
		internal readonly AnimatedColors Alert;
		internal readonly AnimatedColors Strobe;
		internal readonly AnimatedColors Fire;
		internal readonly AnimatedColors Water;
		internal readonly AnimatedColors Air;


		internal IList<AnimatedColors> Defs = new List<AnimatedColors>();


		internal AnimatedColorsManager() {
			this.Alert = AnimatedColors.Create( this, 16, new Color[] { Color.Yellow, Color.Gray } );
			this.Strobe = AnimatedColors.Create( this, 16, new Color[] { Color.Black, Color.White } );
			this.Fire = AnimatedColors.Create( this, 16, new Color[] { Color.Red, Color.Yellow } );
			this.Water = AnimatedColors.Create( this, 16, new Color[] { Color.Blue, Color.Turquoise } );
			this.Air = AnimatedColors.Create( this, 16, new Color[] { Color.White, Color.Gray } );
		}

		internal void Update() {
			foreach( var def in this.Defs ) {
				def.AdvanceColor();
			}
		}
	}
}
