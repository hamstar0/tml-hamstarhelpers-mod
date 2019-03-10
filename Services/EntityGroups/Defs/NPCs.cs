using System;
using Terraria.ID;

using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		private void DefineNPCGroups1( Action<string, string[], NPCMatcher> addDef ) {
			// General

			addDef( "Any Friendly NPC", null, ( npc, grp ) => {
				return npc.friendly;
			} );
			addDef( "Any Hostile NPC", null, ( npc, grp ) => {
				return !npc.friendly;
			} );
			addDef( "Any Town NPC", null, ( npc, grp ) => {
				return npc.townNPC;
			} );

			// Monsters

			addDef( "Any Boss", null, ( npc, grp ) => {
				if( npc.type == NPCID.EaterofWorldsHead ) { return true; }	// special case
				return npc.boss;
			} );

			addDef( "Any Slime", null, ( npc, grp ) => {
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
			} );
		}
	}
}
