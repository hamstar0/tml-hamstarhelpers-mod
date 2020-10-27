using System;


namespace HamstarHelpers.Helpers.Misc {
	/// @private
	[Obsolete( "use Audio.MusicHelpers", true )]
	public partial class MusicHelpers {
		/// @private
		[Obsolete( "use Audio.MusicHelpers", true )]
		public static void SetVolumeScale( float scale ) {
			Audio.MusicHelpers.SetVolumeScale( scale );
		}
	}
}
