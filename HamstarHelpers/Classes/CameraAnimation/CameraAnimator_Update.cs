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

			float progPercent;
			if( this.ToTickDuration > 0 ) {
				progPercent = (float)this.TicksElapsed / (float)this.ToTickDuration;
			} else {
				progPercent = 1f;
			}

			if( this.TicksElapsed > this.ToTickDuration ) {
				int froStart = this.ToTickDuration + this.LingerTicksDuration;

				if( this.TicksElapsed < froStart ) {
					progPercent = 1f;
				} else {
					if( this.FroTickDuration > 0 ) {
						progPercent = (this.TicksElapsed - froStart) / this.FroTickDuration;
						progPercent = progPercent < 0f ? 0f : 1f - progPercent;
					} else {
						progPercent = 0f;
					}
				}
			}

			this.OnRun?.Invoke();

			this.ApplyAnimation( progPercent );

			return true;
		}
	}
}
