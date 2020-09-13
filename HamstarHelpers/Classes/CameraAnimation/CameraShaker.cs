using System;
using Terraria;
using HamstarHelpers.Services.Camera;
using HamstarHelpers.Helpers.Misc;


namespace HamstarHelpers.Classes.CameraAnimation {
	/// <summary>
	/// Represents a shaking animation sequence for the player's 'camera'.
	/// </summary>
	public class CameraShaker : CameraAnimator {
		/// <summary></summary>
		public static CameraShaker Current {
			get => CameraAnimationManager.Instance.CurrentShaker;
			set => CameraAnimationManager.Instance.CurrentShaker = value;
		}



		////////////////
		
		/// <summary></summary>
		public float ShakePeakMagnitude { get; private set; } = 0f;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="peakMagnitude"></param>
		/// <param name="toDuration">How long (in ticks) the camera takes to reach max shake magnitude.</param>
		/// <param name="lingerDuration">How long (in ticks) to linger at max magnitude.</param>
		/// <param name="froDuration">How long (in ticks) the camera takes to return to 0 magnitude.</param>
		/// <param name="isSmoothed">Makes the animation begin and end in a smooth (sine wave, S-curve) fashion.</param>
		/// <param name="onRun">Function to call while running.</param>
		/// <param name="onStop">Function to call on stop (not pause); either by completion or manual stop.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraShaker(
					string name,
					float peakMagnitude,
					int toDuration,
					int lingerDuration,
					int froDuration,
					bool isSmoothed,
					Action onRun = null,
					Action onStop = null,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, isSmoothed, onRun, onStop, skippedTicks ) {
			this.ShakePeakMagnitude = peakMagnitude;
			this.OnRun = onRun;
		}


		////////////////
		
		/// <summary></summary>
		/// <param name="magnitude"></param>
		public void SetPeakMagnitude( float magnitude ) {
			this.ShakePeakMagnitude = magnitude;
		}


		////////////////
		
		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void ApplyAnimation( float percent ) {
			if( this.IsSmoothed ) {
				percent = (float)MathHelpers.CosineInterpolate( 0d, 1d, (double)percent );
			}

			Camera.ApplyShake(
				magnitude: this.ShakePeakMagnitude * percent
			);
		}
		
		/// <summary></summary>
		protected override void EndAnimation() {
			Camera.ResetShake();
		}
	}
}
