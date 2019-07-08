using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to item identification.
	/// </summary>
	public partial class ItemIdentityHelpers {
		/// <summary>
		/// Gets the identifier of an item. For Terraria items, this is `Terraria IronPickaxe`, with the portion after "Terraria"
		/// being the item's field name in `ItemID`. For modded items, the format is ItemModName ModdedItemInternalName; the mod
		/// name first, and the modded item's internal `Name` after.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static string GetUniqueId( int itemType ) {
			if( ItemID.Search.ContainsId(itemType) ) {
				return "Terraria " + ItemID.Search.GetName( itemType );
			} else {
				var item = new Item();
				item.SetDefaults( itemType, true );

				if( item.modItem != null ) {
					return item.modItem.mod.Name + " " + item.modItem.Name;
				}
			}

			return ""+itemType;
		}

		/// <summary>
		/// Gets the identifier of an item. For Terraria items, this is `Terraria IronPickaxe`, with the portion after "Terraria"
		/// being the item's field name in `ItemID`. For modded items, the format is ItemModName ModdedItemInternalName; the mod
		/// name first, and the modded item's internal `Name` after.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetUniqueId( Item item ) {
			if( item.modItem == null ) {
				return "Terraria " + ItemID.Search.GetName( item.type );
			} else {
				return item.modItem.mod.Name + " " + item.modItem.Name;
			}
		}

		////

		/// <summary>
		/// Attempts to get an item's type (id) by a given identifier (see `GetUniqueId(...)`).
		/// </summary>
		/// <param name="itemUid"></param>
		/// <param name="itemId">Outputs as the item's `type`.</param>
		/// <returns>`true` if itemUid identifies as a value item proper name.</returns>
		public static bool TryGetTypeByUid( string itemUid, out int itemId ) {
			string[] split = itemUid.Split( '.' );

			if( split[0] == "Terraria" ) {
				return Int32.TryParse( split[1], out itemId );
			} else {
				Mod mod = ModLoader.GetMod( split[0] );
				if( mod == null ) {
					itemId = -1;
					return false;
				}
				itemId = mod.ItemType( split[1] );
				return true;
			}
		}


		////////////////

		/*public static string GetUniqueId( Item item ) {
			if( item.modItem != null ) {
				return item.modItem.mod.Name + " " + item.Name;
			}
			return ""+ item.netID;
		}*/

		/// <summary>
		/// Gets an item's qualified (human readable) name.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetQualifiedName( Item item ) {
			return Lang.GetItemNameValue( item.type );  // not netID?
		}

		/// <summary>
		/// Gets an item's qualified (human readable) name.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static string GetQualifiedName( int itemType ) {
			return Lang.GetItemNameValue( itemType );
		}


		////////////////

		/// <summary>
		/// Generates a hash value to uniquely identify a (vanilla) item instance by its field values.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="noContext">Omits `owner`, `netID`, and `favorited` fields.</param>
		/// <param name="minimal">Uses only the item's unique ID (`GetUniqueId(...)`), `prefix`, and `stack`.</param>
		/// <returns></returns>
		public static int GetVanillaSnapshotHash( Item item, bool noContext, bool minimal ) {
			int hash = Entities.EntityHelpers.GetVanillaSnapshotHash( item, noContext );

			string id = ItemIdentityHelpers.GetUniqueId( item );  // used to be GetUniqueId

			hash ^= ( "id" + id ).GetHashCode();
			hash ^= ( "prefix" + item.prefix ).GetHashCode();
			hash ^= ( "stack" + item.stack ).GetHashCode();

			if( !noContext ) {
				hash ^= ( "owner" + item.owner ).GetHashCode();
				hash ^= ( "netID" + item.netID ).GetHashCode();
				hash ^= ( "favorited" + item.favorited ).GetHashCode();
			}

			if( !minimal ) {
				hash ^= ( "mana" + item.mana ).GetHashCode();
				//hash ^= ("buyOnce"+item.buyOnce).GetHashCode();	//?
				hash ^= ( "manaIncrease" + item.manaIncrease ).GetHashCode();
				hash ^= ( "lifeRegen" + item.lifeRegen ).GetHashCode();
				hash ^= ( "notAmmo" + item.notAmmo ).GetHashCode();
				hash ^= ( "ammo" + item.ammo ).GetHashCode();
				hash ^= ( "shootSpeed" + item.shootSpeed ).GetHashCode();
				hash ^= ( "rare" + item.rare ).GetHashCode();
				//hash ^= ( "noUseGraphic" + item.noUseGraphic ).GetHashCode();
				hash ^= ( "useAmmo" + item.useAmmo ).GetHashCode();
				hash ^= ( "shieldSlot" + item.shieldSlot ).GetHashCode();
				hash ^= ( "stringColor" + item.stringColor ).GetHashCode();
				hash ^= ( "balloonSlot" + item.balloonSlot ).GetHashCode();
				hash ^= ( "faceSlot" + item.faceSlot ).GetHashCode();
				hash ^= ( "neckSlot" + item.neckSlot ).GetHashCode();
				hash ^= ( "noMelee" + item.noMelee ).GetHashCode();
				hash ^= ( "wingSlot" + item.wingSlot ).GetHashCode();
				hash ^= ( "waistSlot" + item.waistSlot ).GetHashCode();
				hash ^= ( "shoeSlot" + item.shoeSlot ).GetHashCode();
				hash ^= ( "frontSlot" + item.frontSlot ).GetHashCode();
				hash ^= ( "backSlot" + item.backSlot ).GetHashCode();
				hash ^= ( "handOffSlot" + item.handOffSlot ).GetHashCode();
				//hash ^= ("ToolTip"+item.ToolTip).GetHashCode();
				//hash ^= ("release"+item.release).GetHashCode();
				hash ^= ( "buffTime" + item.buffTime ).GetHashCode();
				//hash ^= ("buy"+item.buy).GetHashCode();	//?
				//hash ^= ("newAndShiny"+item.newAndShiny).GetHashCode();	//?
				hash ^= ( "reuseDelay" + item.reuseDelay ).GetHashCode();
				hash ^= ( "sentry" + item.sentry ).GetHashCode();
				hash ^= ( "summon" + item.summon ).GetHashCode();
				hash ^= ( "thrown" + item.thrown ).GetHashCode();
				hash ^= ( "ranged" + item.ranged ).GetHashCode();
				hash ^= ( "magic" + item.magic ).GetHashCode();
				hash ^= ( "melee" + item.melee ).GetHashCode();
				hash ^= ( "crit" + item.crit ).GetHashCode();
				hash ^= ( "DD2Summon" + item.DD2Summon ).GetHashCode();
				hash ^= ( "shopCustomPrice" + ( item.shopCustomPrice == null ? -1 : (int)item.shopCustomPrice ) ).GetHashCode();
				hash ^= ( "shopSpecialCurrency" + item.shopSpecialCurrency ).GetHashCode();
				hash ^= ( "uniqueStack" + item.uniqueStack ).GetHashCode();
				hash ^= ( "cartTrack" + item.cartTrack ).GetHashCode();
				hash ^= ( "mountType" + item.mountType ).GetHashCode();
				hash ^= ( "handOnSlot" + item.handOnSlot ).GetHashCode();
				hash ^= ( "buffType" + item.buffType ).GetHashCode();
				hash ^= ( "noWet" + item.noWet ).GetHashCode();
				hash ^= ( "material" + item.material ).GetHashCode();
				hash ^= ( "vanity" + item.vanity ).GetHashCode();
				hash ^= ( "social" + item.social ).GetHashCode();
				hash ^= ( "value" + item.value ).GetHashCode();
				hash ^= ( "legSlot" + item.legSlot ).GetHashCode();
				hash ^= ( "shoot" + item.shoot ).GetHashCode();
				hash ^= ( "headSlot" + item.headSlot ).GetHashCode();
				hash ^= ( "holdStyle" + item.holdStyle ).GetHashCode();
				//////hash ^= ("type"+item.type).GetHashCode();
				//hash ^= ("keepTime"+item.keepTime).GetHashCode();
				//hash ^= ("ownTime"+item.ownTime).GetHashCode();
				//hash ^= ("ownIgnore"+item.ownIgnore).GetHashCode();
				hash ^= ( "instanced" + item.instanced ).GetHashCode();
				hash ^= ( "paint" + item.paint ).GetHashCode();
				hash ^= ( "hairDye" + item.hairDye ).GetHashCode();
				hash ^= ( "expert" + item.expert ).GetHashCode();
				hash ^= ( "expertOnly" + item.expertOnly ).GetHashCode();
				hash ^= ( "useStyle" + item.useStyle ).GetHashCode();
				hash ^= ( "makeNPC" + item.makeNPC ).GetHashCode();
				hash ^= ( "fishingPole" + item.fishingPole ).GetHashCode();
				hash ^= ( "dye" + item.dye ).GetHashCode();
				hash ^= ( "wornArmor" + item.wornArmor ).GetHashCode();
				hash ^= ( "tileWand" + item.tileWand ).GetHashCode();
				hash ^= ( "spawnTime" + item.spawnTime ).GetHashCode();
				//hash ^= ("isBeingGrabbed"+item.isBeingGrabbed).GetHashCode();
				//hash ^= ("beingGrabbed"+item.beingGrabbed).GetHashCode();
				//hash ^= ("noGrabDelay"+item.noGrabDelay).GetHashCode();
				hash ^= ( "bodySlot" + item.bodySlot ).GetHashCode();
				hash ^= ( "flame" + item.flame ).GetHashCode();
				hash ^= ( "questItem" + item.questItem ).GetHashCode();
				hash ^= ( "bait" + item.bait ).GetHashCode();
				hash ^= ( "channel" + item.channel ).GetHashCode();
				hash ^= ( "mech" + item.mech ).GetHashCode();
				hash ^= ( "useAnimation" + item.useAnimation ).GetHashCode();
				hash ^= ( "defense" + item.defense ).GetHashCode();
				//hash ^= ("UseSound"+item.UseSound).GetHashCode();
				hash ^= ( "scale" + item.scale ).GetHashCode();
				hash ^= ( "glowMask" + item.glowMask ).GetHashCode();
				hash ^= ( "alpha" + item.alpha ).GetHashCode();
				hash ^= ( "color" + item.color ).GetHashCode();
				hash ^= ( "useTurn" + item.useTurn ).GetHashCode();
				hash ^= ( "autoReuse" + item.autoReuse ).GetHashCode();
				hash ^= ( "consumable" + item.consumable ).GetHashCode();
				hash ^= ( "potion" + item.potion ).GetHashCode();
				hash ^= ( "healMana" + item.healMana ).GetHashCode();
				hash ^= ( "knockBack" + item.knockBack ).GetHashCode();
				hash ^= ( "healLife" + item.healLife ).GetHashCode();
				hash ^= ( "placeStyle" + item.placeStyle ).GetHashCode();
				hash ^= ( "createWall" + item.createWall ).GetHashCode();
				hash ^= ( "createTile" + item.createTile ).GetHashCode();
				hash ^= ( "tileBoost" + item.tileBoost ).GetHashCode();
				hash ^= ( "hammer" + item.hammer ).GetHashCode();
				hash ^= ( "axe" + item.axe ).GetHashCode();
				hash ^= ( "pick" + item.pick ).GetHashCode();
				hash ^= ( "maxStack" + item.maxStack ).GetHashCode();
				hash ^= ( "useTime" + item.useTime ).GetHashCode();
				hash ^= ( "damage" + item.damage ).GetHashCode();
				hash ^= ( "accessory" + item.accessory ).GetHashCode();
			}

			return hash;
		}


		////////////////

		/// <summary>
		/// Gets an item's type by it's internal name (ItemID static field name or `ModItem.Name`), and it's origin mod (if any).
		/// </summary>
		/// <param name="name">Item's internal name: For vanilla, ItemID static field name. For mods, `ModItem.Name`.</param>
		/// <param name="mod"></param>
		/// <returns></returns>
		public static int GetTypeByName( string name, Mod mod=null ) {
			if( mod == null ) {
				if( !ItemID.Search.ContainsName( name ) )
					return 0;
				return ItemID.Search.GetId( name );
			}

			ModItem moditem = mod.GetItem( name );
			return moditem == null ? 0 : moditem.item.type;
		}
	}
}
