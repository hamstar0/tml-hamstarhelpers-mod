using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.UserHelpers {
	public class UserHelpers {
		public static bool HasBasicServerPrivilege( Player player, out bool success ) {
			if( Main.netMode == 0 && !Main.dedServ ) {
				throw new Exception( "Not multiplayer." );
			}

			success = false;

			if( string.IsNullOrEmpty( HamstarHelpersMod.Instance.Config.PrivilegedUserId ) ) {
				return false;
			}

			return HamstarHelpersMod.Instance.Config.PrivilegedUserId == PlayerIdentityHelpers.GetUniqueId( player, out success );
		}
	}
}
