using HamstarHelpers.ItemHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.PlayerHelpers {
	public static class PlayerItemHelpers {
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


		public static void DropInventoryItem( Player player, int slot ) {
			if( slot == 58 && player.whoAmI == Main.myPlayer ) {
				Main.mouseItem = new Item();
			}

			Item item = player.inventory[ slot ];

			if( item != null && !item.IsAir ) {
				int idx = Item.NewItem( player.position, item.width, item.height, item.type, item.stack, false, item.prefix, false, false );

				item.position = Main.item[idx].position;
				Main.item[idx] = item;

				if( Main.netMode == 1 ) {   // Client
					NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
				}

				player.inventory[slot] = new Item();
			}
		}


		public static void DropEquippedItem( Player player, int slot ) {
			Item item = player.armor[slot];

			if( item != null && !item.IsAir ) {
				int idx = Item.NewItem( player.position, item.width, item.height, item.type, item.stack, false, item.prefix, false, false );

				item.position = Main.item[idx].position;
				Main.item[idx] = item;

				if( Main.netMode == 1 ) {   // Client
					NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
				}

				player.armor[slot] = new Item();
			}
		}


		public static bool UnhandItem( Player player ) {
			bool is_unhanded = false;

			// Drop mouse item always
			if( player.selectedItem == 58 ) {
				PlayerItemHelpers.DropInventoryItem( player, 58 );
				is_unhanded = true;
			}
			// Preferably select a blank slot
			if( !is_unhanded ) {
				for( int i = 0; i < player.inventory.Length; i++ ) {
					if( player.inventory[i] == null || player.inventory[i].IsAir ) {
						player.selectedItem = i;
						is_unhanded = true;
						break;
					}
				}
			}
			// Otherwise select a non-usable item
			if( !is_unhanded ) {
				for( int i = 0; i < player.inventory.Length; i++ ) {
					Item item = player.inventory[i];
					if( item != null && item.holdStyle == 0 && item.createTile == -1 && !item.potion && item.useStyle == 0 ) {
						player.selectedItem = i;
						is_unhanded = true;
						break;
					}
				}
			}
			// Otherwise select a non-held item
			if( !is_unhanded ) {
				for( int i = 12; i < player.inventory.Length; i++ ) {
					Item item = player.inventory[i];
					if( item != null && item.holdStyle == 0 ) {
						player.selectedItem = i;
						is_unhanded = true;
						break;
					}
				}
			}
			
			player.noItems = true;

			return is_unhanded;
		}


		public static bool IsPlayerNaked( Player player, bool not_vanity = false ) {
			// Armor
			if( !player.armor[0].IsAir ) { return false; }
			if( !player.armor[1].IsAir ) { return false; }
			if( !player.armor[2].IsAir ) { return false; }
			// Accessory
			if( !player.armor[3].IsAir && !player.hideVisual[3] ) { return false; }
			if( !player.armor[4].IsAir && !player.hideVisual[4] ) { return false; }
			if( !player.armor[5].IsAir && !player.hideVisual[5] ) { return false; }
			if( !player.armor[6].IsAir && !player.hideVisual[6] ) { return false; }
			if( !player.armor[7].IsAir && !player.hideVisual[7] ) { return false; }
			if( not_vanity ) {
				// Vanity
				if( !player.armor[8].IsAir ) { return false; }
				if( !player.armor[9].IsAir ) { return false; }
				if( !player.armor[10].IsAir ) { return false; }
				// Vanity Accessory
				if( !player.armor[11].IsAir /*&& !player.hideVisual[3]*/ ) { return false; }
				if( !player.armor[12].IsAir /*&& !player.hideVisual[4]*/ ) { return false; }
				if( !player.armor[13].IsAir /*&& !player.hideVisual[5]*/ ) { return false; }
				if( !player.armor[14].IsAir /*&& !player.hideVisual[6]*/ ) { return false; }
				if( !player.armor[15].IsAir /*&& !player.hideVisual[7]*/ ) { return false; }
			}

			return true;
		}


		public static long CountMoney( Player player ) {
			bool _;
			long inv_count = Utils.CoinsCount( out _, player.inventory, new int[] { 58, 57, 56, 55, 54 } );
			long bank_count = Utils.CoinsCount( out _, player.bank.item, new int[0] );
			long bank2_count = Utils.CoinsCount( out _, player.bank2.item, new int[0] );
			long bank3_count = Utils.CoinsCount( out _, player.bank3.item, new int[0] );
			return Utils.CoinsCombineStacks( out _, new long[] { inv_count, bank_count, bank2_count, bank3_count } );
		}
		

		public static Vector2 TipOfHeldItem( Player player ) {
			Item item = player.HeldItem;
			if( item == null || item.IsAir ) { return Vector2.Zero; }
			Vector2 pos = player.RotatedRelativePoint( player.MountedCenter, true );

			int wid = Main.itemTexture[item.type].Width;
			int length = wid;
			
			if( item.useStyle != 5 ) {
				int hei = Main.itemTexture[item.type].Height;
				length = (int)Math.Sqrt( wid * wid + hei * hei );
			}

			float reach = ((float)length + 6f) * (float)player.direction;

			if( item.useStyle == 4 ) {
				return pos + new Vector2( reach/4f, -28f );
			}

			return pos + (player.itemRotation.ToRotationVector2() * reach);
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
	}
}
