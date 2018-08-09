using System;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public static class SteamHelpers {
		public static string GetSteamID() {
			return Steamworks.SteamUser.GetSteamID().ToString();
		}
	}
}
