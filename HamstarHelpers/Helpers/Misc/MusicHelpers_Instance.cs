using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;
using System;
using Terraria;
using Terraria.ModLoader.Audio;


namespace HamstarHelpers.Helpers.Misc {
	/// @private
	public partial class MusicHelpers {
		private float Scale = 1f;

		private Func<bool> OnTickGet;



		////////////////

		internal MusicHelpers() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += MusicHelpers._Update;

			LoadHooks.AddModUnloadHook( () => {
				try {
					Main.OnTick -= MusicHelpers._Update;
				} catch { }
			} );
		}

		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( mymod == null || mymod.MusicHelpers == null ) { return; }
			if( Main.dedServ ) { return; }
			
			if( mymod.MusicHelpers.OnTickGet() ) {
				mymod.MusicHelpers.Update();
			}
		}

		internal void Update() {
			if( this.Scale == 1f ) { return; }

			Music music = Main.music[ Main.curMusic ];
			float fade = Main.musicFade[ Main.curMusic ];

			if( music != null && music.IsPlaying ) {
				if( fade > this.Scale ) {
					Main.musicFade[ Main.curMusic ] = Math.Max( 0f, fade - 0.01f );
				} else {
					Main.musicFade[ Main.curMusic ] = Math.Min( 1f, fade + 0.01f );
				}
				//music.SetVariable( "Volume", fade * Main.musicVolume * this.Scale );
			}
			
			this.Scale = Math.Min( 1f, this.Scale + 0.05f );
		}
	}
}
