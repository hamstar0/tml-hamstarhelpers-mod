using System;


namespace HamstarHelpers.TmlHelpers {
	public class TmlWorldHelpers {
		public static bool IsWorldLoaded() {
			var mymod = HamstarHelpersMod.Instance;
			var myworld = mymod.GetModWorld<HamstarHelpersWorld>();
			return myworld.WorldLogic.IsLoaded( mymod );
		}
	}
}
