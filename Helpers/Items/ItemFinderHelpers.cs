using HamstarHelpers.Components.DataStructures;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Items {
	/** <summary>Assorted static "helper" functions pertaining to finding items in collections.</summary> */
	public static partial class ItemFinderHelpers {
		private static IDictionary<long, ISet<int>> SellItems = new Dictionary<long, ISet<int>>();



		////////////////

		public static int FindIndexOfFirstOfItemInCollection( Item[] collection, ISet<int> itemTypes ) {
			for( int i = 0; i < collection.Length; i++ ) {
				Item item = collection[i];
				if( item == null || item.IsAir ) { continue; }
				if( itemTypes.Contains( item.type ) ) { return i; }
			}
			return -1;
		}

		
		public static ISet<int> FindIndexOfEach( Item[] collection, ISet<int> itemTypes ) {
			var set = new SortedSet<int>();

			for( int i=0; i<collection.Length; i++ ) {
				Item item = collection[i];
				if( item == null || item.IsAir ) { continue; }

				if( itemTypes.Contains( item.type ) ) { set.Add( i ); }
			}
			return set;
		}

		
		public static int CountTotalOfEach( Item[] collection, ISet<int> itemTypes ) {
			var set = ItemFinderHelpers.FindIndexOfEach( collection, itemTypes );
			int total = 0;

			foreach( int idx in set ) {
				total += collection[idx].stack;
			}
			return total;
		}


		public static IDictionary<int, KeyValuePair<int, int>> FilterByTypes(
				IDictionary<int, KeyValuePair<int, int>> entries,
				ISet<int> types ) {
			IDictionary<int, KeyValuePair<int, int>> found = new Dictionary<int, KeyValuePair<int, int>>();

			foreach( var whereItem in entries ) {
				int where = whereItem.Key;
				int iType = whereItem.Value.Key;

				if( types.Contains( iType ) ) {
					found[where] = whereItem.Value;
				}
			}

			return found;
		}


		public static ReadOnlySet<int> FindItemsByValue( long sellValue, bool includeCoins = false ) {
			if( !ItemFinderHelpers.SellItems.Keys.Contains( sellValue ) ) {
				ItemFinderHelpers.SellItems[sellValue] = new HashSet<int>();
			} else {
				return new ReadOnlySet<int>( ItemFinderHelpers.SellItems[sellValue] );
			}

			for( int i = 0; i < Main.itemTexture.Length; i++ ) {
				if( !includeCoins && i == 71 ) { i = 75; }

				Item item = new Item();
				item.SetDefaults( i );
				if( item.value <= 0 ) { continue; }

				if( sellValue % item.value == 0 ) {
					ItemFinderHelpers.SellItems[sellValue].Add( i );
				}
			}

			return new ReadOnlySet<int>( ItemFinderHelpers.SellItems[sellValue] );
		}


		public static Item FindFirstPlayerItemOfType( Player player, int itemType ) {
			Item item = ItemFinderHelpers.FindFirstItemOfType( player.inventory, itemType );
			if( item != null ) { return item; }

			if( player.chest >= 0 ) {   // Player's current chest
				item = ItemFinderHelpers.FindFirstItemOfType( Main.chest[player.chest].item, itemType );
				if( item != null ) { return item; }
			}
			if( player.chest == -2 ) {  // Piggy bank
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank.item, itemType );
				if( item != null ) { return item; }
			}
			if( player.chest == -3 ) {  // Safe
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank2.item, itemType );
				if( item != null ) { return item; }
			}
			if( player.chest == -4 ) {  // ..whatever this is
				item = ItemFinderHelpers.FindFirstItemOfType( player.bank3.item, itemType );
				if( item != null ) { return item; }
			}

			return null;
		}

		public static Item FindFirstItemOfType( Item[] items, int itemType ) {
			for( int i = 0; i < items.Length; i++ ) {
				Item item = items[i];
				if( item.type == itemType && item.stack > 0 ) {
					return item;
				}
			}
			return null;
		}


		public static IDictionary<int, bool> FindChanges( Item[] prevItems, Item[] currItems ) {
			IDictionary<int, bool> changes = new Dictionary<int, bool>();
			int len = currItems.Length;

			for( int i = 0; i < len; i++ ) {
				Item prevItem = prevItems[i];
				Item currItem = currItems[i];

				if( prevItem == null ) {
					if( currItem != null ) { changes[i] = true; }
					continue;
				} else if( currItem == null ) {
					if( prevItem != null ) { changes[i] = false; }
					continue;
				}

				if( prevItem.type != currItem.type || prevItem.stack != currItem.stack ) {
					changes[i] = prevItem.IsAir || prevItem.stack < currItem.stack;

					// Skip coins
					if( changes[i] ) {
						if( currItem.type == 71 || currItem.type == 72
						|| currItem.type == 73 || currItem.type == 74 ) {
							changes.Remove( i );
						}
					} else {
						if( prevItem.type == 71 || prevItem.type == 72
						|| prevItem.type == 73 || prevItem.type == 74 ) {
							changes.Remove( i );
						}
					}
				}
			}

			return changes;
		}


		public static IDictionary<int, Item> FindChangesOf( IDictionary<int, Item> changesAt, ISet<int> types ) {
			IDictionary<int, Item> found = new Dictionary<int, Item>();

			foreach( var whereItem in changesAt ) {
				int where = whereItem.Key;
				Item item = whereItem.Value;

				if( types.Contains( item.type ) ) {
					found[where] = item;
				}
			}

			return found;
		}


		public static ISet<int> FindPossiblePurchaseTypes( Item[] items, long spent ) {	// Use with NPCTownHelpers.GetCurrentShop()
			ISet<int> possiblePurchases = new HashSet<int>();

			for( int i = 0; i < items.Length; i++ ) {
				Item shopItem = items[i];
				if( shopItem == null || shopItem.IsAir ) { continue; }

				if( shopItem.value == spent ) {
					// If shop item type occurs more than once, skip
					int j;
					for( j = 0; j < i; j++ ) {
						if( items[j].type == shopItem.type ) {
							break;
						}
					}
					if( j != i ) { continue; }

					possiblePurchases.Add( shopItem.type );
				}
			}

			return possiblePurchases;
		}
	}
}
