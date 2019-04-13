using System;
using Terraria;


namespace HamstarHelpers.Services.ExtendedHooks {
	public partial class ExtendedPlayerHooks {
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
