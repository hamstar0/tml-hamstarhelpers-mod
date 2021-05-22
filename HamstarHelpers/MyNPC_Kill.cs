using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.Hooks.ExtendedHooks;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersNPC : GlobalNPC {
		public override bool PreNPCLoot( NPC npc ) {
			ExtendedItemHooks.BeginScanningForLootDrops( npc );
			return base.PreNPCLoot( npc );
		}

		public override void NPCLoot( NPC npc ) {
			ExtendedItemHooks.FinishScanningForLootDropsAndThenRunHooks();

			if( npc.lastInteraction >= 0 && npc.lastInteraction < Main.player.Length ) {
				this.NpcKilledByPlayer( npc );
			}
		}


		////////////////

		private void NpcKilledByPlayer( NPC npc ) {
			var mymod = (ModHelpersMod)this.mod;

			if( Main.netMode == NetmodeID.Server ) {
				Player toPlayer = Main.player[npc.lastInteraction];

				if( toPlayer != null && toPlayer.active ) {
					ExtendedPlayerHooks.RunNpcKillHooks( toPlayer, npc );
				}
			} else if( Main.netMode == NetmodeID.SinglePlayer ) {
				ExtendedPlayerHooks.RunNpcKillHooks( Main.LocalPlayer, npc );
			}
		}
	}
}
