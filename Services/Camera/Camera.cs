using System;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating the player's 'camera' (screen position).
	/// </summary>
	public partial class Camera {
		/// <summary>
		/// Shifts the camers from its current position by the given offset.
		/// </summary>
		/// <param name="offsetX"></param>
		/// <param name="offsetY"></param>
		public static void ApplyOffset( int offsetX, int offsetY ) {
			Camera.OffsetX = offsetX;
			Camera.OffsetY = offsetY;
		}

		/// <summary>
		/// Gets any camera shift offset amount.
		/// </summary>
		/// <returns></returns>
		public static (int X, int Y) GetOffset() {
			return (Camera.OffsetX, Camera.OffsetY);
		}

		/// <summary>
		/// Applies a shaking motion to the camera.
		/// </summary>
		/// <param name="magnitude"></param>
		/// <param name="tickDuration"></param>
		public static void ApplyShake( float magnitude, int tickDuration, int skippedTicks=0 ) {
			Camera.ShakeMagnitude = magnitude;
			Camera.ShakeTickDuration = tickDuration;
			Camera.ShakeTicksElapsed = skippedTicks;
		}

		/// <summary>
		/// Gets the duration of any current shaking.
		/// </summary>
		/// <returns></returns>
		public static int GetShakeDuration() {
			return Camera.ShakeTicksElapsed;
		}


		/// <summary>
		/// Positions the camera statically somewhere in the world.
		/// </summary>
		/// <param name="worldLeft"></param>
		/// <param name="worldTop"></param>
		public static void MoveTo( int worldLeft, int worldTop ) {
			Camera.WorldLeft = worldLeft;
			Camera.WorldTop = worldTop;
		}
	}
}
