using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class NPCGroupIDs {
		/// <summary></summary>
		public const string AnyFriendlyNPC = "Any Friendly NPC";
		/// <summary></summary>
		public const string AnyHostileNPC = "Any Hostile NPC";
		/// <summary></summary>
		public const string AnyTownNPC = "Any Town NPC";
		/// <summary></summary>
		public const string AnyBoss = "Any Boss";
		/// <summary></summary>
		public const string AnySlime = "Any Slime";
	}




	partial class EntityGroupDefs {
		internal static void DefineNPCGroups1( IList<EntityGroupMatcherDefinition<NPC>> defs ) {
			// General

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: NPCGroupIDs.AnyFriendlyNPC,
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: NPCGroupIDs.AnyHostileNPC,
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return !npc.friendly;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: NPCGroupIDs.AnyTownNPC,
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					return npc.townNPC;
				} )
			) );

			// Monsters

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: NPCGroupIDs.AnyBoss,
				grpDeps: null,
				matcher: new NPCGroupMatcher( ( npc, grp ) => {
					if( npc.type == NPCID.EaterofWorldsHead ) { return true; }  // special case
					return npc.boss;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<NPC>(
				grpName: NPCGroupIDs.AnySlime,
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
