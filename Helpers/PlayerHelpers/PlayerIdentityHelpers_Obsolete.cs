using System;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public partial class PlayerIdentityHelpers {
		[Obsolete("use GetProperUniqueId(Player)", true)]
		public static string GetUniqueId( Player player, out bool success ) {
			return PlayerIdentityHelpers._GetUniqueId( player, out success );
		}

		internal static string _GetUniqueId( Player player, out bool success ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();

			success = myplayer.Logic.HasLoadedOldUID;
			return myplayer.Logic.OldPrivateUID;
		}


		[Obsolete( "use GetProperUniqueId(Player)", true )]
		public static Player GetPlayerById( string uid, out bool isNothingOverlooked ) {
			return PlayerIdentityHelpers._GetPlayerById( uid, out isNothingOverlooked );
		}

		internal static Player _GetPlayerById( string uid, out bool isNothingOverlooked ) {
			Player plr = null;
			int len = Main.player.Length;
			isNothingOverlooked = true;

			for( int i=0; i<len; i++ ) {
				plr = Main.player[ i ];
				if( plr == null || !plr.active ) { continue; }

				bool mysuccess;
				string myuid = PlayerIdentityHelpers._GetUniqueId( plr, out mysuccess );
				if( !mysuccess ) {
					isNothingOverlooked = false;
					continue;
				}

				if( myuid == uid ) {
					isNothingOverlooked = true;
					break;
				}
			}

			return plr;
		}
	}
}
