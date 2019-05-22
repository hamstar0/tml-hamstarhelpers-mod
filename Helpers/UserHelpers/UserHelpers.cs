using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UserHelpers {
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
