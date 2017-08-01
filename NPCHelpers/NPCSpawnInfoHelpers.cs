using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCSpawnInfoHelpers {
		internal static bool IsSimulatingSpawns = false;
		internal static IDictionary<int, float> SpawnRates = new Dictionary<int, float>();
		
		internal static void AddSpawn( int npc_type ) {
			if( !NPCSpawnInfoHelpers.SpawnRates.ContainsKey( npc_type ) ) {
				NPCSpawnInfoHelpers.SpawnRates[npc_type] = 1;
			} else {
				NPCSpawnInfoHelpers.SpawnRates[npc_type] += 1;
			}
		}
		
		

		public static IDictionary<int, float> GaugeSpawnRates( int sample ) {
			NPCSpawnInfoHelpers.SpawnRates = new Dictionary<int, float>();
			
			ISet<int> npc_whos = new HashSet<int>();
			for( int i=0; i<Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					npc_whos.Add( i );
				}
			}

			NPCSpawnInfoHelpers.IsSimulatingSpawns = true;
			for( int i=0; i<sample; i++ ) { NPC.SpawnNPC(); }
			NPCSpawnInfoHelpers.IsSimulatingSpawns = false;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					if( !npc_whos.Contains(i) ) {
						Main.npc[i].active = false;
						Main.npc[i] = new NPC();
					}
				}
			}

			return NPCSpawnInfoHelpers.SpawnRates;
		}
	}
}
