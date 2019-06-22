using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.NPCs {
	/** <summary>Assorted static "helper" functions pertaining to finding world NPCs.</summary> */
	public static class NPCFinderHelpers {
		private static IDictionary<int, int> AnyWhoOfType = new Dictionary<int, int>();



		////////////////
		
		public static NPC FindFirstNpcByType( int type ) {
			if( NPCFinderHelpers.AnyWhoOfType.Keys.Contains( type ) ) {
				NPC npc = Main.npc[NPCFinderHelpers.AnyWhoOfType[type]];
				if( npc != null && npc.active && npc.type == type ) {
					return npc;
				}
			}

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active && npc.type == type ) {
					NPCFinderHelpers.AnyWhoOfType[type] = i;
					return npc;
				}
			}

			return null;
		}
	}
}
