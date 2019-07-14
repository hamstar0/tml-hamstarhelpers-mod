using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineNPCGroups1( IList<EntityGroupMatcherDefinition<NPC>> defs ) {
			// General

			defs.Add( new EntityGroupMatcherDefinition<NPC>( 
				"Any Friendly NPC", null,
				new NPCGroupMatcher( ( npc, grp ) => {
					return npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				"Any Hostile NPC", null,
				new NPCGroupMatcher( ( npc, grp ) => {
					return !npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				"Any Town NPC", null,
				new NPCGroupMatcher( ( npc, grp ) => {
					return npc.townNPC;
				} )
			) );

			// Monsters

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				"Any Boss", null,
				new NPCGroupMatcher( ( npc, grp ) => {
					if( npc.type == NPCID.EaterofWorldsHead ) { return true; }	// special case
					return npc.boss;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				"Any Slime", null,
				new NPCGroupMatcher( ( npc, grp ) => {
					if( npc.aiStyle == 1 ) {
						switch( npc.netID ) {
						case NPCID.HoppinJack:	//?
						case NPCID.Grasshopper:
						case NPCID.GoldGrasshopper:
							return false;
						}
						return true;
					} else {
						switch( npc.netID ) {
						case NPCID.Slimer:
						case NPCID.Slimer2:
						case NPCID.Gastropod:
							return true;
						}
						return false;
					}
				} )
			) );
		}
	}
}
