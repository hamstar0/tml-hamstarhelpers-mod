using Terraria;


namespace HamstarHelpers.Helpers.Buffs {
	/** <summary>Assorted static "helper" functions pertaining to buffs.</summary> */
	public partial class BuffHelpers {
		public static void AddPermaBuff( Player player, int buffId ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.AddPermaBuff( buffId );
		}

		public static void RemovePermaBuff( Player player, int buffId ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.RemovePermaBuff( buffId );
		}
	}
}
