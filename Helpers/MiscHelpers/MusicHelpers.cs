using System;
using Terraria;
using Terraria.ModLoader.Audio;


namespace HamstarHelpers.MiscHelpers {
	public class MusicHelpers {
		public static void SetVolumeScale( float scale ) {
			HamstarHelpersMod.Instance.MusicHelpers.Scale = scale;
		}


		////////////////

		private float Scale = 1f;


		////////////////

		internal MusicHelpers() {
			Main.OnTick += this.UpdateMusic;
		}

		~MusicHelpers() {
			Main.OnTick -= this.UpdateMusic;
		}

		////////////////

		internal void UpdateMusic() {
			if( this.Scale == 1f ) { return; }

			Music music = Main.music[Main.curMusic];
			float fade = Main.musicFade[Main.curMusic];

			if( music.IsPlaying ) {
				if( fade > this.Scale ) {
					Main.musicFade[Main.curMusic] = Math.Max( 0f, fade - 0.01f );
				} else {
					Main.musicFade[Main.curMusic] = Math.Min( 1f, fade + 0.01f );
				}
				//music.SetVariable( "Volume", fade * Main.musicVolume * this.Scale );
			}

			this.Scale = Math.Min( 1f, this.Scale += 0.05f );
		}
	}
}
