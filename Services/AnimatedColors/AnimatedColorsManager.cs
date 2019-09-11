using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.AnimatedColor {
	class AnimatedColorsManager {
		internal readonly AnimatedColors Alert;
		internal readonly AnimatedColors Ember;
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
			this.Ember = AnimatedColors.Create( this, 16, new Color[] { Color.Orange, Color.Orange * 0.65f } );
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

			if( !Main.dedServ ) {
				this.OnTickGet = Timers.Timers.MainOnTickGet();
				Main.OnTick += AnimatedColorsManager._Update;
			}
		}

		internal void OnPostSetupContent() {
			if( !Main.dedServ ) {
				LoadHooks.AddModUnloadHook( () => {
					Main.OnTick -= AnimatedColorsManager._Update;
				} );
			}
		}


		////////////////

		public void AddAnimation( AnimatedColors animation ) {
			this.Defs.Add( animation );
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
