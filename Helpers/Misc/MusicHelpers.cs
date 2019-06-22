using System;
using Terraria;


namespace HamstarHelpers.Helpers.Misc {
	/** <summary>Assorted static "helper" functions pertaining to game music.</summary> */
	public partial class MusicHelpers {
		public static void SetVolumeScale( float scale ) {
			ModHelpersMod.Instance.MusicHelpers.Scale = scale;
		}
	}
}
