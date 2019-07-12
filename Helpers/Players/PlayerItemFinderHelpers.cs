using HamstarHelpers.Helpers.Items;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to unique player identification.
	/// </summary>
	public class PlayerItemFinderHelpers {
		/// <summary>
		/// Finds first of any of a set of item types currently in the player's position.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="itemTypes"></param>
		/// <param name="includeBanks"></param>
		/// <returns></returns>
		public static Item FindFirstOfPossessedItemFor( Player player, ISet<int> itemTypes, bool includeBanks ) {
			int found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, itemTypes );
			if( found != -1 ) {
				return player.inventory[found];
			}

			found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.armor, itemTypes );
			if( found != -1 ) {
				return player.armor[found];
			}

			found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.miscEquips, itemTypes );
			if( found != -1 ) {
				return player.miscEquips[found];
			}

			if( includeBanks ) {
				found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank.item, itemTypes );
				if( found != -1 ) {
					return player.bank.item[found];
				}
				found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank2.item, itemTypes );
				if( found != -1 ) {
					return player.bank2.item[found];
				}
				found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.bank3.item, itemTypes );
				if( found != -1 ) {
					return player.bank3.item[found];
				}
			}

			return null;
		}


		/// <summary>
		/// Find item changes in a given player's inventory between a previous snapshot.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="prevMouseInfo">Mouse item type and stack size.</param>
		/// <param name="prevInv">Map of inventory slots and their item types and sizes.</param>
		/// <returns>Map of inventory slots with changes and their item types and sizes. Mouse items count as the -1 slot.</returns>
		public static IDictionary<int, KeyValuePair<int, int>> FindInventoryChanges( Player player,
					KeyValuePair<int, int> prevMouseInfo,
					IDictionary<int, KeyValuePair<int, int>> prevInv ) {
			IDictionary<int, KeyValuePair<int, int>> changes = new Dictionary<int, KeyValuePair<int, int>>();
			int len = player.inventory.Length;

			for( int i = 0; i < len; i++ ) {
				Item item = player.inventory[i];

				if( prevInv[i].Key != 0 && item == null ) {
					changes[i] = new KeyValuePair<int, int>( 0, 0 );
				} else if( prevInv[i].Key != item.type || prevInv[i].Value != item.stack ) {
					changes[i] = new KeyValuePair<int, int>( item.type, item.stack );
				}
			}

			if( prevMouseInfo.Key != 0 && Main.mouseItem == null ) {
				changes[-1] = new KeyValuePair<int, int>( 0, 0 );
			} else if( prevMouseInfo.Key != Main.mouseItem.type || prevMouseInfo.Value != Main.mouseItem.stack ) {
				changes[-1] = new KeyValuePair<int, int>( Main.mouseItem.type, Main.mouseItem.stack );
			}

			return changes;
		}


		/// <summary>
		/// Gets the current ammo item of a given player expected to be in use by a given weapon item.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="weapon"></param>
		/// <returns></returns>
		public static Item GetCurrentAmmo( Player player, Item weapon ) {
			if( weapon.useAmmo == 0 ) { return null; }

			bool first = true;

			for( int i = 54; i < player.inventory.Length; i++ ) {
				if( i == 58 ) { i = 0; }
				else if( !first && i == 54 ) { break; }

				Item ammoItem = player.inventory[i];
				if( ammoItem == null || ammoItem.IsAir ) { continue; }

				if( ammoItem.ammo == weapon.useAmmo ) {
					return ammoItem;
				}

				first = false;
			}

			return null;
		}


		/// <summary>
		/// Gets all unoccupied inventory slot indices.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static ISet<int> UnusedInventorySlots( Player player ) {
			var myset = new HashSet<int>();

			for( int i = 0; i < PlayerItemHelpers.VanillaInventoryLastMainSlot; i++ ) {
				if( player.inventory[i] == null || player.inventory[i].IsAir ) {
					myset.Add( i );
				}
			}

			return myset;
		}
	}
}
