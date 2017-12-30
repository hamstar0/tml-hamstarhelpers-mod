using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.BuffHelpers {
	public static class BuffHelpers {
		public readonly static IDictionary<string, int> BuffIdsByName = new Dictionary<string, int>();


		internal static void Initialize() {
			for( int i=0; i<Main.buffTexture.Length; i++ ) {
				BuffHelpers.BuffIdsByName[ Lang.GetBuffName(i) ] = i;
			}
		}

		////////////////

		public static void AddPermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			modplayer.PermaBuffsById.Add( buff_id );
		}

		public static void RemovePermaBuff( Player player, int buff_id ) {
			var modplayer = player.GetModPlayer<HamstarHelpersPlayer>();
			modplayer.PermaBuffsById.Remove( buff_id );
		}
	}
}
