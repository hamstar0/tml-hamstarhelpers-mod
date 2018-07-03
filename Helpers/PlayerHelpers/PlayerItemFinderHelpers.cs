using HamstarHelpers.Helpers.ItemHelpers;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerItemFinderHelpers {
		public static ISet<int> FindPossiblePurchaseTypes( Player player, long spent ) {
			ISet<int> possible_purchases = new HashSet<int>();

			if( Main.npcShop <= 0 || Main.npcShop > Main.instance.shop.Length ) {
				return possible_purchases;
			}
			Item[] shop_items = Main.instance.shop[Main.npcShop].item;

			for( int i = 0; i < shop_items.Length; i++ ) {
				Item shop_item = shop_items[i];
				if( shop_item == null || shop_item.IsAir ) { continue; }

				if( shop_item.value == spent ) {
					// If shop item type occurs more than once, skip
					int j;
					for( j = 0; j < i; j++ ) {
						if( shop_items[j].type == shop_item.type ) {
							break;
						}
					}
					if( j != i ) { continue; }

					possible_purchases.Add( shop_item.type );
				}
			}

			return possible_purchases;
		}

		////////////////

		public static Item FindFirstOfItemFor( Player player, ISet<int> item_types ) {
			int found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.inventory, item_types );
			if( found != -1 ) {
				return player.inventory[found];
			} else {
				found = ItemFinderHelpers.FindIndexOfFirstOfItemInCollection( player.armor, item_types );
				if( found != -1 ) {
					return player.armor[found];
				}
			}

			return null;
		}


		public static IDictionary<int, KeyValuePair<int, int>> FindInventoryChanges( Player player,
				KeyValuePair<int, int> prev_mouse_info,
				IDictionary<int, KeyValuePair<int, int>> prev_inv ) {
			IDictionary<int, KeyValuePair<int, int>> changes = new Dictionary<int, KeyValuePair<int, int>>();
			int len = player.inventory.Length;

			for( int i = 0; i < len; i++ ) {
				Item item = player.inventory[i];

				if( prev_inv[i].Key != 0 && item == null ) {
					changes[i] = new KeyValuePair<int, int>( 0, 0 );
				} else if( prev_inv[i].Key != item.type || prev_inv[i].Value != item.stack ) {
					changes[i] = new KeyValuePair<int, int>( item.type, item.stack );
				}
			}

			if( prev_mouse_info.Key != 0 && Main.mouseItem == null ) {
				changes[-1] = new KeyValuePair<int, int>( 0, 0 );
			} else if( prev_mouse_info.Key != Main.mouseItem.type || prev_mouse_info.Value != Main.mouseItem.stack ) {
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

				Item ammo_item = player.inventory[i];
				if( ammo_item == null || ammo_item.IsAir ) { continue; }

				if( ammo_item.ammo == weapon.useAmmo ) {
					return ammo_item;
				}

				first = false;
			}

			return null;
		}
	}
}
