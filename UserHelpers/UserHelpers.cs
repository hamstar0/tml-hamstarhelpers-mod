using Terraria;


namespace HamstarHelpers.UserHelpers {
	public static class UserHelpers {
		public static bool IsAdmin( Player player ) {
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			return modworld.Logic.IsAdmin( player );
		}

		
		internal static void AddAdmin( Player player ) {    // Unfortunately, this isn't an API method (yet?)
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			modworld.Logic.SetAsAdmin( player );
		}

		internal static bool RemoveAdmin( Player player ) {    // Unfortunately, this isn't an API method (yet?)
			var modworld = HamstarHelpersMod.Instance.GetModWorld<HamstarHelpersWorld>();
			return modworld.Logic.RemoveAsAdmin( player );
		}
	}
}
