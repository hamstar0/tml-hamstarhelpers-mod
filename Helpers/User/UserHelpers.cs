using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.Players;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.User {
	public static class UserHelpers {
		public static bool HasBasicServerPrivilege( Player player ) {
			if( Main.netMode == 0 && !Main.dedServ ) {
				throw new HamstarException( "Not multiplayer." );
			}
			
			if( string.IsNullOrEmpty( ModHelpersMod.Instance.Config.PrivilegedUserId ) ) {
				return false;
			}

			return ModHelpersMod.Instance.Config.PrivilegedUserId == PlayerIdentityHelpers.GetMyProperUniqueId();
		}
	}
}
