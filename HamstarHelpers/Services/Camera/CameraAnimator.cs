using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Represents a sequence of controlled behavior for the player's 'camera'.
	/// </summary>
	public abstract class CameraAnimator {
		/// <summary>Called while camera is at destination.</summary>
		protected Action OnTraversed;


		////////////////

		/// <summary></summary>
		public string Name { get; private set; }

		////

		/// <summary></summary>
		public bool IsPaused { get; private set; } = false;

		////

		/// <summary></summary>
		public int ToTickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int LingerTicksDuration { get; private set; } = 0;

		/// <summary></summary>
		public int FroTickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int TotalTickDuration => this.ToTickDuration + this.LingerTicksDuration + this.FroTickDuration;

		////

		/// <summary></summary>
		public int TicksElapsed { get; private set; } = 0;

		/// <summary>Note: Negative values indicate moving still in progress.</summary>
		public int TicksLingerElapsed => this.TicksElapsed - this.ToTickDuration;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="toDuration">How long (in ticks) the camera state takes to go from A to B.</param>
		/// <param name="lingerDuration">How long (in ticks) to linger at destination (B).</param>
		/// <param name="froDuration">How long (in ticks) the camera state takes to go back from B to A.</param>
		/// <param name="onTraversed">Function to call on reaching destination (B).</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		protected CameraAnimator(
					string name,
					int toDuration,
					int lingerDuration,
					int froDuration,
					Action onTraversed = null,
					int skippedTicks = 0 ) {
			this.Name = name;
			this.ToTickDuration = toDuration;
			this.LingerTicksDuration = lingerDuration;
			this.FroTickDuration = froDuration;
			this.OnTraversed = onTraversed;
			this.TicksElapsed = skippedTicks;
		}


		////////////////

		/// <summary></summary>
		/// <returns></returns>
		public bool IsAnimating() {
			return this.TicksElapsed < this.TotalTickDuration;
		}


		////////////////

		/// <summary>Skips the given number of ticks into the animation. Accepts negative numbers.</summary>
		/// <param name="ticks"></param>
		public void Skip( int ticks ) {
			int total = this.TotalTickDuration;

			this.TicksElapsed += ticks;
			this.TicksElapsed = this.TicksElapsed < 0
				? 0
				: this.TicksElapsed > total
					? total
					: this.TicksElapsed;
		}

		////

		/// <summary></summary>
		/// <param name="pause"></param>
		public void Pause( bool pause ) {
			this.IsPaused = pause;
		}

		/// <summary></summary>
		public void Reset() {
			this.TicksElapsed = 0;
		}

		/// <summary></summary>
		public void Stop() {
			this.TicksElapsed = this.TotalTickDuration;
		}


		////////////////

		internal bool Animate() {
			if( !this.IsPaused ) {
				if( this.TicksElapsed++ >= this.TotalTickDuration ) {
					this.Stop();
					this.RunAnimation( -1 );
					return false;
				}
			}

			float movePercent = (float)this.TicksElapsed / (float)this.ToTickDuration;
			if( this.TicksElapsed > this.ToTickDuration ) {
				int fro = this.ToTickDuration + this.LingerTicksDuration;

				if( this.TicksElapsed < fro ) {
					movePercent = 1f;
					this.OnTraversed?.Invoke();
				} else {
					movePercent = (this.TicksElapsed - fro) / this.FroTickDuration;
					movePercent = movePercent < 0f ? 0f : 1f - movePercent;
				}
			}

			this.RunAnimation( movePercent );

			return true;
		}

		////

		/// <summary></summary>
		/// <param name="percent"></param>
		protected abstract void RunAnimation( float percent );
	}
}
