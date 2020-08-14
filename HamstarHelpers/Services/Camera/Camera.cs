using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating the player's 'camera' (screen position).
	/// </summary>
	public partial class Camera {
		/// <summary>
		/// Shifts the camera from its current position by the given offset.
		/// </summary>
		/// <param name="offsetX"></param>
		/// <param name="offsetY"></param>
		public static void ApplyOffset( int offsetX, int offsetY ) {
			var inst = Camera.Instance;
			inst.OffsetX = offsetX;
			inst.OffsetY = offsetY;
		}


		/// <summary>
		/// Positions the camera statically somewhere in the world. Enter -1 for an axis to use the default.
		/// </summary>
		/// <param name="position"></param>
		public static void ApplyPosition( Vector2 position ) {
			var inst = Camera.Instance;
			inst.WorldPosition = position;
		}


		/// <summary>
		/// Applies a shaking motion to the camera.
		/// </summary>
		/// <param name="magnitude"></param>
		public static void ApplyShake( float magnitude ) {
			var inst = Camera.Instance;
			inst.ShakeMagnitude = magnitude;
		}


		/// <summary>
		/// Applies zoom to the camera.
		/// </summary>
		/// <param name="scale"></param>
		public static void ApplyZoom( float scale ) {
			var inst = Camera.Instance;

			if( inst.OldZoomScale == -1f ) {
				inst.OldZoomScale = Main.GameZoomTarget;
			}
			inst.ZoomScale = scale;
		}


		////////////////

		/// <summary>
		/// Resets camera shift.
		/// </summary>
		public static void ResetOffset() {
			var inst = Camera.Instance;
			inst.OffsetX = 0;
			inst.OffsetY = 0;
		}


		/// <summary>
		/// Resets camera position.
		/// </summary>
		public static void ResetPosition() {
			var inst = Camera.Instance;
			inst.WorldPosition = new Vector2( -1 );
		}


		/// <summary>
		/// Reset shaking motion to the camera.
		/// </summary>
		public static void ResetShake() {
			var inst = Camera.Instance;
			inst.ShakeMagnitude = 0f;
		}


		/// <summary>
		/// Resets zoom of the camera.
		/// </summary>
		public static void ResetZoom() {
			var inst = Camera.Instance;

			inst.ZoomScale = inst.OldZoomScale;
			inst.OldZoomScale = -1;
		}
	}
}
