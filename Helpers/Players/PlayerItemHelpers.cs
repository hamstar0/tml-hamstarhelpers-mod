using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Internals.NetProtocols;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Players {
	/// <summary>
	/// Assorted static "helper" functions pertaining to player inventory or equips.
	/// </summary>
	public static partial class PlayerItemHelpers {
		/// <summary></summary>
		public const int VanillaInventorySize = 58;
		/// <summary></summary>
		public const int VanillaInventoryHotbarSize = 10;
		/// <summary></summary>
		public const int VanillaInventoryMainSize = 40;
		/// <summary></summary>
		public const int VanillaInventoryLastMainSlot = 49;
		/// <summary></summary>
		public const int VanillaInventoryLastCoinSlot = 53;
		/// <summary></summary>
		public const int VanillaInventoryLastAmmolot = 57;
		/// <summary></summary>
		public const int VanillaInventorySelectedSlot = 58;

		/// <summary></summary>
		public const int VanillaAccessorySlotFirst = 3;



		////////////////

		/// <summary>
		/// Removes a quantity of a given item type from the player's inventory.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="itemType"></param>
		/// <param name="quantity"></param>
		/// <returns>Amount removed.</returns>
		public static int RemoveInventoryItemQuantity( Player player, int itemType, int quantity ) {
			int removed = 0;

			for( int i = 0; i < player.inventory.Length; i++ ) {
				Item item = player.inventory[i];
				if( item == null || item.IsAir || item.type != itemType ) { continue; }

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

		////
		
		/// <summary>
		/// Drops a given inventory item to the ground.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="slot"></param>
		/// <param name="noGrabDelay"></param>
		/// <returns>World item index (in `Main.item` array). -1 if none.</returns>
		public static int DropInventoryItem( Player player, int slot, int noGrabDelay=100 ) {
			Item item = player.inventory[ slot ];
			if( item == null || item.IsAir ) {
				return -1;
			}

			int idx = Item.NewItem( (int)player.position.X, (int)player.position.Y, player.width, player.height, item.type, item.stack, false, -1, false, false );
			Item protoNewItem = Main.item[idx];

			item.position.X = protoNewItem.position.X;
			item.position.Y = protoNewItem.position.Y;

			Main.item[ idx ] = item;

			player.inventory[ slot ] = new Item();

			if( slot == PlayerItemHelpers.VanillaInventorySelectedSlot && player.whoAmI == Main.myPlayer ) {
				Main.mouseItem = new Item();
			}

			item.velocity.Y = (float)Main.rand.Next( -20, 1 ) * 0.2f;
			item.velocity.X = (float)Main.rand.Next( -20, 21 ) * 0.2f;
			item.noGrabDelay = noGrabDelay;
			item.favorited = false;
			item.newAndShiny = false;
			item.owner = player.whoAmI;

			if( Main.netMode != 0 && noGrabDelay > 0 ) {
				item.ownIgnore = player.whoAmI;
				item.ownTime = noGrabDelay;
			}

			Recipe.FindRecipes();

			if( Main.netMode == 1 ) {   // Client
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, idx, 0f/*1f*/, 0f, 0f, 0, 0, 0 );
				ItemNoGrabProtocol.SendToServer( idx, noGrabDelay );
			}

			return idx;
			//player.QuickSpawnClonedItem( player.inventory[slot], player.inventory[slot].stack );
		}

		/// <summary>
		/// Drops a given armor item to the ground.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="slot"></param>
		/// <param name="noGrabDelay"></param>
		public static void DropEquippedArmorItem( Player player, int slot, int noGrabDelay = 100 ) {
			Item item = player.armor[slot];

			if( item != null && !item.IsAir ) {
				int itemIdx = Item.NewItem( player.position, item.width, item.height, item.type, item.stack, false, item.prefix, false, false );

				item.position = Main.item[itemIdx].position;
				item.noGrabDelay = noGrabDelay;
				Main.item[itemIdx] = item;

				if( Main.netMode == 1 ) {   // Client
					NetMessage.SendData( MessageID.SyncItem, -1, -1, null, itemIdx, 1f, 0f, 0f, 0, 0, 0 );
				}

				player.armor[slot] = new Item();
			}
		}

		/// <summary>
		/// Drops a given misc item to the ground.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="slot"></param>
		/// <param name="noGrabDelay"></param>
		public static void DropEquippedMiscItem( Player player, int slot, int noGrabDelay = 100 ) {
			Item item = player.miscEquips[ slot ];

			if( item != null && !item.IsAir ) {
				int idx = Item.NewItem( player.position, item.width, item.height, item.type, item.stack, false, item.prefix, false, false );

				item.position = Main.item[ idx ].position;
				item.noGrabDelay = noGrabDelay;
				Main.item[ idx ] = item;

				if( Main.netMode == 1 ) {   // Client
					NetMessage.SendData( MessageID.SyncItem, -1, -1, null, idx, 1f, 0f, 0f, 0, 0, 0 );
				}

				player.miscEquips[ slot ] = new Item();
			}
		}

		////

		/// <summary>
		/// Drops player's currently equipped item.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool UnhandItem( Player player ) {
			bool isUnhanded = false;

			// Drop mouse item always
			if( player.selectedItem == PlayerItemHelpers.VanillaInventorySelectedSlot ) {
				PlayerItemHelpers.DropInventoryItem( player, PlayerItemHelpers.VanillaInventorySelectedSlot );
				isUnhanded = true;
			}
			// Preferably select a blank slot
			if( !isUnhanded ) {
				for( int i = 0; i < player.inventory.Length; i++ ) {
					if( player.inventory[i] == null || player.inventory[i].IsAir ) {
						player.selectedItem = i;
						isUnhanded = true;
						break;
					}
				}
			}
			// Otherwise select a non-usable item
			if( !isUnhanded ) {
				for( int i = 0; i < player.inventory.Length; i++ ) {
					Item item = player.inventory[i];
					if( item != null && item.holdStyle == 0 && item.createTile == -1 && !item.potion && item.useStyle == 0 ) {
						player.selectedItem = i;
						isUnhanded = true;
						break;
					}
				}
			}
			// Otherwise select a non-held item
			if( !isUnhanded ) {
				for( int i = 12; i < player.inventory.Length; i++ ) {
					Item item = player.inventory[i];
					if( item != null && item.holdStyle == 0 ) {
						player.selectedItem = i;
						isUnhanded = true;
						break;
					}
				}
			}

			player.noItems = true;

			return isUnhanded;
		}


		////////////////

		/// <summary>
		/// Indicates if player is not wearing any items.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="alsoVanity"></param>
		/// <param name="canHide">Indicates items can be hidden to not count against being "naked".</param>
		/// <returns></returns>
		public static bool IsPlayerNaked( Player player, bool alsoVanity = false, bool canHide = true ) {
			// Armor
			for( int i = 0; i < 3; i++ ) {
				if( !player.armor[0].IsAir ) { return false; }
			}

			int accRange = 8 + player.extraAccessorySlots;

			// Accessory
			for( int i = 0; i < accRange; i++ ) {
				if( !player.armor[i].IsAir && ( !player.hideVisual[3] || ( player.hideVisual[3] && !canHide ) ) ) {
					return false;
				}
			}

			if( alsoVanity ) {
				// Vanity armor/clothes
				for( int i = accRange; i < accRange + 3; i++ ) {
					if( !player.armor[i].IsAir ) { return false; }
				}
				// Vanity Accessory
				for( int i = accRange + 3; i < 20; i++ ) {
					if( !player.armor[i].IsAir && ( !player.hideVisual[i] || ( player.hideVisual[i] && !canHide ) ) ) {
						return false;
					}
				}
			}

			return true;
		}


		////////////////

		/// <summary>
		/// Totals up player's money.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="includeBanks"></param>
		/// <returns></returns>
		public static long CountMoney( Player player, bool includeBanks ) {
			bool _;
			long invCount = Utils.CoinsCount( out _, player.inventory, new int[] { 58, 57, 56, 55, 54 } );
			long bankCount = 0, bank2Count = 0, bank3Count = 0;

			if( includeBanks ) {
				bankCount = Utils.CoinsCount( out _, player.bank.item, new int[0] );
				bank2Count = Utils.CoinsCount( out _, player.bank2.item, new int[0] );
				bank3Count = Utils.CoinsCount( out _, player.bank3.item, new int[0] );
			}
			return Utils.CoinsCombineStacks( out _, new long[] { invCount, bankCount, bank2Count, bank3Count } );
		}


		////////////////

		/// <summary>
		/// Gets world position of the tip of a player's weilded item.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Gets the player's grappling item.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static Item GetGrappleItem( Player player ) {
			if( ItemAttributeHelpers.IsGrapple( player.miscEquips[4] ) ) {
				return player.miscEquips[4];
			}
			for( int i = 0; i < PlayerItemHelpers.VanillaInventorySize; i++ ) {
				if( Main.projHook[player.inventory[i].shoot] ) {
					return player.inventory[i];
				}
			}
			return null;
		}


		////////////////

		/// <summary>
		/// Indicates if a given slot position is an armor slot (in `player.armor`).
		/// </summary>
		/// <param name="slot"></param>
		/// <returns></returns>
		public static bool IsArmorSlot( int slot ) {
			return slot < PlayerItemHelpers.VanillaAccessorySlotFirst;
		}

		/// <summary>
		/// Indicates if a given slot position is an accessory slot (in `player.armor`).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="slot"></param>
		/// <returns></returns>
		public static bool IsAccessorySlot( Player player, int slot ) {
			return slot >= PlayerItemHelpers.VanillaAccessorySlotFirst && slot < 8 + player.extraAccessorySlots;
		}

		/// <summary>
		/// Indicates if a given slot position is a vanity slot (in `player.armor`).
		/// </summary>
		/// <param name="player"></param>
		/// <param name="slot"></param>
		/// <returns></returns>
		public static bool IsVanitySlot( Player player, int slot ) {
			return slot >= 8 + player.extraAccessorySlots;
		}


		////////////////

		/// <summary>
		/// Accesses items of a player's currently opened chest.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="isPersonalChest"></param>
		/// <returns></returns>
		public static Item[] GetCurrentlyOpenChest( Player player, out bool? isPersonalChest ) {
			if( player.chest != -1 ) {
				if( player.chest >= 0 ) {
					isPersonalChest = false;
					return Main.chest[player.chest].item;
				} else if( player.chest == -2 ) {
					isPersonalChest = true;
					return player.bank.item;
				} else if( player.chest == -3 ) {
					isPersonalChest = true;
					return player.bank2.item;
				} else if( player.chest == -4 ) {
					isPersonalChest = true;
					return player.bank3.item;
				}
			}

			isPersonalChest = null;
			return null;
		}
	}
}
