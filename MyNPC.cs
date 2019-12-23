using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Items;
using HamstarHelpers.Services.Hooks.ExtendedHooks;
using HamstarHelpers.Services.NPCChat;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers {
	/// @private
	class ModHelpersNPC : GlobalNPC {
		public override bool? CanChat( NPC npc ) {
			if( !Main.mouseRight || !Main.npcChatRelease ) {
				return base.CanChat( npc );
			}

			Timers.SetTimer( "ModHelpersTownNPCChatOverride", 1, true, () => {
				this.GetChatAssurred( npc, ref Main.npcChatText );
				return false;
			} );

			return base.CanChat( npc );
		}

		public override void GetChat( NPC npc, ref string chat ) {
			this.GetChatAssurred( npc, ref chat );
		}

		////

		private bool GetChatAssurred( NPC npc, ref string chat ) {
			bool? isNewChat;

			while( true) {
				isNewChat = NPCChat.GetChat( npc, ref chat );
				if( isNewChat.HasValue ) {
					break;
				}

				chat = npc.GetChat();
			}

			return isNewChat.Value;
		}


		////////////////

		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( ModHelpersConfig.Instance.GeoResonantOrbSoldByDryad ) {
				if( type == NPCID.Dryad ) {
					var orbItem = new Item();
					orbItem.SetDefaults( ModContent.ItemType<GeoResonantOrbItem>() );

					shop.item[ nextSlot++ ] = orbItem;
				}
			}
		}


		////////////////

		public override void NPCLoot( NPC npc ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_A", 1 );
			if( npc.lastInteraction >= 0 && npc.lastInteraction < Main.player.Length ) {
				this.NpcKilledByPlayer( npc );
			}

			if( ModHelpersConfig.Instance.MagiTechScrapMechBossDropsEnabled ) {
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
