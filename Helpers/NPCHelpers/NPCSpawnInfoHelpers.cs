using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCSpawnInfoHelpers {
		/*internal static bool IsSimulatingSpawns = false;
		internal static IDictionary<int, float> SpawnRates = new Dictionary<int, float>();
		
		internal static void AddSpawn( int npc_type ) {
			if( !NPCSpawnInfoHelpers.SpawnRates.ContainsKey( npc_type ) ) {
				NPCSpawnInfoHelpers.SpawnRates[npc_type] = 1;
			} else {
				NPCSpawnInfoHelpers.SpawnRates[npc_type] += 1;
			}
		}
		

		internal static IDictionary<int, Player> IsolatePlayer( Player player ) {
			var players = new Dictionary<int, Player>();

			for( int i = 0; i < Main.player.Length; i++ ) {
				if( Main.player[i] != null && Main.player[i].active ) {
					if( Main.player[i].whoAmI != player.whoAmI ) {
						players[i] = Main.player[i];
						Main.player[i] = new Player();
					}
				}
			}
			return players;
		}

		internal static ISet<int> GetNpcSnapshot() {
			ISet<int> npc_whos = new HashSet<int>();

			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					npc_whos.Add( i );
				}
			}
			return npc_whos;
		}

		private static Vector2? ClearGaugedNpcSpawns( Player player, ISet<int> orig_npc_whos, float orig_active_npcs ) {
			Vector2? a_rand_pos = null;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					if( !orig_npc_whos.Contains( i ) ) {
						a_rand_pos = Main.npc[i].position;

						Main.npc[i].active = false;
						Main.npc[i] = new NPC();
					}
				}
			}
			player.activeNPCs = orig_active_npcs;

			return a_rand_pos;
		}
		
		public static IDictionary<int, float> GaugeSpawnRatesForPlayer( Player player, int sample, out Vector2 a_rand_pos ) {
			NPCSpawnInfoHelpers.SpawnRates = new Dictionary<int, float>();
			var other_players = NPCSpawnInfoHelpers.IsolatePlayer( player );
			var npc_whos_snapshot = NPCSpawnInfoHelpers.GetNpcSnapshot();
			float curr_active_npcs = player.activeNPCs;
			Vector2? rand_pos;
			a_rand_pos = Vector2.Zero;

			int curr_net_mode = Main.netMode;
			Main.netMode = 0;

			NPCSpawnInfoHelpers.IsSimulatingSpawns = true;
			for( int i=1; i<=sample; i++ ) {
				NPC.SpawnNPC();
				if( i % 50 == 0 ) {
					rand_pos = NPCSpawnInfoHelpers.ClearGaugedNpcSpawns( player, npc_whos_snapshot, curr_active_npcs );
					if( rand_pos != null ) { a_rand_pos = (Vector2)rand_pos; }
				}
			}
			NPCSpawnInfoHelpers.IsSimulatingSpawns = false;

			Main.netMode = curr_net_mode;

			if( sample % 50 != 0 ) {
				rand_pos = NPCSpawnInfoHelpers.ClearGaugedNpcSpawns( player, npc_whos_snapshot, curr_active_npcs );
				if( rand_pos != null ) { a_rand_pos = (Vector2)rand_pos; }
			}

			foreach( var kv in other_players ) {
				Main.player[kv.Key] = kv.Value;
			}

			return NPCSpawnInfoHelpers.SpawnRates;
		}*/
	}
}
