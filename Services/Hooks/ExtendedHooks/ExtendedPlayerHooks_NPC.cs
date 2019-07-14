using System;
using Terraria;


namespace HamstarHelpers.Services.Hooks.ExtendedHooks {
	/// <summary>
	/// Supplies custom tModLoader-like, delegate-based hooks for player-relevant functions not currently available in
	/// tModLoader.
	/// </summary>
	public partial class ExtendedPlayerHooks {
		/// <summary>
		/// Declares an action to run when an NPC is killed by a given player.
		/// </summary>
		/// <param name="action">Action to run. Includes the Player (killer) and killed NPC as parameters.</param>
		public static void AddNpcKillHook( Action<Player, NPC> action ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			playerHooks.NpcKillHooks.Add( action );
		}


		////////////////

		internal static void RunNpcKillHooks( Player player, NPC npc ) {
			var playerHooks = ModHelpersMod.Instance.PlayerHooks;

			foreach( var hook in playerHooks.NpcKillHooks ) {
				hook( player, npc );
			}
		}
	}
}
