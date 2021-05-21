using System;
using Terraria;
using HamstarHelpers.Classes.Errors;


namespace HamstarHelpers.Helpers.Misc {
	/// @private
	[Obsolete( "use Audio.SoundHelpers", true )]
	public class SoundHelpers {
		/// @private
		[Obsolete("use Audio.SoundHelpers", true)]
		public static (float Volume, float Pan) GetSoundDataFromSource( int worldX, int worldY ) {
			return Helpers.Audio.SoundHelpers.GetSoundDataFromSource( worldX, worldY );
		}
	}
}
