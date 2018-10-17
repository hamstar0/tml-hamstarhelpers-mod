using System;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static partial class PlayerIdentityHelpers {
		[Obsolete("use GetProperUniqueId(Player)", true)]
		public static string GetUniqueId( Player player, out bool success ) {
			return PlayerIdentityHelpers._GetUniqueId( player, out success );
		}

		internal static string _GetUniqueId( Player player, out bool success ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			success = myplayer.Logic.HasLoadedUID;
			return myplayer.Logic.PrivateUID;
		}


		[Obsolete( "use GetProperUniqueId(Player)", true )]
		public static Player GetPlayerById( string uid, out bool is_nothing_overlooked ) {
			return PlayerIdentityHelpers._GetPlayerById( uid, out is_nothing_overlooked );
		}

		internal static Player _GetPlayerById( string uid, out bool is_nothing_overlooked ) {
			Player plr = null;
			int len = Main.player.Length;
			is_nothing_overlooked = true;

			for( int i=0; i<len; i++ ) {
				plr = Main.player[ i ];
				if( plr == null || !plr.active ) { continue; }

				bool mysuccess;
				string myuid = PlayerIdentityHelpers._GetUniqueId( plr, out mysuccess );
				if( !mysuccess ) {
					is_nothing_overlooked = false;
					continue;
				}

				if( myuid == uid ) {
					is_nothing_overlooked = true;
					break;
				}
			}

			return plr;
		}
	}
}
