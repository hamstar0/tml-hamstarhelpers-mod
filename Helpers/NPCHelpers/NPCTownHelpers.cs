using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCTownHelpers {
		public static void Spawn( int town_npc_type, int tile_x, int tile_y ) {
			int npc_who = NPC.NewNPC( tile_x * 16, tile_y * 16, town_npc_type, 1, 0f, 0f, 0f, 0f, 255 );
			NPC npc = Main.npc[ npc_who ];

			Main.townNPCCanSpawn[ town_npc_type ] = false;
			npc.homeTileX = tile_x;
			npc.homeTileY = tile_y;
			npc.homeless = true;

			if( tile_x < WorldGen.bestX ) {
				npc.direction = 1;
			} else {
				npc.direction = -1;
			}

			npc.netUpdate = true;

			if( Main.netMode == 0 ) {
				Main.NewText( Language.GetTextValue( "Announcement.HasArrived", npc.FullName ), 50, 125, 255, false );
			} else if( Main.netMode == 2 ) {
				var msg = NetworkText.FromKey( "Announcement.HasArrived", new object[] { npc.GetFullNetName() } );
				NetMessage.BroadcastChatMessage( msg, new Color( 50, 125, 255 ), -1 );
			}

			//AchievementsHelper.NotifyProgressionEvent( 8 );
			//if( Main.npc[ npc_who ].type == 160 ) {
			//	AchievementsHelper.NotifyProgressionEvent( 18 );
			//}
		}


		public static void Leave( NPC npc, bool announce = true ) {
			int whoami = npc.whoAmI;
			if( announce ) {
				string msg = Main.npc[whoami].GivenName + " the " + Main.npc[whoami].TypeName + " " + Lang.misc[35];

				if( Main.netMode == 0 ) {
					Main.NewText( msg, 50, 125, 255, false );
				} else if( Main.netMode == 2 ) {
					NetMessage.SendData( 25, -1, -1, NetworkText.FromLiteral( msg ), 255, 50f, 125f, 255f, 0, 0, 0 );
				}
			}
			Main.npc[whoami].active = false;
			Main.npc[whoami].netSkip = -1;
			Main.npc[whoami].life = 0;
			NetMessage.SendData( 23, -1, -1, null, whoami, 0f, 0f, 0f, 0, 0, 0 );
		}


		public static Chest GetShop( int npc_type ) {
			if( Main.instance == null ) {
				LogHelpers.Log( "No main instance." );
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
	}
}
