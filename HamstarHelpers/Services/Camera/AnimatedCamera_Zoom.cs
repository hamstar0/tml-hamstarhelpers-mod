using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating changes of state for the player's 'camera' (screen position).
	/// </summary>
	public partial class AnimatedCamera : ILoadable {
		/// <summary>
		/// Applies a changing zoom to the camera over time.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="zoomFrom"></param>
		/// <param name="zoomTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginZoomSequence(
					string name,
					float zoomFrom,
					float zoomTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			var anim = AnimatedCamera.Instance;
			anim.CurrentZoomSequence = name;
			anim.ZoomFrom = zoomFrom;
			anim.ZoomTo = zoomTo;
			anim.ZoomTickDuration = tickDuration;
			anim.ZoomTicksElapsed = skippedTicks;
			anim.ZoomTicksLingerDuration = lingerDuration;
		}

		/// <summary>
		/// Applies a changing zoom to the camera over time.
		/// </summary>
		/// <param name="zoomFrom"></param>
		/// <param name="zoomTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginZoomSequence(
					float zoomFrom,
					float zoomTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			AnimatedCamera.BeginZoomSequence(
				"Default",
				zoomFrom,
				zoomTo,
				tickDuration,
				skippedTicks,
				lingerDuration
			);
		}



		////////////////
		
		private void AnimateZoom() {
			if( this.ZoomTicksElapsed++ >= (this.ZoomTickDuration + this.ZoomTicksLingerDuration) ) {
				this.ZoomTicksElapsed = 0;
				this.ZoomTickDuration = 0;
				this.ZoomTicksLingerDuration = 0;

				Camera.ApplyZoom( -1f );
				return;
			}

			float percent = (float)this.ZoomTicksElapsed / (float)this.ZoomTickDuration;
			float zoom = MathHelper.Lerp( this.ZoomFrom, this.ZoomTo, Math.Min( percent, 1f ) );

			Camera.ApplyZoom( zoom );
		}
	}
}
