using System;


namespace HamstarHelpers.Libraries.Audio {
	/// <summary>
	/// Assorted static "helper" functions pertaining to game music.
	/// </summary>
	public partial class MusicLibraries {
		/// <summary>
		/// Adjusts the volume scale for the currently playing music.
		/// </summary>
		/// <param name="scale"></param>
		public static void SetVolumeScale( float scale ) {
			ModHelpersMod.Instance.MusicHelpers.Scale = scale;
		}
	}
}
