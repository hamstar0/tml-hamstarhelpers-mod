using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class NPCGroupIDs {
		//"Any Friendly NPC", null,
		//"Any Hostile NPC", null,
		//"Any Town NPC", null,
		//"Any Boss", null,
		//"Any Slime", null,
	}




	partial class EntityGroupDefs {
		internal static void DefineNPCGroups1( IList<EntityGroupMatcherDefinition<NPC>> defs ) {
			// General

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: "Any Friendly NPC",
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: "Any Hostile NPC",
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return !npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: "Any Town NPC",
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return npc.townNPC;
				} )
			) );

			// Monsters

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: "Any Boss",
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					if( npc.type == NPCID.EaterofWorldsHead ) { return true; }  // special case
					return npc.boss;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: "Any Slime",
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					if( npc.aiStyle == 1 ) {
						switch( npc.netID ) {
						case NPCID.HoppinJack:  //?
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
