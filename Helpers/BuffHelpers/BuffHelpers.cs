using Terraria;


namespace HamstarHelpers.Helpers.BuffHelpers {
	public partial class BuffHelpers {
		public static void AddPermaBuff( Player player, int buff_id ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.AddPermaBuff( buff_id );
		}

		public static void RemovePermaBuff( Player player, int buff_id ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.RemovePermaBuff( buff_id );
		}
	}
}
