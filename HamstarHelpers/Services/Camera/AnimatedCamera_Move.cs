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
		/// Applies motion to the camera.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="moveXFrom"></param>
		/// <param name="moveYFrom"></param>
		/// <param name="moveXTo"></param>
		/// <param name="moveYTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginMoveSequence(
					string name,
					int moveXFrom,
					int moveYFrom,
					int moveXTo,
					int moveYTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			var anim = AnimatedCamera.Instance;
			anim.CurrentMoveSequence = name;
			anim.MoveXFrom = moveXFrom;
			anim.MoveYFrom = moveYFrom;
			anim.MoveXTo = moveXTo;
			anim.MoveYTo = moveYTo;
			anim.MoveTickDuration = tickDuration;
			anim.MoveTicksElapsed = skippedTicks;
			anim.MoveTicksLingerDuration = lingerDuration;
		}

		/// <summary>
		/// Applies motion to the camera.
		/// </summary>
		/// <param name="moveXFrom"></param>
		/// <param name="moveYFrom"></param>
		/// <param name="moveXTo"></param>
		/// <param name="moveYTo"></param>
		/// <param name="tickDuration">How long the sequence takes to complete.</param>
		/// <param name="lingerDuration">How long to linger at destination before reset. Set to -1 for permanent.</param>
		/// <param name="skippedTicks">How far into the sequence to skip to (in ticks).</param>
		public static void BeginMoveSequence(
					int moveXFrom,
					int moveYFrom,
					int moveXTo,
					int moveYTo,
					int tickDuration,
					int lingerDuration,
					int skippedTicks = 0 ) {
			AnimatedCamera.BeginMoveSequence(
				"Default",
				moveXFrom,
				moveYFrom,
				moveXTo,
				moveYTo,
				tickDuration,
				lingerDuration,
				skippedTicks
			);
		}


		////

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
				fromX = Main.LocalPlayer.Center.X - (Main.GameZoomTarget * ((float)Main.screenWidth * 0.5f));
			}
			if( toX < 0 ) {
				toX = Main.LocalPlayer.Center.X - (Main.GameZoomTarget * ((float)Main.screenWidth * 0.5f));
			}
			if( fromY < 0 ) {
				fromY = Main.LocalPlayer.Center.Y - (Main.GameZoomTarget * ((float)Main.screenHeight * 0.5f));
			}
			if( toY < 0 ) {
				toY = Main.LocalPlayer.Center.Y - (Main.GameZoomTarget * ((float)Main.screenHeight * 0.5f));
			}

			return new Vector2(
				fromX + ((toX - fromX) * percent),
				fromY + ((toY - fromY) * percent)
			);
		}



		////////////////

		private void AnimateMove() {
			if( this.MoveTicksElapsed++ >= (this.MoveTickDuration + this.MoveTicksLingerDuration) ) {
				this.MoveTicksElapsed = 0;
				this.MoveTickDuration = 0;
				this.MoveTicksLingerDuration = 0;

				Camera.ApplyPosition( new Vector2(-1f) );
				return;
			}

			float movePercent = (float)this.MoveTicksElapsed / (float)this.MoveTickDuration;
			Vector2 position = AnimatedCamera.GetMovePosition(
				this.MoveXFrom,
				this.MoveYFrom,
				this.MoveXTo,
				this.MoveYTo,
				Math.Min( movePercent, 1f )
			);

			Camera.ApplyPosition( position );
		}
	}
}
