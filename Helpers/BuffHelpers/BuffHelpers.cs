using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.BuffHelpers {
	public class BuffHelpers {
		[System.Obsolete( "use BuffIdentityHelpers.NamesToIds", true )]
		public static IReadOnlyDictionary<string, int> BuffIdsByName { get {
			return BuffIdentityHelpers.NamesToIds;
		} }


		////////////////

		public static void AddPermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			modplayer.Logic.PermaBuffsById.Add( buff_id );
		}

		public static void RemovePermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			if( modplayer.Logic.PermaBuffsById.Contains(buff_id) ) {
				modplayer.Logic.PermaBuffsById.Remove( buff_id );
			}
		}
	}
}
