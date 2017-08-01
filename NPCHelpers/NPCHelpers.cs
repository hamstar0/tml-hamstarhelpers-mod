using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


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

		public static void Leave( NPC npc, bool announce=true ) {
			int whoami = npc.whoAmI;
			if( announce ) {
				string str = Main.npc[whoami].GivenName + " the " + Main.npc[whoami].TypeName;
				
				if( Main.netMode == 0 ) {
					Main.NewText( str + " " + Lang.misc[35], 50, 125, 255, false );
				} else if( Main.netMode == 2 ) {
					string msg = str + " " + Lang.misc[35];
					NetMessage.SendData( 25, -1, -1, NetworkText.FromLiteral(msg), 255, 50f, 125f, 255f, 0, 0, 0 );
				}
			}
			Main.npc[whoami].active = false;
			Main.npc[whoami].netSkip = -1;
			Main.npc[whoami].life = 0;
			NetMessage.SendData( 23, -1, -1, null, whoami, 0f, 0f, 0f, 0, 0, 0 );
		}


		public static Chest GetShop( int npc_type ) {
			if( Main.instance == null ) {
				ErrorLogger.Log( "No main instance." );
				return null;
			}

			switch( npc_type ) {
			case 17:
				return Main.instance.shop[1];
			case 19:
				return Main.instance.shop[2];
			case 20:
				return Main.instance.shop[3];
			case 38:
				return Main.instance.shop[4];
			case 54:
				return Main.instance.shop[5];
			case 107:
				return Main.instance.shop[6];
			case 108:
				return Main.instance.shop[7];
			case 124:
				return Main.instance.shop[8];
			case 142:
				return Main.instance.shop[9];
			case 160:
				return Main.instance.shop[10];
			case 178:
				return Main.instance.shop[11];
			case 207:
				return Main.instance.shop[12];
			case 208:
				return Main.instance.shop[13];
			case 209:
				return Main.instance.shop[14];
			case 227:
				return Main.instance.shop[15];
			case 228:
				return Main.instance.shop[16];
			case 229:
				return Main.instance.shop[17];
			case 353:
				return Main.instance.shop[18];
			case 368:
				return Main.instance.shop[19];
			case 453:
				return Main.instance.shop[20];
			case 550:
				return Main.instance.shop[21];
			}

			return null;
		}


		public static ISet<int> GetFemaleTownNpcTypes() {
			return new HashSet<int>( new int[] {
				NPCID.Nurse,
				NPCID.PartyGirl,
				NPCID.Stylist,
				NPCID.Dryad,
				NPCID.Mechanic,
				NPCID.Steampunker
			} );
		}

		public static ISet<int> GetNonGenderedTownNpcTypes() {
			return new HashSet<int>( new int[] {
				NPCID.WitchDoctor,	// Indeterminable
				//NPCID.TaxCollector,   // lol!
				NPCID.Truffle,
				//NPCID.Cyborg,	// Cyborgs still have fleshy bits
				//NPCID.SkeletonMerchant	// Dialog suggests male in past life
			} );
		}


		public static int ForceSpawn( Player player, int npc_type ) {
			IDictionary<int, Player> players = new Dictionary<int, Player>();
			ISet<int> npc_whos = new HashSet<int>();
			int npc_who = -1;

			for( int i=0; i< Main.player.Length; i++ ) {
				if( Main.player[i] != null && Main.player[i].active && player.whoAmI != i ) {
					players[i] = Main.player[i];
					Main.player[i] = new Player();
				}
			}

			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					npc_whos.Add( i );
				}
			}

			NPCSpawnInfoHelpers.IsSimulatingSpawns = true;
			for( int i = 0; i < 100; i++ ) {
				NPC.SpawnNPC();

				for( int j = 0; j < Main.npc.Length; j++ ) {
					if( Main.npc[j] == null || !Main.npc[j].active ) { continue; }

					if( !npc_whos.Contains( j ) ) {
						if( Main.npc[j].type == npc_type ) {
							npc_who = j;
							break;
						}

						Main.npc[j].active = false;
						Main.npc[j] = new NPC();
					}
				}

				if( npc_who != -1 ) {
					break;
				}
			}
			NPCSpawnInfoHelpers.IsSimulatingSpawns = false;
			
			foreach( var kv in players ) {
				Main.player[kv.Key] = kv.Value;
			}

			return npc_who;
		}
	}
}
