using System;
using Terraria;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Represents a shaking animation sequence for the player's 'camera' (screen position).
	/// </summary>
	public class CameraShaker {
		/// <summary></summary>
		public static CameraShaker Current {
			get => CameraAnimationManager.Instance.CurrentShaker;
			set => CameraAnimationManager.Instance.CurrentShaker = value;
		}



		////////////////

		/// <summary></summary>
		public string Name { get; private set; }

		////

		/// <summary></summary>
		public float ShakePeakMagnitude { get; private set; } = 0f;

		////

		/// <summary></summary>
		public int TickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int TicksElapsed { get; private set; } = 0;

		/// <summary></summary>
		public int TicksLingerDuration { get; private set; } = 0;

		/// <summary>Note: Negative values indicate shaking still in progress.</summary>
		public int TicksLingerElapsed => this.TicksElapsed - this.TickDuration;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="peakMagnitude"></param>
		/// <param name="tickDuration"></param>
		/// <param name="lingerDuration"></param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraShaker(
					string name,
					float peakMagnitude,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			this.Name = name;
			this.ShakePeakMagnitude = peakMagnitude;
			this.TickDuration = tickDuration;
			this.TicksElapsed = skippedTicks;
			this.TicksLingerDuration = lingerDuration;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public bool IsAnimating() {
			return this.TicksElapsed < this.TickDuration
				&& (this.TicksLingerElapsed <= 0 || this.TicksLingerElapsed < this.TicksLingerDuration);
		}


		////////////////
		
		/// <summary></summary>
		public void Stop() {
			this.TicksElapsed = 0;
			this.TickDuration = 0;
			this.TicksLingerDuration = 0;
		}


		////////////////

		internal bool Animate() {
			if( this.TicksElapsed++ >= (this.TickDuration + this.TicksLingerDuration) ) {
				this.Stop();
				Camera.ApplyShake( 0f );
				return false;
			}

			float shakeScale = Math.Min( (float)this.TicksElapsed / (float)this.TickDuration, 1f );
			shakeScale = shakeScale - 0.5f;
			shakeScale = 0.5f - Math.Abs( shakeScale );
			shakeScale *= 2f;

			Camera.ApplyShake( this.ShakePeakMagnitude * shakeScale );
			return true;
		}
	}
}
