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
			int number = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode == 1 ) {
				NetMessage.SendData( 21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0 );
			}
			return number;
		}

		public static void DestroyItem( Item item ) {
			item.active = false;
			item.type = 0;
			//item.name = "";
			item.stack = 0;
		}

		public static void DestroyWorldItem( int i ) {
			Item item = Main.item[i];
			ItemHelpers.DestroyItem( item );

			if( Main.netMode == 2 ) {
				NetMessage.SendData( 21, -1, -1, null, i );
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


		public static Item GetGrappleItem( Player player ) {
			if( ItemIdentityHelpers.IsGrapple( player.miscEquips[4] ) ) {
				return player.miscEquips[4];
			}
			for( int i = 0; i < 58; i++ ) {
				if( Main.projHook[player.inventory[i].shoot] ) {
					return player.inventory[i];
				}
			}
			return null;
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
	}
}
