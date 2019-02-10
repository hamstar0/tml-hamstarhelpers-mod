using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	public class AnimatedColors {
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


		////////////////

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


	

	class AnimatedColorsManager {
		internal readonly AnimatedColors Alert;
		internal readonly AnimatedColors Strobe;
		internal readonly AnimatedColors Fire;
		internal readonly AnimatedColors Water;
		internal readonly AnimatedColors Air;
		internal readonly AnimatedColors Ether;
		internal readonly AnimatedColors Disco;
		internal readonly AnimatedColors DiscoFast;

		////

		internal IList<AnimatedColors> Defs = new List<AnimatedColors>();
		private Func<bool> OnTickGet;


		////////////////

		internal AnimatedColorsManager() {
			var mymod = ModHelpersMod.Instance;

			this.Alert = AnimatedColors.Create( this, 16, new Color[] { Color.Yellow, Color.Gray } );
			this.Strobe = AnimatedColors.Create( this, 16, new Color[] { Color.Black, Color.White } );
			this.Fire = AnimatedColors.Create( this, 16, new Color[] { Color.Red, Color.Yellow } );
			this.Water = AnimatedColors.Create( this, 16, new Color[] { Color.Blue, Color.Turquoise } );
			this.Air = AnimatedColors.Create( this, 16, new Color[] { Color.White, Color.Gray } );
			this.Ether = AnimatedColors.Create( this, 16, new Color[] { Color.MediumSpringGreen, Color.Gray } );
			Color green = Color.Lime;	// The colors lie!
			Color indigo = new Color( 147, 0, 255, 255 );
			Color violet = new Color( 255, 139, 255, 255 );
			this.Disco = AnimatedColors.Create( this, 56, new Color[] { Color.Red, Color.Orange, Color.Yellow, green, Color.Blue, indigo, violet } );
			this.DiscoFast = AnimatedColors.Create( this, 8, new Color[] { Color.Red, Color.Orange, Color.Yellow, green, Color.Blue, indigo, violet } );

			this.OnTickGet = Timers.Timers.MainOnTickGet();
			Main.OnTick += AnimatedColorsManager._Update;
		}

		~AnimatedColorsManager() {
			Main.OnTick -= AnimatedColorsManager._Update;
		}


		////////////////

		private static void _Update() {
			var mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.AnimatedColors == null ) { return; }

			if( mymod.AnimatedColors.OnTickGet() ) {
				mymod.AnimatedColors.Update();
			}
		}


		internal void Update() {
			foreach( var def in this.Defs ) {
				def.AdvanceColor();
			}
		}
	}
}
