﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Players;


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
			if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
				throw new ModHelpersException( "Not multiplayer." );
			}

			if( string.IsNullOrEmpty( ModHelpersPrivilegedUserConfig.Instance.PrivilegedUserId) ) {
				return false;
			}

			return ModHelpersPrivilegedUserConfig.Instance.PrivilegedUserId == PlayerIdentityHelpers.GetUniqueId( player );
		}
	}
}
