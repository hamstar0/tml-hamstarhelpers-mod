using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Internals.NetProtocols;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;

namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static partial class PlayerItemHelpers {
		public const int VanillaInventorySize = 58;
		public const int VanillaInventoryHotbarSize = 10;
		public const int VanillaInventoryMainSize = 40;
		public const int VanillaInventoryLastMainSlot = 49;
		public const int VanillaInventoryLastCoinSlot = 53;
		public const int VanillaInventoryLastAmmolot = 57;
		public const int VanillaInventorySelectedSlot = 58;


		////////////////

		public static ISet<int> AvailableInventorySlots( Player player ) {
			var myset = new HashSet<int>();

			for( int i=0; i<PlayerItemHelpers.VanillaInventoryLastMainSlot; i++ ) {
				if( player.inventory[i] != null && player.inventory[i].active && player.inventory[i].stack > 0 ) {
					myset.Add( i );
				}
			}

			return myset;
		}

		public static int RemoveInventoryItemQuantity( Player player, int item_type, int quantity ) {
			int removed = 0;

			for( int i = 0; i < player.inventory.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || item.type != item_type ) { continue; }

				int stack = item.stack;

				if( stack > quantity ) {
					item.stack -= quantity;
					removed += quantity;
					break;
				} else {
					quantity -= stack;
					removed += stack;
					player.inventory[i] = new Item();

					if( quantity == 0 ) { break; }
				}
			}

			return removed;
		}

		
		public static void DropInventoryItem( Player player, int slot ) {
			int _;
			PlayerItemHelpers.DropInventoryItem( player, slot, 100, out _ );
		}
		public static void DropInventoryItem( Player player, int slot, int no_grab_delay, out int idx ) {
			idx = PlayerItemHelpers.DropInventoryItem( player, slot, no_grab_delay );
		}

		public static int DropInventoryItem( Player player, int slot, int no_grab_delay ) {
			Item item = player.inventory[ slot ];
			if( item == null || item.IsAir ) {
				return -1;
			}

			int idx = Item.NewItem( (int)player.position.X, (int)player.position.Y, player.width, player.height, item.type, item.stack, false, -1, false, false );
			Item proto_new_item = Main.item[idx];

			item.position.X = proto_new_item.position.X;
			item.position.Y = proto_new_item.position.Y;

			Main.item[ idx ] = item;

			player.inventory[ slot ] = new Item();

			if( slot == PlayerItemHelpers.VanillaInventorySelectedSlot && player.whoAmI == Main.myPlayer ) {
				Main.mouseItem = new Item();
			}
			
			item.velocity.Y = (float)Main.rand.Next( -20, 1 ) * 0.2f;
			item.velocity.X = (float)Main.rand.Next( -20, 21 ) * 0.2f;
			item.noGrabDelay = no_grab_delay;
			item.favorited = false;
			item.newAndShiny = false;
			item.owner = player.whoAmI;

			if( Main.netMode != 0 && no_grab_delay > 0 ) {
				item.ownIgnore = player.whoAmI;
				item.ownTime = no_grab_delay;
			}

			Recipe.FindRecipes();

			if( Main.netMode == 1 ) {   // Client
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, idx, 0f/*1f*/, 0f, 0f, 0, 0, 0 );
				ItemNoGrabProtocol.SendToServer( idx, no_grab_delay );
			}

			return idx;
			//player.QuickSpawnClonedItem( player.inventory[slot], player.inventory[slot].stack );
		}

		public static void DropEquippedMiscItem( Player player, int slot ) {
			Item item = player.miscEquips[ slot ];

			if( item != null && !item.IsAir ) {
				int idx = Item.NewItem( player.position, item.width, item.height, item.type, item.stack, false, item.prefix, false, false );

				item.position = Main.item[ idx ].position;
				Main.item[ idx ] = item;

				if( Main.netMode == 1 ) {   // Client
					NetMessage.SendData( 21, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
				}

				player.miscEquips[ slot ] = new Item();
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
			if( player.selectedItem == PlayerItemHelpers.VanillaInventorySelectedSlot ) {
				PlayerItemHelpers.DropInventoryItem( player, PlayerItemHelpers.VanillaInventorySelectedSlot );
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


		////////////////

		public static bool IsPlayerNaked( Player player, bool also_vanity = false, bool can_hide = true ) {
			// Armor
			for( int i = 0; i < 3; i++ ) {
				if( !player.armor[0].IsAir ) { return false; }
			}

			int acc_range = 8 + player.extraAccessorySlots;

			// Accessory
			for( int i = 0; i < acc_range; i++ ) {
				if( !player.armor[i].IsAir && (!player.hideVisual[3] || (player.hideVisual[3] && !can_hide)) ) {
					return false;
				}
			}

			if( also_vanity ) {
				// Vanity armor/clothes
				for( int i = acc_range; i < acc_range + 3; i++ ) {
					if( !player.armor[i].IsAir ) { return false; }
				}
				// Vanity Accessory
				for( int i = acc_range + 3; i < 20; i++ ) {
					if( !player.armor[i].IsAir && (!player.hideVisual[i] || (player.hideVisual[i] && !can_hide)) ) {
						return false;
					}
				}
			}

			return true;
		}

		////////////////

		public static long CountMoney( Player player ) {
			bool _;
			long inv_count = Utils.CoinsCount( out _, player.inventory, new int[] { 58, 57, 56, 55, 54 } );
			long bank_count = Utils.CoinsCount( out _, player.bank.item, new int[0] );
			long bank2_count = Utils.CoinsCount( out _, player.bank2.item, new int[0] );
			long bank3_count = Utils.CoinsCount( out _, player.bank3.item, new int[0] );
			return Utils.CoinsCombineStacks( out _, new long[] { inv_count, bank_count, bank2_count, bank3_count } );
		}

		////////////////

		public static Vector2 TipOfHeldItem( Player player ) {
			Item item = player.HeldItem;
			if( item == null || item.IsAir ) { return Vector2.Zero; }

			Vector2 pos = player.RotatedRelativePoint( player.MountedCenter, true );

			int wid = Main.itemTexture[ item.type ].Width;
			int length = wid;
			
			if( item.useStyle != 5 ) {
				int hei = Main.itemTexture[ item.type ].Height;
				length = (int)Math.Sqrt( wid * wid + hei * hei );
			}

			float reach = ((float)length + 6f) * (float)player.direction;

			if( item.useStyle == 4 ) {
				return pos + new Vector2( reach/4f, -28f );
			}

			return pos + (player.itemRotation.ToRotationVector2() * reach);
		}

		////////////////

		public static Item GetGrappleItem( Player player ) {
			if( ItemAttributeHelpers.IsGrapple( player.miscEquips[4] ) ) {
				return player.miscEquips[4];
			}
			for( int i = 0; i < PlayerItemHelpers.VanillaInventorySize; i++ ) {
				if( Main.projHook[ player.inventory[i].shoot ] ) {
					return player.inventory[i];
				}
			}
			return null;
		}

		////////////////

		public static bool IsArmorSlot( int slot ) {
			return slot < 3;
		}

		public static bool IsAccessorySlot( Player player, int slot ) {
			return slot >= 3 && slot < 8 + player.extraAccessorySlots;
		}

		public static bool IsVanitySlot( Player player, int slot ) {
			return slot >= 8 + player.extraAccessorySlots;
		}
	}
}
