using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.ItemHelpers {
	public static class ItemFinderHelpers {
		public static int FindFirstOfItemInCollection( Item[] collection, ISet<int> item_types ) {
			for( int i = 0; i < collection.Length; i++ ) {
				Item item = collection[i];
				if( item.stack == 0 ) { continue; }
				if( item_types.Contains( item.type ) ) { return i; }
			}

			return -1;
		}


		public static IDictionary<int, KeyValuePair<int, int>> FilterByTypes(
				IDictionary<int, KeyValuePair<int, int>> entries,
				ISet<int> types ) {
			IDictionary<int, KeyValuePair<int, int>> found = new Dictionary<int, KeyValuePair<int, int>>();

			foreach( var where_item in entries ) {
				int where = where_item.Key;
				int i_type = where_item.Value.Key;

				if( types.Contains( i_type ) ) {
					found[where] = where_item.Value;
				}
			}

			return found;
		}


		private static IDictionary<long, ISet<int>> SellItems = new Dictionary<long, ISet<int>>();

		public static ISet<int> FindItemsByValue( long sell_value, bool include_coins = false ) {
			if( !ItemFinderHelpers.SellItems.Keys.Contains( sell_value ) ) {
				ItemFinderHelpers.SellItems[sell_value] = new HashSet<int>();
			} else {
				return ItemFinderHelpers.SellItems[sell_value];
			}

			for( int i = 0; i < Main.itemTexture.Length; i++ ) {
				if( !include_coins && i == 71 ) { i = 75; }

				Item item = new Item();
				item.SetDefaults( i );
				if( item.value <= 0 ) { continue; }

				if( sell_value % item.value == 0 ) {
					ItemFinderHelpers.SellItems[sell_value].Add( i );
				}
			}

			return ItemFinderHelpers.SellItems[sell_value];
		}


		public static Item FindFirstPlayerItemOfType( Player player, int item_type ) {
			Item item = ItemFinderHelpers.FindFirstItemOfType( player.inventory, item_type );
			if( item != null ) { return item; }

			if( player.chest >= 0 ) {   // Player's current chest
				item = ItemFinderHelpers.FindFirstItemOfType( Main.chest[player.chest].item, item_type );
				if( item != null ) { return item; }
			}
			if( player.chest == -2 ) {  // Piggy bank
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank.item, item_type );
				if( item != null ) { return item; }
			}
			if( player.chest == -3 ) {  // Safe
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank2.item, item_type );
				if( item != null ) { return item; }
			}
			if( player.chest == -4 ) {  // ..whatever this is
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank3.item, item_type );
				if( item != null ) { return item; }
			}

			return null;
		}

		public static Item FindFirstItemOfType( Item[] items, int item_type ) {
			for( int i = 0; i < items.Length; i++ ) {
				Item item = items[i];
				if( item.type == item_type && item.stack > 0 ) {
					return item;
				}
			}
			return null;
		}


		public static IDictionary<int, bool> FindChanges( Item[] prev_items, Item[] curr_items ) {
			IDictionary<int, bool> changes = new Dictionary<int, bool>();
			int len = curr_items.Length;

			for( int i = 0; i < len; i++ ) {
				Item prev_item = prev_items[i];
				Item curr_item = curr_items[i];

				if( prev_item == null ) {
					if( curr_item != null ) { changes[i] = true; }
					continue;
				} else if( curr_item == null ) {
					if( prev_item != null ) { changes[i] = false; }
					continue;
				}

				if( prev_item.type != curr_item.type || prev_item.stack != curr_item.stack ) {
					changes[i] = prev_item.IsAir || prev_item.stack < curr_item.stack;

					// Skip coins
					if( changes[i] ) {
						if( curr_item.type == 71 || curr_item.type == 72
						|| curr_item.type == 73 || curr_item.type == 74 ) {
							changes.Remove( i );
						}
					} else {
						if( prev_item.type == 71 || prev_item.type == 72
						|| prev_item.type == 73 || prev_item.type == 74 ) {
							changes.Remove( i );
						}
					}
				}
			}

			return changes;
		}


		public static IDictionary<int, Item> FindChangesOf( IDictionary<int, Item> changes_at, ISet<int> types ) {
			IDictionary<int, Item> found = new Dictionary<int, Item>();

			foreach( var where_item in changes_at ) {
				int where = where_item.Key;
				Item item = where_item.Value;

				if( types.Contains( item.type ) ) {
					found[where] = item;
				}
			}

			return found;
		}
	}
}
