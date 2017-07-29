using HamstarHelpers.NPCHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	class HamstarHelpersNpc : GlobalNPC {
		public override void SetDefaults( NPC npc ) {
			if( Main.netMode == 0 ) {   // Single
				if( NPCSpawnInfoHelpers.IsGaugingSpawnRates ) {
					if( Main.npc[npc.whoAmI] != null && Main.npc[npc.whoAmI].active ) {
						NPCSpawnInfoHelpers.AddSpawn( npc.type );
					}
				}
			}
		}

		public override void EditSpawnRate( Player player, ref int spawn_rate, ref int max_spawns ) {
			if( NPCSpawnInfoHelpers.IsGaugingSpawnRates ) {
				spawn_rate = 1;
				max_spawns = 100;
			}
		}
	}
}
