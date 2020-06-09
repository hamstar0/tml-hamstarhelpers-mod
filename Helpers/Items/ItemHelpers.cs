using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Players;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static functions pertaining to general use of item.
	/// </summary>
	public partial class ItemHelpers {
		/// <summary>
		/// Get all active items found lying around in the world.
		/// </summary>
		/// <returns></returns>
		public static IList<Item> GetActive() {
			var list = new List<Item>();

			for( int i = 0; i < Main.item.Length; i++ ) {
				Item item = Main.item[i];
				if( item != null && item.active && item.type != ItemID.None ) {
					list.Add( item );
				}
			}
			return list;
		}


		////////////////

		/// <summary>
		/// Creates a world item and ensures it syncs.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="type"></param>
		/// <param name="stack"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		public static int CreateItem( Vector2 pos, int type, int stack, int width, int height, int prefix = 0 ) {
			int idx = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
			}
			return idx;
		}

		////////////////

		/// <summary>
		/// Destroyes an item (turns it to air). No sync.
		/// </summary>
		/// <param name="item"></param>
		public static void DestroyItem( Item item ) {
			item.active = false;
			item.type = ItemID.None;
			//item.name = "";
			item.stack = 0;
		}

		/// <summary>
		/// Destroys a world item (ensures sync).
		/// </summary>
		/// <param name="idx"></param>
		public static void DestroyWorldItem( int idx ) {
			Item item = Main.item[idx];
			ItemHelpers.DestroyItem( item );

			if( Main.netMode != NetmodeID.SinglePlayer ) {	// Server
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, idx );
			}
		}


		/// <summary>
		/// Reduces an item's stack, accommodating if it's held in the mouse, if the stack becomes 0, and any needed sync.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amt"></param>
		public static void ReduceStack( Item item, int amt ) {
			int newStackSize = (item.stack >= amt) ? (item.stack - amt) : 0;

			for( int i=0; i<Main.player.Length; i++ ) {
				Player plr = Main.player[i];
				if( plr?.active != true ) { continue; }

				Item selectItem = plr.inventory[ PlayerItemHelpers.VanillaInventorySelectedSlot ];

				if( selectItem == item ) {
					if( Main.mouseItem.type == item.type && Main.mouseItem.stack == item.stack ) {
						selectItem.stack = newStackSize;
						Main.mouseItem.stack = newStackSize;
					}
					break;
				}
			}

			item.stack = newStackSize;

			if( item.stack <= 0 ) {
				item.TurnToAir();
				item.active = false;
			}

			// Only world items need sync?
			if( Main.netMode != NetmodeID.SinglePlayer && item.whoAmI > 0 ) {
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, item.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}

		/// <summary>
		/// Reduces a world item's stack.
		/// </summary>
		/// <param name="idx">Index (`whoAmI`) of item in `Main.item`.</param>
		/// <param name="amt"></param>
		public static void ReduceWorldItemStack( int idx, int amt ) {
			Item item = Main.item[ idx ];
			item.whoAmI = idx;	//needed?

			ItemHelpers.ReduceStack( item, amt );
		}

		////////////////

		/// <summary>
		/// Consumes a quantity of items (by type) from a given item source. Does not sync item changes.
		/// </summary>
		/// <param name="sourceItems"></param>
		/// <param name="consumeAmounts"></param>
		/// <param name="allOrNothing">Consumes the given amount of items only if all of them are present.</param>
		/// <returns>`true` if all items consumed in full.</returns>
		public static bool ConsumeItems( IEnumerable<Item> sourceItems, IDictionary<int, int> consumeAmounts,
					bool allOrNothing ) {
			var testConsumeAmounts = new Dictionary<int, int>( consumeAmounts );

			foreach( Item item in sourceItems ) {
				if( testConsumeAmounts.ContainsKey( item.netID ) ) {
					if( testConsumeAmounts[item.netID] > item.stack ) {
						testConsumeAmounts[item.netID] -= item.stack;
					} else {
						testConsumeAmounts.Remove( item.netID );
					}
				}
			}

			if( allOrNothing && testConsumeAmounts.Count > 0 ) {
				return false;
			}

			foreach( Item item in sourceItems ) {
				if( consumeAmounts.ContainsKey(item.netID) ) {
					if( consumeAmounts[item.netID] > item.stack ) {
						consumeAmounts[item.netID] -= item.stack;
						item.stack = 0;
						item.active = false;
					} else {
						item.stack -= consumeAmounts[ item.netID ];
						consumeAmounts.Remove( item.netID );
					}
				}
			}

			return consumeAmounts.Count == 0;
		}



		////////////////
		
		/// <summary>
		/// Calculates the "use time" of an item, accounting for non-melee `reuseDelay` and items that incur "reuse" via.
		/// animations instead of the internal `useTime` value.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Tick duration between reuses.</returns>
		public static int CalculateStandardUseTime( Item item ) {
			int useTime;

			// No exact science for this one (Note: No accommodations made for other mods' non-standard use of useTime!)
			if( item.melee || item.useTime == 0 ) {
				useTime = item.useAnimation;
			} else {
				useTime = item.useTime;
				if( item.reuseDelay > 0 ) {
					useTime = (useTime + item.reuseDelay) / 2;
				}
			}

			if( item.useTime <= 0 || item.useTime == 100 ) {    // 100 = default amount
				if( item.useAnimation > 0 /*&& item.useAnimation != 100*/ ) {   // 100 = default amount
					useTime = item.useAnimation;
				} else {
					useTime = 100;
				}
			}

			return useTime;
		}


		////////////////

		/// <summary>
		/// Creates the best fitting set and amount of coins totalling the given amount. Does not sync.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="position"></param>
		/// <returns>List of item `whoAmI` values of each coin.</returns>
		public static int[] CreateCoins( long amount, Vector2 position ) {
			var coins = new List<int>( 4 );

			long plat = amount / 1000000L;
			amount -= plat * 1000000L;
			long gold = amount / 100000L;
			amount -= gold * 10000L;
			long silv = amount / 100L;
			amount -= silv * 100L;
			long copp = amount;

			if( plat > 0 ) {
				coins.Add( Item.NewItem(position, ItemID.PlatinumCoin, (int)plat) );
			}
			if( gold > 0 ) {
				coins.Add( Item.NewItem(position, ItemID.GoldCoin, (int)gold ) );
			}
			if( silv > 0 ) {
				coins.Add( Item.NewItem(position, ItemID.SilverCoin, (int)silv ) );
			}
			if( copp > 0 ) {
				coins.Add( Item.NewItem(position, ItemID.CopperCoin, (int)copp ) );
			}

			return coins.ToArray();
		}
	}
}
