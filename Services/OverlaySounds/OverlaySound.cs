using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Services.OverlaySounds {
	/// <summary>
	/// Provides a way to create ambient sound effects that overlay existing game sounds and music.
	/// </summary>
	public class OverlaySound {
		/// <summary></summary>
		/// <param name="soundPath"></param>
		/// <param name="fadeTicks"></param>
		/// <param name="repeatTicks"></param>
		/// <param name="randomAddedTicks"></param>
		/// <param name="repeats"></param>
		/// <param name="customCondition">Returns a volume scale float and a boolean to indicate if the sound has ended.</param>
		/// <returns></returns>
		public static OverlaySound Create(
				string soundPath,
				int fadeTicks,
				int repeatTicks,
				int randomAddedTicks,
				int repeats=-1,
				Func<(float VolumeScale, bool IsEnded)> customCondition=null ) {
			return new OverlaySound( soundPath, fadeTicks, repeatTicks, randomAddedTicks, repeats, customCondition );
		}



		////////////////

		private string SoundPath;
		private SoundEffectInstance MyInstance = null;

		private int ElapsedFadeTicks;
		private int Repeats;

		private int FadeTicks;
		private int RepeatTicks;
		private int RandomAddedTicks;
		private int RepeatsMax;

		private bool IsEnding = false;

		private Func<(float VolumeOverride, bool IsEnded)> CustomCondition = null;



		////////////////

		internal OverlaySound(
				string soundPath,
				int fadeTicks,
				int repeatTicks,
				int randomAddedTicks,
				int repeats,
				Func<(float VolumeOverride, bool IsEnded)> customCondition ) {
			this.SoundPath = soundPath;
			this.FadeTicks = fadeTicks;
			this.RepeatTicks = repeatTicks;
			this.RandomAddedTicks = randomAddedTicks;
			this.Repeats = repeats;
			this.RepeatsMax = repeats;
			this.CustomCondition = customCondition;
		}


		////////////////

		private int GetRepeatDurationTicks() {
			if( this.RandomAddedTicks <= 0 ) {
				return this.RepeatTicks;
			}
			return this.RepeatTicks + TmlHelpers.SafelyGetRand().Next( this.RandomAddedTicks );
		}


		////////////////

		internal void Update() {
			if( this.MyInstance == null ) {
				return;
			}

			(float VolumeOverride, bool IsEnded) state = this.CustomCondition?.Invoke() ?? (1f, true);

			if( state.IsEnded ) {
				this.IsEnding = true;
			}

			if( !this.IsEnding ) {
				if( this.ElapsedFadeTicks < this.FadeTicks ) {
					this.ElapsedFadeTicks++;
				}
			} else {
				if( this.ElapsedFadeTicks > 0 ) {
					this.ElapsedFadeTicks--;
				} else {
					this.Stop();
				}
			}

			this.MyInstance.Volume = (state.VolumeOverride * ( float)this.ElapsedFadeTicks) / (float)this.FadeTicks;
		}


		////////////////

		/// <summary></summary>
		public void Play() {
			string timerName = "OverlaySound_" + this.GetHashCode();

			Timers.Timers.SetTimer( timerName, this.GetRepeatDurationTicks(), () => {
				int soundSlot = ModHelpersMod.Instance.GetSoundSlot( SoundType.Custom, this.SoundPath );
				float initVolume = this.FadeTicks > 0 ? 0.01f : 1f;

				this.MyInstance = Main.PlaySound( (int)SoundType.Custom, -1, -1, soundSlot, initVolume );

				if( this.Repeats < 0 || this.Repeats-- > 0 ) {
					return true;
				} else {
					this.IsEnding = true;
					return this.ElapsedFadeTicks > 0;
				}
			} );

			if( this.FadeTicks > 0 ) {
				Timers.Timers.SetTimer( timerName + "_fade", 1, () => {
					this.Update();
					return this.ElapsedFadeTicks > 0;
				} );
			}
		}


		/// <summary></summary>
		public void Stop() {
			if( this.MyInstance == null ) {
				return;
			}

			string timerName = "OverlaySound_" + this.GetHashCode();
			Timers.Timers.UnsetTimer( timerName );
			Timers.Timers.UnsetTimer( timerName+"_fade" );

			this.Repeats = this.RepeatsMax;
			this.MyInstance.Stop( true );
			this.MyInstance = null;
		}
	}
}
