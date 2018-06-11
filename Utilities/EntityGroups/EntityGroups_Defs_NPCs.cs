using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineNPCGroups1( Action<string, Func<NPC, bool>> add_def ) {
			// Weapon Classes

			add_def( "Any Slime", ( NPC npc ) => {
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
				}
				return false;
			} );
		}
	}
}
