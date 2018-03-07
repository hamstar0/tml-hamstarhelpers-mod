using Terraria;


namespace HamstarHelpers.TmlHelpers {
	[System.Obsolete( "use TmlLoadHelpers", true )]
	public class TmlWorldHelpers {
		public static bool IsWorldLoaded() {
			return TmlLoadHelpers.IsWorldLoaded();
		}


		public static bool IsGameLoaded() {
			return TmlLoadHelpers.IsWorldBeingPlayed();
		}
	}
}
