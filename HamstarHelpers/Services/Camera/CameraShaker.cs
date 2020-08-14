using System;
using Terraria;


namespace HamstarHelpers.Services.Camera {
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
		/// <param name="onTraversed">Function to call on reaching max magnitude.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraShaker(
					string name,
					float peakMagnitude,
					int toDuration,
					int lingerDuration,
					int froDuration,
					Action onTraversed,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, onTraversed, skippedTicks ) {
			this.ShakePeakMagnitude = peakMagnitude;
			this.OnTraversed = onTraversed;
		}


		////////////////
		
		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void ApplyAnimation( float percent ) {
			Camera.ApplyShake( this.ShakePeakMagnitude * percent );
		}
		
		/// <summary></summary>
		protected override void EndAnimation() {
			Camera.ResetShake();
		}
	}
}
