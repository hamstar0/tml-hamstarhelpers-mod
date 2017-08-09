using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCHelpers {
		public static string GetUniqueId( NPC npc ) {
			string id = npc.TypeName;

			if( npc.HasGivenName ) { id = npc.GivenName + " " + id; }
			if( npc.modNPC != null ) { id = npc.modNPC.mod.Name + " " + id; }
			
			if( id != "" ) { return id; }
			return ""+npc.type;
		}


		public static bool IsNPCDead( NPC check_npc ) {
			return check_npc.life <= 0 || !check_npc.active;
		}


		public static void Kill( NPC npc ) {
			npc.life = 0;
			npc.checkDead();
			npc.active = false;
			NetMessage.SendData( 28, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0 );
		}



		////////////////
		
		[System.Obsolete( "use NPCTownHelpers.Leave", true )]
		public static void Leave( NPC npc, bool announce=true ) {
			NPCTownHelpers.Leave( npc, announce );
		}

		[System.Obsolete( "use NPCTownHelpers.GetShop", true )]
		public static Chest GetShop( int npc_type ) {
			return NPCTownHelpers.GetShop( npc_type );
		}


		[System.Obsolete( "use NPCTownHelpers.GetFemaleTownNpcTypes", true )]
		public static ISet<int> GetFemaleTownNpcTypes() {
			return NPCTownHelpers.GetFemaleTownNpcTypes();
		}

		[System.Obsolete( "use NPCTownHelpers.GetNonGenderedTownNpcTypes", true )]
		public static ISet<int> GetNonGenderedTownNpcTypes() {
			return NPCTownHelpers.GetNonGenderedTownNpcTypes();
		}



		/*#region Forced spawns
		private static int ClearBadForcedSpawns( Player player, ISet<int> orig_npc_whos, int find_of_npc_type, float orig_active_npcs ) {
			int npc_who = -1;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || orig_npc_whos.Contains( i ) ) { continue; }

				// Found THE spawn
				if( npc_who == -1 && npc.type == find_of_npc_type ) {
					npc_who = i;
				} else {
					// Otherwise get rid of it
					npc.active = false;
					Main.npc[i] = new NPC();
				}
			}
			player.activeNPCs = orig_active_npcs + (npc_who == -1 ? 0 : 1);

			return npc_who;
		}
		
		public static int ForceSpawnForPlayer( Player player, int find_of_npc_type, int determination ) {
			var other_players = NPCSpawnInfoHelpers.IsolatePlayer( player );
			var npc_whos_snapshot = NPCSpawnInfoHelpers.GetNpcSnapshot();
			float orig_active_npcs = player.activeNPCs;
			int npc_who = -1;

			NPCSpawnInfoHelpers.IsSimulatingSpawns = true;
			// Test spawns
			for( int i = 1; i <= determination; i++ ) {
				NPC.SpawnNPC();

				if( (i % 25) == 0 ) {
					// Force remove spawned npcs that aren't the given type
					npc_who = NPCHelpers.ClearBadForcedSpawns( player, npc_whos_snapshot, find_of_npc_type, orig_active_npcs );
					if( npc_who != -1 ) { break; }
				}
			}
			NPCSpawnInfoHelpers.IsSimulatingSpawns = false;

			// Restore players
			foreach( var kv in other_players ) {
				Main.player[kv.Key] = kv.Value;
			}

			return npc_who;
		}
		#endregion*/
	}
}
