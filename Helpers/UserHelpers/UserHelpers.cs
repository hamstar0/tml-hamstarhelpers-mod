using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UserHelpers {
	public static class UserHelpers {
		[Obsolete("use HasBasicServerPrivilege(Player)", true)]
		public static bool HasBasicServerPrivilege( Player player, out bool success ) {
			success = true;
			return UserHelpers.HasBasicServerPrivilege( player );
		}

		public static bool HasBasicServerPrivilege( Player player ) {
			if( Main.netMode == 0 && !Main.dedServ ) {
				throw new Exception( "Not multiplayer." );
			}
			
			if( string.IsNullOrEmpty( ModHelpersMod.Instance.Config.PrivilegedUserId ) ) {
				return false;
			}

			return ModHelpersMod.Instance.Config.PrivilegedUserId == PlayerIdentityHelpers.GetProperUniqueId( player );
		}
	}
}
