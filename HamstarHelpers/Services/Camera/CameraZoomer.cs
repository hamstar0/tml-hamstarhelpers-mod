using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Camera {
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
		/// <param name="onTraversed">Function to call on reaching peak zoom.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraZoomer(
					string name,
					float zoomFrom,
					float zoomTo,
					int toDuration,
					int lingerDuration,
					int froDuration,
					Action onTraversed,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, onTraversed, skippedTicks ) {
			this.ZoomFrom = zoomFrom;
			this.ZoomTo = zoomTo;
			this.OnTraversed = onTraversed;
		}


		////////////////

		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void RunAnimation( float? percent ) {
			if( !percent.HasValue ) {
				Camera.ApplyZoom( -1 );
			} else {
				float zoom = MathHelper.Lerp( this.ZoomFrom, this.ZoomTo, percent.Value );
				Camera.ApplyZoom( zoom );
			}
		}
	}
}
