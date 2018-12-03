using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class ModHelpersNPC : GlobalNPC {
		public override void NPCLoot( NPC npc ) {
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
		}


		public override bool PreAI( NPC npc ) {
			var mymod = (ModHelpersMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusNPCEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				float radius = ent.GetComponentByType<HitRadiusNPCEntityComponent>().Radius;

				if( Vector2.Distance( ent.Core.Center, npc.Center ) <= radius ) {
					this.ApplyHits( ent, npc );
				}
			}

			return true;
		}

		////////////////

		private void ApplyHits( CustomEntity ent, NPC npc ) {
			int dmg = npc.damage;
			var hitRadComp = ent.GetComponentByType<HitRadiusNPCEntityComponent>();

			if( hitRadComp.PreHurt( ent, npc, ref dmg ) ) {
				hitRadComp.PostHurt( ent, npc, dmg );
			}
		}
	}
}
