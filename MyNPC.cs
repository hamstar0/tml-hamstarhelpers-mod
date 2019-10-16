using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Items;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_A", 1 );
			if( npc.lastInteraction >= 0 && npc.lastInteraction < Main.player.Length ) {
				this.NpcKilledByPlayer( npc );
			}

			if( ModHelpersMod.Config.MagiTechScrapMechBossDropsEnabled ) {
				int scrapType = ModContent.ItemType<MagiTechScrapItem>();

				switch( npc.type ) {
				case NPCID.Retinazer:
				case NPCID.Spazmatism:
					ItemHelpers.CreateItem( npc.position, scrapType, 5, 24, 24 );
					break;
				case NPCID.TheDestroyer:
				case NPCID.SkeletronPrime:
					ItemHelpers.CreateItem( npc.position, scrapType, 10, 24, 24 );
					break;
				}
			}
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_B", 1 );
		}


		////////////////

		private void NpcKilledByPlayer( NPC npc ) {
			var mymod = (ModHelpersMod)this.mod;

			if( Main.netMode == 2 ) {
				Player toPlayer = Main.player[npc.lastInteraction];

				if( toPlayer != null && toPlayer.active ) {
					ExtendedPlayerHooks.RunNpcKillHooks( toPlayer, npc );
				}
			} else if( Main.netMode == 0 ) {
				ExtendedPlayerHooks.RunNpcKillHooks( Main.LocalPlayer, npc );
			}
		}
	}
}
