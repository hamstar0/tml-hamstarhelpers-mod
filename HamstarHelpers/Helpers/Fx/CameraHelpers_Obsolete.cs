using System;
using HamstarHelpers.Services.Camera;


namespace HamstarHelpers.Helpers.Fx {
	/// @private
	[Obsolete("use `Camera` service", true)]
	public partial class CameraHelpers {
		/// @private
		[Obsolete( "use `Camera.ApplyOffset`", true )]
		public static void ApplyOffset( int offsetX, int offsetY ) {
			Camera.ApplyOffset( offsetX, offsetY );
		}

		/// @private
		[Obsolete( "use `Camera.GetOffset`", true )]
		public static (int X, int Y) GetOffset() {
			var cam = Camera.Instance;
			return (cam.OffsetX, cam.OffsetY);
		}

		/// @private
		[Obsolete( "use `Camera.ApplyShake`", true )]
		public static void ApplyShake( float magnitude, int tickDuration ) {
			Camera.ApplyShake( magnitude, tickDuration );
		}

		/// @private
		[Obsolete( "use `AnimatedCamera.ShakeTickDuration`", true )]
		public static int GetShakeDuration() {
			return CameraShaker.Current?.ToTickDuration ?? 0;
		}


		/// @private
		[Obsolete( "use `Camera.MoveTo`", true )]
		public static void MoveTo( int worldLeft, int worldTop ) {
			Camera.MoveTo( worldLeft, worldTop );
		}
	}
}
