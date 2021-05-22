using Terraria;


namespace HamstarHelpers.Libraries.Buffs {
	/// <summary>
	/// Assorted static "helper" functions pertaining to buffs.
	/// </summary>
	public partial class BuffLibraries {
		/// <summary>
		/// Adds a permanent buff to a player. Will persist across saves.
		/// </summary>
		/// <param name="player">Player to apply buff to.</param>
		/// <param name="buffId">ID of buff.</param>
		public static void AddPermaBuff( Player player, int buffId ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.AddPermaBuff( buffId );
		}

		/// <summary>
		/// Removes a permanent buff from a player.
		/// </summary>
		/// <param name="player">Player to remove buff from.</param>
		/// <param name="buffId">ID of buff.</param>
		public static void RemovePermaBuff( Player player, int buffId ) {
			var myplayer = player.GetModPlayer<ModHelpersPlayer>();
			myplayer.Logic.RemovePermaBuff( buffId );
		}
	}
}
