using System;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Classes.Loadable;


namespace HamstarHelpers.Services.Camera {
	/// <summary>
	/// Supplies a set of controls for manipulating the player's 'camera' (screen position).
	/// </summary>
	public partial class Camera : ILoadable {
		/// @private
		[Obsolete( "use ApplyPosition", true )]
		public static void MoveTo( int worldLeft, int worldTop ) {
			Camera.ApplyPosition( worldLeft, worldTop );
		}

		/// @private
		[Obsolete( "use AnimatedCamera.BeginShakeSequence", true )]
		public static void ApplyShake( float magnitude, int tickDuration, int skippedTicks = 0 ) {
			CameraShaker.Current = new CameraShaker(
				name: "ObsoleteDefault",
				peakMagnitude: magnitude,
				toDuration: tickDuration,
				lingerDuration: 0,
				froDuration: 0,
				isSmoothed: false,
				onRun: null,
				onStop: null,
				skippedTicks: skippedTicks
			);
		}
	}
}
