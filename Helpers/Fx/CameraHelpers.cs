using HamstarHelpers.Classes.Loadable;
using System;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Fx {
	/// <summary>
	/// Assorted static "helper" functions pertaining to basic camera movement.
	/// </summary>
	public partial class CameraHelpers : ILoadable {
		/// <summary>
		/// Shifts the camers from its current position by the given offset.
		/// </summary>
		/// <param name="offsetX"></param>
		/// <param name="offsetY"></param>
		public static void ApplyOffset( int offsetX, int offsetY ) {
			var cam = ModContent.GetInstance<CameraHelpers>();

			cam.OffsetX = offsetX;
			cam.OffsetY = offsetY;
		}

		/// <summary>
		/// Gets any camera shift offset amount.
		/// </summary>
		/// <returns></returns>
		public static (int X, int Y) GetOffset() {
			var cam = ModContent.GetInstance<CameraHelpers>();

			return (cam.OffsetX, cam.OffsetY);
		}

		/// <summary>
		/// Applies a shaking motion to the camera.
		/// </summary>
		/// <param name="magnitude"></param>
		/// <param name="tickDuration"></param>
		public static void ApplyShake( float magnitude, int tickDuration ) {
			var cam = ModContent.GetInstance<CameraHelpers>();

			cam.ShakeMagnitude = magnitude;
			cam.ShakeTickDuration = tickDuration;
			cam.ShakeTicksElapsed = 0;
		}

		/// <summary>
		/// Gets the duration of any current shaking.
		/// </summary>
		/// <returns></returns>
		public static int GetShakeDuration() {
			var cam = ModContent.GetInstance<CameraHelpers>();

			return cam.ShakeTicksElapsed;
		}


		/// <summary>
		/// Positions the camera statically somewhere in the world.
		/// </summary>
		/// <param name="worldLeft"></param>
		/// <param name="worldTop"></param>
		public static void MoveTo( int worldLeft, int worldTop ) {
			var cam = ModContent.GetInstance<CameraHelpers>();

			cam.WorldLeft = worldLeft;
			cam.WorldTop = worldTop;
		}
	}
}
