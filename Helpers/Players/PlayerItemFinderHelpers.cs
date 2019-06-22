using HamstarHelpers.Helpers.Items;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	/** <summary>Assorted static "helper" functions pertaining to unique player identification.</summary> */
	public static class PlayerItemFinderHelpers {
		public static Item FindFirstOfPossessedItemFor( Player player, ISet<int> itemTypes ) {
			int found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, itemTypes );
			if( found != -1 ) {
				return player.inventory[found];
			} else {
				found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.armor, itemTypes );
				if( found != -1 ) {
					return player.armor[found];
				}
			}

			return null;
		}


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
	}
}
