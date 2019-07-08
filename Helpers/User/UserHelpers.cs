using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Players;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.User {
	/** <summary>Assorted static "helper" functions pertaining to the concept of "users" (corrently supports only a single, config-defined "priviledged" user).</summary> */
	public static class UserHelpers {
		public static bool HasBasicServerPrivilege( Player player ) {
			if( Main.netMode == 0 && !Main.dedServ ) {
				throw new HamstarException( "Not multiplayer." );
			}
			
			if( string.IsNullOrEmpty( ModHelpersMod.Instance.Config.PrivilegedUserId ) ) {
				return false;
			}

			return ModHelpersMod.Instance.Config.PrivilegedUserId == PlayerIdentityHelpers.GetUniqueId();
		}
	}
}
