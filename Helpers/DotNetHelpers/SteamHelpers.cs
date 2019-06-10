using System;


namespace HamstarHelpers.Helpers.DotNET {
	public static class SteamHelpers {
		public static string GetSteamID() {
			return Steamworks.SteamUser.GetSteamID().ToString();
		}
	}
}
