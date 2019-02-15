using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Items;
using HamstarHelpers.Services.DataStore;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			if( !mymod.Config.MagiTechScrapMechBossDropsEnabled ) { return; }

			int scrapType = mymod.ItemType<MagiTechScrapItem>();

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
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_B", 1 );
		}


		public override bool PreAI( NPC npc ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_A", 1 );
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusNpcEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				var hitRadComp = ent.GetComponentByType<HitRadiusNpcEntityComponent>();
				float radius = hitRadComp.GetRadius( ent );

				if( Vector2.Distance( ent.Core.Center, npc.Center ) <= radius ) {
					int dmg = npc.damage;

					if( hitRadComp.PreHurt( ent, npc, ref dmg ) ) {
						hitRadComp.PostHurt( ent, npc, dmg );
					}
				}
			}
			
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_B", 1 );
			return true;
		}
	}
}
