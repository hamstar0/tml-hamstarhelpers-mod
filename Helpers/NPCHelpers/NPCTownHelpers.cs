using HamstarHelpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace HamstarHelpers.NPCHelpers {
	public static class NPCTownHelpers {
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
