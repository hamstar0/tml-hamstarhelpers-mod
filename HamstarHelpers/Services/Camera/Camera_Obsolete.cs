using System;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating the player's 'camera' (screen position).
	/// </summary>
	public partial class Camera {
		/// @private
		[Obsolete( "use ApplyPosition", true )]
		public static void MoveTo( int worldLeft, int worldTop ) {
			Camera.ApplyPosition( new Microsoft.Xna.Framework.Vector2(worldLeft, worldTop) );
		}

		/// @private
		[Obsolete( "use AnimatedCamera.BeginShakeSequence", true )]
		public static void ApplyShake( float magnitude, int tickDuration, int skippedTicks = 0 ) {
			CameraShaker.Current = new CameraShaker( "ObsoleteDefault", magnitude, tickDuration, 0, 0, null, skippedTicks );
		}
	}
}
