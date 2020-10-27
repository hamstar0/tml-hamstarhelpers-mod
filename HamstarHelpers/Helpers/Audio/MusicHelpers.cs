using System;


namespace HamstarHelpers.Helpers.Audio {
	/// <summary>
	/// Assorted static "helper" functions pertaining to game music.
	/// </summary>
	public partial class MusicHelpers {
		/// <summary>
		/// Adjusts the volume scale for the currently playing music.
		/// </summary>
		/// <param name="scale"></param>
		public static void SetVolumeScale( float scale ) {
			ModHelpersMod.Instance.MusicHelpers.Scale = scale;
		}
	}
}
