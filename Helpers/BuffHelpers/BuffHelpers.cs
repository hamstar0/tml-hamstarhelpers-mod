using Terraria;


namespace HamstarHelpers.Helpers.BuffHelpers {
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
