using System;
using Terraria;
using Terraria.UI;
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
		/// Gets an interpolated position between 2 points.
		/// </summary>
		/// <param name="fromX">-1 substitutes with current screen X coord.</param>
		/// <param name="fromY">-1 substitutes with current screen Y coord.</param>
		/// <param name="toX">-1 substitutes with current screen X coord.</param>
		/// <param name="toY">-1 substitutes with current screen Y coord.</param>
		/// <param name="percent"></param>
		/// <param name="useGameZoom"></param>
		/// <param name="leftOffset"></param>
		/// <param name="topOffset"></param>
		/// <returns></returns>
		public static (int worldX, int worldY) GetMovePosition(
					float fromX,
					float fromY,
					float toX,
					float toY,
					float percent,
					bool useGameZoom,
					StyleDimension leftOffset,
					StyleDimension topOffset ) {
			float zoom = useGameZoom ? Main.GameZoomTarget : 1f;

			if( fromX < 0f ) {
				fromX = Main.LocalPlayer.Center.X - (zoom * ((float)Main.screenWidth * 0.5f));
			}
			if( toX < 0f ) {
				toX = Main.LocalPlayer.Center.X - (zoom * ((float)Main.screenWidth * 0.5f));
			}
			if( fromY < 0f ) {
				fromY = Main.LocalPlayer.Center.Y - (zoom * ((float)Main.screenHeight * 0.5f));
			}
			if( toY < 0f ) {
				toY = Main.LocalPlayer.Center.Y - (zoom * ((float)Main.screenHeight * 0.5f));
			}

			float worldX = fromX + ((toX - fromX) * percent);
			float worldY = fromY + ((toY - fromY) * percent);
			worldX -= leftOffset.Pixels;
			worldX -= (zoom * (float)Main.screenWidth * leftOffset.Percent);
			worldY -= topOffset.Pixels;
			worldY -= (zoom * (float)Main.screenHeight * topOffset.Percent);

			worldX = Math.Max( worldX, 0f );
			worldY = Math.Max( worldY, 0f );

			return ((int)worldX, (int)worldY);
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

		/// <summary></summary>
		public bool UseGameZoom { get; private set; } = true;

		/// <summary>Pixel or percent offset of the screen position from its left starting point.</summary>
		public StyleDimension LeftOffset { get; private set; } = new StyleDimension( 0f, 0f );

		/// <summary>Pixel or percent offset of the screen position from its top starting point.</summary>
		public StyleDimension TopOffset { get; private set; } = new StyleDimension( 0f, 0f );



		////////////////

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="moveXFrom">World coordinates. -1 substitutes with current screen X coord.</param>
		/// <param name="moveYFrom">World coordinates. -1 substitutes with current screen Y coord.</param>
		/// <param name="moveXTo">World coordinates. -1 substitutes with current screen X coord.</param>
		/// <param name="moveYTo">World coordinates. -1 substitutes with current screen Y coord.</param>
		/// <param name="toDuration">How long (in ticks) the camera takes to travel from A to B.</param>
		/// <param name="lingerDuration">How long (in ticks) to linger at destination (B).</param>
		/// <param name="froDuration">How long (in ticks) the camera takes to travel back from B to A.</param>
		/// <param name="useGameZoomForOffset"></param>
		/// <param name="leftOffset">Pixel or percent offset of the screen position from its left starting point. Defaults to 50% (0.5).</param>
		/// <param name="topOffset">Pixel or percent offset of the screen position from its top starting point. Defaults to 50% (0.5).</param>
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
					bool useGameZoomForOffset = true,
					StyleDimension? leftOffset = null,
					StyleDimension? topOffset = null,
					Action onTraversed = null,
					Action onStop = null,
					int skippedTicks = 0 )
					: base( name, toDuration, lingerDuration, froDuration, onTraversed, onStop, skippedTicks ) {
			this.MoveXFrom = moveXFrom;
			this.MoveYFrom = moveYFrom;
			this.MoveXTo = moveXTo;
			this.MoveYTo = moveYTo;
			this.UseGameZoom = useGameZoomForOffset;
			this.LeftOffset = leftOffset ?? new StyleDimension( 0f, 0.5f );
			this.TopOffset = topOffset ?? new StyleDimension( 0f, 0.5f );
		}


		////////////////

		/// <summary></summary>
		/// <param name="percent"></param>
		protected override void ApplyAnimation( float percent ) {
			(int x, int y) worldPos = CameraMover.GetMovePosition(
				fromX: this.MoveXFrom,
				fromY: this.MoveYFrom,
				toX: this.MoveXTo,
				toY: this.MoveYTo,
				percent: Math.Min( percent, 1f ),
				useGameZoom: this.UseGameZoom,
				leftOffset: this.LeftOffset,
				topOffset: this.TopOffset
			);

			Camera.ApplyPosition( worldPos.x, worldPos.y );
		}

		/// <summary></summary>
		protected override void EndAnimation() {
			Camera.ResetPosition();
		}
	}
}
