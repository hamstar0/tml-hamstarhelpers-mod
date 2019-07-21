using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Players;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.User {
	/// <summary>
	/// Assorted static "helper" functions pertaining to the concept of "users" (corrently supports only a single,
	/// config-defined "priviledged" user).
	/// </summary>
	public class UserHelpers {
		/// <summary>
		/// Indicates if the given player has special priviledge on a server. Currently, this is only defined by a config
		/// setting (`PrivilegedUserId`) using the user's internal unique ID (see `PlayerIdentityHelpers.GetUniqueId()`).
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool HasBasicServerPrivilege( Player player ) {
			if( Main.netMode == 0 && !Main.dedServ ) {
				throw new HamstarException( "Not multiplayer." );
			}

			var mymod = ModHelpersMod.Instance;

			if( string.IsNullOrEmpty(mymod.Config.PrivilegedUserId) ) {
				return false;
			}

			return mymod.Config.PrivilegedUserId == PlayerIdentityHelpers.GetUniqueId( player );
		}
	}
}
