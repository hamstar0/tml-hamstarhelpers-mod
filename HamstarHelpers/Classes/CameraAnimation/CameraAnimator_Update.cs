using System;
using Terraria;


namespace HamstarHelpers.Classes.CameraAnimation {
	/// <summary>
	/// Represents a sequence of controlled behavior for the player's 'camera'.
	/// </summary>
	public abstract partial class CameraAnimator {
		internal bool Update() {
			if( !this.IsPaused ) {
				this.TicksElapsed++;

				int total = this.TotalTickDuration;

				if( this.TicksElapsed == total ) {
					this.TicksElapsed++;
					this.EndAnimation_Private();
				}
				if( this.TicksElapsed >= total ) {
					return false;
				}
			}

			float movePercent;
			if( this.ToTickDuration > 0 ) {
				movePercent = (float)this.TicksElapsed / (float)this.ToTickDuration;
			} else {
				movePercent = 1f;
			}

			if( this.TicksElapsed > this.ToTickDuration ) {
				int froStart = this.ToTickDuration + this.LingerTicksDuration;

				if( this.TicksElapsed < froStart ) {
					movePercent = 1f;

					this.OnTraversed?.Invoke();
				} else {
					if( this.FroTickDuration > 0 ) {
						movePercent = (this.TicksElapsed - froStart) / this.FroTickDuration;
						movePercent = movePercent < 0f ? 0f : 1f - movePercent;
					} else {
						movePercent = 0f;
					}
				}
			}

			this.ApplyAnimation( movePercent );

			return true;
		}
	}
}
