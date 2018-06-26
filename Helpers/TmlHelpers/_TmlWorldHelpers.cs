using Terraria;


namespace HamstarHelpers.TmlHelpers {
	[System.Obsolete( "use LoadHelpers", true )]
	public class TmlWorldHelpers {
		public static bool IsWorldLoaded() {
			return LoadHelpers.IsWorldLoaded();
		}


		public static bool IsGameLoaded() {
			return LoadHelpers.IsWorldBeingPlayed();
		}
	}
}
