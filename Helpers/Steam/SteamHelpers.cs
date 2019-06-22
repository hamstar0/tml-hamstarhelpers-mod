using System;


namespace HamstarHelpers.Helpers.Steam {
	/** <summary>Assorted static "helper" functions pertaining to the Steam platform.</summary> */
	public static class SteamHelpers {
		public static string GetSteamID() {
			return Steamworks.SteamUser.GetSteamID().ToString();
		}
	}
}
