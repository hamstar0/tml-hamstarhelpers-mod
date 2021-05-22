using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Services.Camera;
using HamstarHelpers.Libraries.Misc;


namespace HamstarHelpers.Classes.CameraAnimation {
	/// <summary>
	/// Represents a zooming animation sequence for the player's 'camera'.
	/// </summary>
	public class CameraZoomer : CameraAnimator {
		/// <summary></summary>
		public static CameraZoomer Current {
			get => CameraAnimationManager.Instance.CurrentZoomer;
			set => CameraAnimationManager.Instance.CurrentZoomer = value;
		}



		////////////////

		/// <summary></summary>
		public float ZoomFrom { get; private set; } = -1;

		/// <summary></summary>
		public float ZoomTo { get; private set; } = -1;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="zoomFrom"></param>
		/// <param name="zoomTo"></param>
		/// <param name="toDuration">How long (in ticks) the camera takes to reach peak zoom.</param>
		/// <param name="lingerDuration">How long (in ticks) to linger at peak zoom.</param>
		/// <param name="froDuration">How long (in ticks) the camera takes to return to start zoom.</param>
		/// <param name="isSmoothed">Makes the animation begin and end in a smooth (sine wave, S-curve) fashion.</param>
		/// <param name="onRun">Function to call while running.</param>
		/// <param name="onStop">Function to call on stop (not pause); either by completion or manual stop.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraZoomer(
					string name,
					float zoomFrom,
					float zoomTo,
					int toDuration,
					int lingerDuration,
					int froDuration,
					bool isSmoothed,
					Action onRun = null,
					Action onStop = null,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, isSmoothed, onRun, onStop, skippedTicks ) {
			this.ZoomFrom = zoomFrom;
			this.ZoomTo = zoomTo;
			this.OnRun = onRun;
		}


		////////////////
		
		/// <summary></summary>
		/// <param name="percent"></param>
		public void SetZoomFrom( float percent ) {
			this.ZoomFrom = percent;
		}
		
		/// <summary></summary>
		/// <param name="percent"></param>
		public void SetZoomTo( float percent ) {
			this.ZoomTo = percent;
		}


		////////////////

		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void ApplyAnimation( float percent ) {
			if( this.IsSmoothed ) {
				percent = (float)MathLibraries.CosineInterpolate( 0d, 1d, (double)percent );
			}

			float zoom = MathHelper.Lerp( this.ZoomFrom, this.ZoomTo, percent );

			Camera.ApplyZoom( zoom );
		}

		/// <summary></summary>
		protected override void EndAnimation() {
			Camera.ResetZoom();
		}
	}
}
