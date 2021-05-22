using System;


namespace HamstarHelpers.Libraries.Steam {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the Steam platform.
	/// </summary>
	public class SteamLibraries {
		/// <summary>
		/// Attempts to get the current Steam user's Steam ID.
		/// </summary>
		/// <returns></returns>
		public static string GetSteamID() {
			return Steamworks.SteamUser.GetSteamID().ToString();
		}
	}
}
