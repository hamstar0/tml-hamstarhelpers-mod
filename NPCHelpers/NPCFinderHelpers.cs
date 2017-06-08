using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCFinderHelpers {
		public static int FindNpcTypeByUniqueId( string uid ) {
			NPC npc = new NPC();
			for( int i = Main.npcTexture.Length - 1; i >= 0; i-- ) {
				npc.SetDefaults( i );
				if( NPCHelpers.GetUniqueId( npc ) == uid ) {
					return i;
				}
			}
			return -1;
		}

		
		private static IDictionary<int, int> TypesToWhos = new Dictionary<int, int>();

		public static NPC FindFirstNpcByType( int type ) {
			if( NPCFinderHelpers.TypesToWhos.Keys.Contains( type ) ) {
				NPC npc = Main.npc[NPCFinderHelpers.TypesToWhos[type]];
				if( npc != null && npc.active && npc.type == type ) {
					return npc;
				}
			}

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active && npc.type == type ) {
					NPCFinderHelpers.TypesToWhos[type] = i;
					return npc;
				}
			}

			return null;
		}
	}
}
