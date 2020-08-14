using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Services.Camera;


namespace HamstarHelpers.Classes.CameraAnimation {
	/// <summary>
	/// Represents a sequence of controlled movement for the player's 'camera'.
	/// </summary>
	public class CameraMover : CameraAnimator {
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
		public int MoveXFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYFrom { get; private set; } = -1;

		/// <summary></summary>
		public int MoveXTo { get; private set; } = -1;

		/// <summary></summary>
		public int MoveYTo { get; private set; } = -1;



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="moveXFrom"></param>
		/// <param name="moveYFrom"></param>
		/// <param name="moveXTo"></param>
		/// <param name="moveYTo"></param>
		/// <param name="toDuration">How long (in ticks) the camera takes to travel from A to B.</param>
		/// <param name="lingerDuration">How long (in ticks) to linger at destination (B).</param>
		/// <param name="froDuration">How long (in ticks) the camera takes to travel back from B to A.</param>
		/// <param name="onTraversed">Function to call on reaching destination (B).</param>
		/// <param name="onStop">Function to call on stop (not pause); either by completion or manual stop.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public CameraMover(
					string name,
					int moveXFrom,
					int moveYFrom,
					int moveXTo,
					int moveYTo,
					int toDuration,
					int lingerDuration,
					int froDuration,
					Action onTraversed = null,
					Action onStop = null,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, onTraversed, onStop, skippedTicks ) {
			this.MoveXFrom = moveXFrom;
			this.MoveYFrom = moveYFrom;
			this.MoveXTo = moveXTo;
			this.MoveYTo = moveYTo;
		}


		////////////////

		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void ApplyAnimation( float percent ) {
			Vector2 position = CameraMover.GetMovePosition(
				this.MoveXFrom,
				this.MoveYFrom,
				this.MoveXTo,
				this.MoveYTo,
				Math.Min( percent, 1f )
			);

			Camera.ApplyPosition( position );
		}

		/// <summary></summary>
		protected override void EndAnimation() {
			Camera.ResetPosition();
		}
	}
}
