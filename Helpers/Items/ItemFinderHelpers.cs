using HamstarHelpers.Classes.Errors;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to finding items in collections.
	/// </summary>
	public partial class ItemFinderHelpers {
		/// <summary>
		/// Finds index of the first valid item of a set of item types within a given collection.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="itemTypes"></param>
		/// <returns>Index within the given collection.</returns>
		public static int FindIndexOfFirstOfItemInCollection( Item[] collection, ISet<int> itemTypes ) {
			for( int i = 0; i < collection.Length; i++ ) {
				Item item = collection[i];
				if( item == null || item.IsAir ) { continue; }
				if( itemTypes.Contains( item.type ) ) { return i; }
			}
			return -1;
		}


		/// <summary>
		/// Finds each valid item of a set of item types within a given collection.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="itemTypes"></param>
		/// <returns>Set of indices within the given collection.</returns>
		public static ISet<int> FindIndexOfEach( Item[] collection, ISet<int> itemTypes ) {
			var set = new SortedSet<int>();

			for( int i=0; i<collection.Length; i++ ) {
				Item item = collection[i];
				if( item == null || item.IsAir ) { continue; }

				if( itemTypes.Contains( item.type ) ) { set.Add( i ); }
			}
			return set;
		}


		/// <summary>
		/// Finds each valid item of a set of item types within a given collection.
		/// </summary>
		/// <param name="indexedCollection"></param>
		/// <param name="itemTypes"></param>
		/// <returns></returns>
		public static IDictionary<int, Item> FindIndexOfEach( IDictionary<int, Item> indexedCollection, ISet<int> itemTypes ) {
			IDictionary<int, Item> found = new Dictionary<int, Item>();

			foreach( var whereItem in indexedCollection ) {
				int where = whereItem.Key;
				Item item = whereItem.Value;
				if( item == null || item.IsAir ) { continue; }

				if( itemTypes.Contains( item.type ) ) {
					found[where] = item;
				}
			}

			return found;
		}	// <- Used to be 'FindChangesAt'


		/// <summary>
		/// Counts total valid items of a set of item types within a given collection.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="itemTypes"></param>
		/// <returns></returns>
		public static int CountTotalOfEach( Item[] collection, ISet<int> itemTypes ) {
			var set = ItemFinderHelpers.FindIndexOfEach( collection, itemTypes );
			int total = 0;

			foreach( int idx in set ) {
				total += collection[idx].stack;
			}
			return total;
		}


		/// <summary>
		/// Filters a given collection of item types (mapped typically to inventory indices) by a set of filter types.
		/// </summary>
		/// <param name="entries">Collection to build a filtered collection from. Keys are typically inventory indices, values are
		/// item types and stack sizes.</param>
		/// <param name="types">Item types to filter by.</param>
		/// <returns>A new collection (of the same format as the parameter), filtered by the given filter set.</returns>
		public static IDictionary<int, KeyValuePair<int, int>> FilterByTypes(
				IDictionary<int, KeyValuePair<int, int>> entries,
				ISet<int> types ) {
			IDictionary<int, KeyValuePair<int, int>> found = new Dictionary<int, KeyValuePair<int, int>>();

			foreach( var whereItem in entries ) {
				int invIdx = whereItem.Key;
				int itemType = whereItem.Value.Key;
				int itemStack = whereItem.Value.Value;

				if( types.Contains( itemType ) ) {
					found[ invIdx ] = whereItem.Value;
				}
			}

			return found;
		}


		/// <summary>
		/// Finds first item of a given type belonging to a player (including their bank storage).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="itemType"></param>
		/// <param name="skipArmors"></param>
		/// <param name="skipBanks"></param>
		/// <returns></returns>
		public static Item FindFirstPlayerItemOfType( Player player, int itemType, bool skipArmors=false, bool skipBanks=false ) {
			int itemIdx;
			var itemTypeSet = new HashSet<int> { itemType };

			itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, itemTypeSet );
			if( itemIdx != -1 ) {
				return player.inventory[ itemIdx ];
			}

			if( !skipArmors ) {
				itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.armor, itemTypeSet );
				if( itemIdx != -1 ) {
					return player.armor[itemIdx];
				}
			}

			if( !skipBanks ) {
				if( player.chest >= 0 ) {   // Player's current chest
					itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( Main.chest[player.chest].item, itemTypeSet );
					if( itemIdx != -1 ) {
						return Main.chest[player.chest].item[itemIdx];
					}
				}
				if( player.chest == -2 ) {  // Piggy bank
					itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank.item, itemTypeSet );
					if( itemIdx != -1 ) {
						return player.bank.item[itemIdx];
					}
				}
				if( player.chest == -3 ) {  // Safe
					itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank2.item, itemTypeSet );
					if( itemIdx != -1 ) {
						return player.bank2.item[itemIdx];
					}
				}
				if( player.chest == -4 ) {  // ..whatever this is
					itemIdx = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank3.item, itemTypeSet );
					if( itemIdx != -1 ) {
						return player.bank3.item[itemIdx];
					}
				}
			}

			return null;
		}


		/// <summary>
		/// Finds changes between 2 same-sized arrays of items.
		/// </summary>
		/// <param name="prevItems"></param>
		/// <param name="currItems"></param>
		/// <param name="skipCoins"></param>
		/// <returns>Set of array indices of changed items between the 2 collections. Mapped to a "direction" indicator of
		/// changes.</returns>
		public static IDictionary<int, int> FindChanges( Item[] prevItems, Item[] currItems, bool skipCoins=true ) {
			if( prevItems.Length != currItems.Length ) {
				throw new ModHelpersException( "Mismatched item array sizes." );
			}

			IDictionary<int, int> changes = new Dictionary<int, int>();
			int len = currItems.Length;
			Item prevItem, currItem;

			for( int i = 0; i < len; i++ ) {
				prevItem = prevItems[i];
				currItem = currItems[i];

				if( prevItem == null ) {
					if( currItem != null ) {
						changes[i] = 1;
					}
					continue;
				} else if( currItem == null ) {
					if( prevItem != null ) {
						changes[i] = -1;
					}
					continue;
				}

				if( (prevItem.type != currItem.type || prevItem.stack != currItem.stack) && prevItem.IsAir != currItem.IsAir ) {
					if( prevItem.IsAir ) {
						changes[i] = 1;
					} else if( currItem.IsAir ) {
						changes[i] = -1;
					} else {
						changes[i] = prevItem.stack < currItem.stack ? 1 : -1;
					}
				}
			}

			if( skipCoins ) {
				foreach( var kv in changes ) {
					int itemIdx = kv.Key;
					int changeDir = kv.Value;

					currItem = currItems[itemIdx];
					prevItem = prevItems[itemIdx];

					if( currItem == null || currItem.IsAir ) {
						if( prevItem.type >= 71 && prevItem.type <= 74 ) {
							changes.Remove( itemIdx );
							continue;
						}
					}

					if( currItems[itemIdx].type >= 71 || currItems[itemIdx].type <= 74 ) {
						changes.Remove( itemIdx );
						continue;
					}
				}
			}

			return changes;
		}


		/// <summary>
		/// Finds all items of a given array with a given buy value.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="buyValue"></param>
		/// <returns></returns>
		public static ISet<int> FindPossiblePurchaseTypes( Item[] items, long buyValue ) {	// Use with NPCTownHelpers.GetCurrentShop()
			ISet<int> possiblePurchases = new HashSet<int>();

			for( int i = 0; i < items.Length; i++ ) {
				Item shopItem = items[i];
				if( shopItem == null || shopItem.IsAir ) { continue; }
				if( shopItem.value != buyValue ) { continue; }
				
				possiblePurchases.Add( shopItem.type );
			}

			return possiblePurchases;
		}
	}
}
