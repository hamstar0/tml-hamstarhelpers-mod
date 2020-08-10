using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Represents a sequence of controlled movement for the player's 'camera' (screen position).
	/// </summary>
	public class CameraMover {
		/// <summary></summary>
		public static CameraMover Current {
			get => CameraAnimationManager.Instance.CurrentMover;
			set => CameraAnimationManager.Instance.CurrentMover = value;
		}



		////////////////

		/// <summary>
		/// Gets an interpolated position between 2 points, accounting for points that refer to screen position (-1 values).
		/// </summary>
		/// <param name="fromX"></param>
		/// <param name="fromY"></param>
		/// <param name="toX"></param>
		/// <param name="toY"></param>
		/// <param name="percent"></param>
		/// <returns></returns>
		public static Vector2 GetMovePosition( float fromX, float fromY, float toX, float toY, float percent ) {
			if( fromX < 0 ) {
				fromX = Main.LocalPlayer.Center.X - ( Main.GameZoomTarget * ( (float)Main.screenWidth * 0.5f ) );
			}
			if( toX < 0 ) {
				toX = Main.LocalPlayer.Center.X - ( Main.GameZoomTarget * ( (float)Main.screenWidth * 0.5f ) );
			}
			if( fromY < 0 ) {
				fromY = Main.LocalPlayer.Center.Y - ( Main.GameZoomTarget * ( (float)Main.screenHeight * 0.5f ) );
			}
			if( toY < 0 ) {
				toY = Main.LocalPlayer.Center.Y - ( Main.GameZoomTarget * ( (float)Main.screenHeight * 0.5f ) );
			}

			return new Vector2(
				fromX + ( ( toX - fromX ) * percent ),
				fromY + ( ( toY - fromY ) * percent )
			);
		}



		////////////////

		/// <summary></summary>
		public string Name { get; private set; }

		////

		/// <summary></summary>
		public int MoveXFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveXTo { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYTo { get; private set; } = -1;

		////

		/// <summary></summary>
		public int TickDuration { get; private set; } = 0;

		/// <summary></summary>
		public int TicksElapsed { get; private set; } = 0;

		/// <summary></summary>
		public int TicksLingerDuration { get; private set; } = 0;

		/// <summary>Note: Negative values indicate moving still in progress.</summary>
		public int TicksLingerElapsed => this.TicksElapsed - this.TickDuration;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="moveXFrom"></param>
		/// <param name="moveYFrom"></param>
		/// <param name="moveXTo"></param>
		/// <param name="moveYTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraMover(
					string name,
					int moveXFrom,
					int moveYFrom,
					int moveXTo,
					int moveYTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			this.Name = name;
			this.MoveXFrom = moveXFrom;
			this.MoveYFrom = moveYFrom;
			this.MoveXTo = moveXTo;
			this.MoveYTo = moveYTo;
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
				Camera.ApplyPosition( new Vector2( -1f ) );
				return false;
			}

			float movePercent = (float)this.TicksElapsed / (float)this.TickDuration;
			Vector2 position = CameraMover.GetMovePosition(
				this.MoveXFrom,
				this.MoveYFrom,
				this.MoveXTo,
				this.MoveYTo,
				Math.Min( movePercent, 1f )
			);

			Camera.ApplyPosition( position );
			return true;
		}
	}
}
