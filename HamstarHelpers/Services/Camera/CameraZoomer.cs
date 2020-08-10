using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Represents a zooming animation sequence for the player's 'camera' (screen position).
	/// </summary>
	public class CameraZoomer {
		/// <summary></summary>
		public static CameraZoomer Current {
			get => CameraAnimationManager.Instance.CurrentZoomer;
			set => CameraAnimationManager.Instance.CurrentZoomer = value;
		}



		////////////////

		/// <summary></summary>
		public string Name { get; private set; }

		////

		/// <summary></summary>
		public float ZoomFrom { get; private set; } = -1;

		/// <summary></summary>
		public float ZoomTo { get; private set; } = -1;

		////

		/// <summary></summary>
		public int TickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int TicksElapsed { get; private set; } = 0;

		/// <summary></summary>
		public int TicksLingerDuration { get; private set; } = 0;

		/// <summary>Note: Negative values indicate zooming still in progress.</summary>
		public int TicksLingerElapsed => this.TicksElapsed - this.TickDuration;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="zoomFrom"></param>
		/// <param name="zoomTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraZoomer(
					string name,
					float zoomFrom,
					float zoomTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			this.Name = name;
			this.ZoomFrom = zoomFrom;
			this.ZoomTo = zoomTo;
			this.TickDuration = tickDuration;
			this.TicksElapsed = skippedTicks;
			this.TicksLingerDuration = lingerDuration;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public bool IsAnimating() {
			return this.TicksElapsed < this.TickDuration
				&& ( this.TicksLingerElapsed <= 0 || this.TicksLingerElapsed < this.TicksLingerDuration );
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public void Stop() {
			this.TicksElapsed = 0;
			this.TickDuration = 0;
			this.TicksLingerDuration = 0;
		}


		////////////////

		internal bool Animate() {
			if( this.TicksElapsed++ >= (this.TickDuration + this.TicksLingerDuration) ) {
				this.Stop();
				Camera.ApplyZoom( -1f );
				return false;
			}

			float percent = (float)this.TicksElapsed / (float)this.TickDuration;
			float zoom = MathHelper.Lerp( this.ZoomFrom, this.ZoomTo, Math.Min( percent, 1f ) );

			Camera.ApplyZoom( zoom );
			return true;
		}
	}
}
