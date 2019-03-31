using HamstarHelpers.Helpers.PlayerHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.ItemHelpers {
	public static partial class ItemHelpers {
		public static IList<Item> GetActive() {
			var list = new List<Item>();

			for( int i = 0; i < Main.item.Length; i++ ) {
				Item item = Main.item[i];
				if( item != null && item.active && item.type != 0 ) {
					list.Add( item );
				}
			}
			return list;
		}


		////////////////

		public static int CreateItem( Vector2 pos, int type, int stack, int width, int height, int prefix = 0 ) {
			int idx = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode == 1 ) {	// Client
				NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
			}
			return idx;
		}

		////////////////

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
			int newStackSize = (item.stack >= amt) ? (item.stack - amt) : 0;

			if( Main.netMode != 2 && !Main.dedServ ) {
				Item selectItem = Main.LocalPlayer.inventory[ PlayerItemHelpers.VanillaInventorySelectedSlot ];

				if( selectItem == item && Main.mouseItem.type == item.type && Main.mouseItem.stack == item.stack ) {
					selectItem.stack = newStackSize;
					Main.mouseItem.stack = newStackSize;
				}
			}

			item.stack = newStackSize;

			if( item.stack <= 0 ) {
				item.TurnToAir();
				item.active = false;
			}

			if( Main.netMode != 0 && item.owner == Main.myPlayer && item.whoAmI > 0 ) {
				NetMessage.SendData( 21, -1, -1, null, item.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}

		public static void ReduceWorldItemStack( int idx, int amt ) {
			Item item = Main.item[ idx ];
			item.whoAmI = idx;

			ItemHelpers.ReduceStack( item, amt );
		}

		////////////////
		
		public static void ConsumeItems( IDictionary<int, int> consumeAmounts, IEnumerable<Item> sourceItems ) {
			foreach( Item item in sourceItems ) {
				if( consumeAmounts.ContainsKey(item.netID) ) {
					if( consumeAmounts[item.netID] > item.stack ) {
						consumeAmounts[item.netID] -= item.stack;
						item.stack = 0;
						item.active = false;
					} else {
						item.stack -= consumeAmounts[item.netID];
						consumeAmounts.Remove( item.netID );
					}
				}
			}
		}



		////////////////

		public static int CalculateStandardUseTime( Item item ) {
			int useTime;

			// No exact science for this one (Note: No accommodations made for other mods' non-standard use of useTime!)
			if( item.melee || item.useTime == 0 ) {
				useTime = item.useAnimation;
			} else {
				useTime = item.useTime;
				if( item.reuseDelay > 0 ) { useTime = (useTime + item.reuseDelay) / 2; }
			}

			if( item.useTime <= 0 || item.useTime == 100 ) {    // 100 = default amount
				if( item.useAnimation > 0 && item.useAnimation != 100 ) {   // 100 = default amount
					useTime = item.useAnimation;
				} else {
					useTime = 100;
				}
			}

			return useTime;
		}
	}
}
