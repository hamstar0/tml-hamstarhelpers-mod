using HamstarHelpers.Helpers.Items;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	public static class PlayerItemFinderHelpers {
		public static ISet<int> FindPossiblePurchaseTypes( Player player, long spent ) {
			ISet<int> possiblePurchases = new HashSet<int>();

			if( Main.npcShop <= 0 || Main.npcShop > Main.instance.shop.Length ) {
				return possiblePurchases;
			}
			Item[] shopItems = Main.instance.shop[Main.npcShop].item;

			for( int i = 0; i < shopItems.Length; i++ ) {
				Item shopItem = shopItems[i];
				if( shopItem == null || shopItem.IsAir ) { continue; }

				if( shopItem.value == spent ) {
					// If shop item type occurs more than once, skip
					int j;
					for( j = 0; j < i; j++ ) {
						if( shopItems[j].type == shopItem.type ) {
							break;
						}
					}
					if( j != i ) { continue; }

					possiblePurchases.Add( shopItem.type );
				}
			}

			return possiblePurchases;
		}

		////////////////

		public static Item FindFirstOfItemFor( Player player, ISet<int> itemTypes ) {
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
