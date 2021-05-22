using System;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;


namespace HamstarHelpers.Services.OverlaySounds {
	/// <summary>
	/// Provides a way to create ambient sound effects that overlay existing game sounds and music.
	/// </summary>
	public partial class OverlaySound {
		private (float VolumeOverride, float PanOverride, float PitchOverride, bool IsEnded) GetSoundLoopState() {
			return this.CustomCondition?.Invoke() ?? (1f, 0f, 0f, false);
		}

		private float GetBaseVolume() {
			if( this.FadeTicks == -1 ) {
				return 1f;
			}
			return 1f - ((float)this.ElapsedFadeTicks / (float)this.FadeTicks);
		}


		////////////////

		private void UpdateLoop() {
			var soundLoopState = this.GetSoundLoopState();

			if( soundLoopState.IsEnded ) {
				this.IsFadingOut = true;
			}

			if( !this.IsEndlessLoop && this.ElapsedTicks >= this.MaxPlayDurationTicks ) {
				this.IsFadingOut = true;
			}

			this.ElapsedTicks++;

			if( !this.IsFadingOut ) {
				if( this.ElapsedFadeTicks < this.FadeTicks ) {
					this.ElapsedFadeTicks++;
				}
			} else {
				if( this.ElapsedFadeTicks > 0 ) {
					this.ElapsedFadeTicks--;
				} else {
					this.StopImmediately();
				}
			}

			if( this.MyInstance != null ) {
				float vol = soundLoopState.VolumeOverride * this.GetBaseVolume();
				float pan = soundLoopState.PanOverride;
				float pit = soundLoopState.PitchOverride;

				if( this.MyInstance.State != SoundState.Playing ) {
					this.MyInstance.Volume = vol;
					this.MyInstance.Pan = pan;
					this.MyInstance.Pitch = pit;
					this.MyInstance.Play();
				}
				this.MyInstance.Volume = vol;
				this.MyInstance.Pan = pan;
				this.MyInstance.Pitch = pit;
			}
		}


		////////////////

		/// <summary></summary>
		public void Play() {
			this.IsFadingOut = false;
			this.ElapsedTicks = 0;
			this.ElapsedFadeTicks = 1;

			var soundLoopState = this.GetSoundLoopState();

			float volume = this.GetBaseVolume() * soundLoopState.VolumeOverride;

			if( this.MyInstance == null ) {
				int sndStyle = this.SourceMod.GetSoundSlot( SoundType.Custom, this.SoundPath );

				this.MyInstance = Main.PlaySound(
					type: (int)SoundType.Custom,
					x: -1,
					y: -1,
					Style: sndStyle,
					volumeScale: volume,
					pitchOffset: soundLoopState.PitchOverride );
				//this.MyInstance.IsLooped = true;	//<- Crashes?
			} else {
				this.MyInstance.Play();
				this.MyInstance.Volume = volume;
				this.MyInstance.Pitch = soundLoopState.PitchOverride;
				this.MyInstance.Pan = soundLoopState.PanOverride;
			}

			Timers.Timers.SetTimer( "OverlaySound_" + this.GetHashCode(), 1, false, () => {
				this.UpdateLoop();
				return this.ElapsedFadeTicks > 0;
			} );
		}


		////////////////

		/// <summary>
		/// Begins sound loop fade out.
		/// </summary>
		public void Stop() {
			this.ElapsedFadeTicks = this.ElapsedFadeTicks < this.MaxPlayDurationTicks
				? this.MaxPlayDurationTicks
				: this.ElapsedFadeTicks;
			this.IsFadingOut = true;
		}


		/// <summary>Stops sound loop without fadeout.</summary>
		public void StopImmediately() {
			Timers.Timers.UnsetTimer( "OverlaySound_" + this.GetHashCode() );

			this.ElapsedFadeTicks = this.MaxPlayDurationTicks;

			if( this.MyInstance != null ) {
				this.MyInstance.Stop( true );
				this.MyInstance = null;
			}
		}
	}
}
