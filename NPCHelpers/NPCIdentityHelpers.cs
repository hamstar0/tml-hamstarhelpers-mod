using Terraria;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCIdentityHelpers {
		public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }

			if( id != "" ) { return id; }
			return "" + npc.type;
		}


		public static float LooselyAssessThreat( NPC npc ) {
			float damage_factor = (npc.damage / 100f) * (npc.coldDamage ? 1.2f : 1f);
			float defense_factor = 1f + (npc.defense * 0.01f);

			float vitality = ((float)npc.lifeMax / 80000f) * defense_factor;

			//float versatility = 0f;
			//for( int i=0; i<npc.buffImmune.Length; i++ ) {
			//	if( npc.buffImmune[i] ) { versatility++; }
			//}
			//float versatility_factor = 1f + (versatility * 0.01f);
			float mobility_factor = npc.noTileCollide ? 1.2f : 1f;
			float knockback_factor = ((1f - npc.knockBackResist) * 0.1f) + 1f;

			float vitality_factor = vitality * mobility_factor * knockback_factor;  //* versatility_factor

			if( npc.value > 0 ) {
				float value_factor = (float)npc.value / 150000f;
				return (vitality_factor + damage_factor + value_factor) / 3f;
			}

			return (vitality_factor + damage_factor) / 2f;
		}
	}
}
