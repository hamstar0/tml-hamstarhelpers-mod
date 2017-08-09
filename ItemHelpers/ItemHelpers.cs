using HamstarHelpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static class ItemHelpers {
		public static string GetUniqueId( Item item ) {
			if( item.modItem != null ) {
				return item.modItem.mod.Name + " " + item.Name;
			}
			return ""+ item.netID;
		}

		
		public static int CreateItem( Vector2 pos, int type, int stack, int width, int height, int prefix = 0 ) {
			int idx = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode == 1 ) {	// Client
				NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
			}
			return idx;
		}

		public static void DestroyItem( Item item ) {
			item.active = false;
			item.type = 0;
			//item.name = "";
			item.stack = 0;
		}

		public static void DestroyWorldItem( int idx ) {
			Item item = Main.item[idx];
			ItemHelpers.DestroyItem( item );

			if( Main.netMode == 2 ) {	// Server
				NetMessage.SendData( 21, -1, -1, null, idx );
			}
		}


		public static void ReduceStack( Item item, int amt ) {
			item.stack -= amt;

			if( item.stack <= 0 ) {
				item.TurnToAir();
				item.active = false;
			}

			if( Main.netMode != 0 && item.owner == Main.myPlayer && item.whoAmI > 0 ) {
				NetMessage.SendData( 21, -1, -1, null, item.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}


		public static int CalculateStandardUseTime( Item item ) {
			int use_time;

			// No exact science for this one (Note: No accommodations made for other mods' non-standard use of useTime!)
			if( item.melee || item.useTime == 0 ) {
				use_time = item.useAnimation;
			} else {
				use_time = item.useTime;
				if( item.reuseDelay > 0 ) { use_time = (use_time + item.reuseDelay) / 2; }
			}

			if( item.useTime <= 0 || item.useTime == 100 ) {    // 100 = default amount
				if( item.useAnimation > 0 && item.useAnimation != 100 ) {   // 100 = default amount
					use_time = item.useAnimation;
				} else {
					use_time = 100;
				}
			}

			return use_time;
		}


		[System.Obsolete( "use PlayerItemHelpers.GetGrappleItem", true )]
		public static Item GetGrappleItem( Player player ) {
			return PlayerItemHelpers.GetGrappleItem( player );
		}
	}
}
