using HamstarHelpers.Services.Timers;
using System;
using Terraria;
using Terraria.ModLoader.Audio;


namespace HamstarHelpers.Helpers.Misc {
	public partial class MusicHelpers {
		public static void SetVolumeScale( float scale ) {
			ModHelpersMod.Instance.MusicHelpers.Scale = scale;
		}
	}
}
