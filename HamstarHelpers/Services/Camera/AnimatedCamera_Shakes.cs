using System;
using Terraria;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating changes of state for the player's 'camera' (screen position).
	/// </summary>
	public partial class AnimatedCamera : ILoadable {
		/// <summary>
		/// Applies a shaking motion to the camera. Fades in and out.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="peakMagnitude"></param>
		/// <param name="tickDuration"></param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginShakeSequence( string name, float peakMagnitude, int tickDuration, int skippedTicks = 0 ) {
			var anim = AnimatedCamera.Instance;
			anim.CurrentShakeSequence = name;
			anim.ShakePeakMagnitude = peakMagnitude;
			anim.ShakeTickDuration = tickDuration;
			anim.ShakeTicksElapsed = skippedTicks;
		}

		/// <summary>
		/// Applies a shaking motion to the camera. Fades in and out.
		/// </summary>
		/// <param name="peakMagnitude"></param>
		/// <param name="tickDuration"></param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginShakeSequence( float peakMagnitude, int tickDuration, int skippedTicks = 0 ) {
			AnimatedCamera.BeginShakeSequence(
				"Default",
				peakMagnitude,
				tickDuration,
				skippedTicks
			);
		}



		////////////////

		private void AnimateShakes() {
			if( this.ShakeTicksElapsed++ >= this.ShakeTickDuration ) {
				this.ShakeTicksElapsed = 0;
				this.ShakeTickDuration = 0;

				Camera.ApplyShake( 0f );
				return;
			}

			float shakeScale = (float)this.ShakeTicksElapsed / (float)this.ShakeTickDuration;
			shakeScale = shakeScale - 0.5f;
			shakeScale = 0.5f - Math.Abs( shakeScale );
			shakeScale *= 2f;

			Camera.ApplyShake( this.ShakePeakMagnitude * shakeScale );
		}
	}
}
