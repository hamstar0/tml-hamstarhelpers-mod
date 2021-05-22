using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Libraries.Items {
	/// <summary>
	/// Assorted static "helper" functions pertaining to item identification.
	/// </summary>
	public partial class ItemIdentityLibraries {
		/// <summary>
		/// Gets a (human readable) unique key (as segments) from a given item type.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetUniqueKeySegs( int itemType ) {
			if( itemType < 0 || itemType >= ItemLoader.ItemCount ) {
				throw new ArgumentOutOfRangeException( "Invalid type: " + itemType );
			}
			if( itemType < ItemID.Count ) {
				return Tuple.Create( "Terraria", ItemID.Search.GetName( itemType ) );
			}

			var modItem = ItemLoader.GetItem( itemType );
			return Tuple.Create( modItem.mod.Name, modItem.Name );
		}


		////////////////

		/// <summary>
		/// Generates a hash value to uniquely identify a (vanilla) item instance by its field values.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="noContext">Omits `owner`, `netID`, and `favorited` fields.</param>
		/// <param name="minimal">Uses only the item's unique ID (`GetUniqueKey(...)`), `prefix`, and `stack`.</param>
		/// <returns></returns>
		public static int GetVanillaSnapshotHash( Item item, bool noContext, bool minimal ) {
			int pow = 1;
			int Pow() {
				pow *= 2;
				if( pow > 16777216 ) { pow = 1; }
				return pow;
			}

			//

			int hash = Entities.EntityLibraries.GetVanillaSnapshotHash( item, noContext );

			string id = ItemID.GetUniqueKey( item );

			hash += ( "id" + id ).GetHashCode() * Pow();
			hash += ( "prefix" + item.prefix ).GetHashCode() * Pow();
			hash += ( "stack" + item.stack ).GetHashCode() * Pow();

			if( !noContext ) {
				hash += ( "netID" + item.netID ).GetHashCode() * Pow();
				hash += ( "favorited" + item.favorited ).GetHashCode() * Pow();
			} else {
				Pow();
				Pow();
			}

			if( !minimal ) {
				hash += ( "mana" + item.mana ).GetHashCode() * Pow();
				//hash += ("buyOnce"+item.buyOnce).GetHashCode()+ Pow();	//?
				hash += ( "manaIncrease" + item.manaIncrease ).GetHashCode() * Pow();
				hash += ( "lifeRegen" + item.lifeRegen ).GetHashCode() * Pow();
				hash += ( "notAmmo" + item.notAmmo ).GetHashCode() * Pow();
				hash += ( "ammo" + item.ammo ).GetHashCode() * Pow();
				hash += ( "shootSpeed" + item.shootSpeed ).GetHashCode() * Pow();
				hash += ( "rare" + item.rare ).GetHashCode() * Pow();
				//hash += ( "noUseGraphic" + item.noUseGraphic ).GetHashCode()+ Pow();
				hash += ( "useAmmo" + item.useAmmo ).GetHashCode() * Pow();
				hash += ( "shieldSlot" + item.shieldSlot ).GetHashCode() * Pow();
				hash += ( "stringColor" + item.stringColor ).GetHashCode() * Pow();
				hash += ( "balloonSlot" + item.balloonSlot ).GetHashCode() * Pow();
				hash += ( "faceSlot" + item.faceSlot ).GetHashCode() * Pow();
				hash += ( "neckSlot" + item.neckSlot ).GetHashCode() * Pow();
				hash += ( "noMelee" + item.noMelee ).GetHashCode() * Pow();
				hash += ( "wingSlot" + item.wingSlot ).GetHashCode() * Pow();
				hash += ( "waistSlot" + item.waistSlot ).GetHashCode() * Pow();
				hash += ( "shoeSlot" + item.shoeSlot ).GetHashCode() * Pow();
				hash += ( "frontSlot" + item.frontSlot ).GetHashCode() * Pow();
				hash += ( "backSlot" + item.backSlot ).GetHashCode() * Pow();
				hash += ( "handOffSlot" + item.handOffSlot ).GetHashCode() * Pow();
				//hash += ("ToolTip"+item.ToolTip).GetHashCode() * Pow();
				//hash += ("release"+item.release).GetHashCode() * Pow();
				hash += ( "buffTime" + item.buffTime ).GetHashCode() * Pow();
				//hash += ("buy"+item.buy).GetHashCode() * Pow();	//?
				//hash += ("newAndShiny"+item.newAndShiny).GetHashCode() * Pow();	//?
				hash += ( "reuseDelay" + item.reuseDelay ).GetHashCode() * Pow();
				hash += ( "sentry" + item.sentry ).GetHashCode() * Pow();
				hash += ( "summon" + item.summon ).GetHashCode() * Pow();
				hash += ( "thrown" + item.thrown ).GetHashCode() * Pow();
				hash += ( "ranged" + item.ranged ).GetHashCode() * Pow();
				hash += ( "magic" + item.magic ).GetHashCode() * Pow();
				hash += ( "melee" + item.melee ).GetHashCode() * Pow();
				hash += ( "crit" + item.crit ).GetHashCode() * Pow();
				hash += ( "DD2Summon" + item.DD2Summon ).GetHashCode() * Pow();
				hash += ( "shopCustomPrice" + ( item.shopCustomPrice == null ? -1 : (int)item.shopCustomPrice ) ).GetHashCode() * Pow();
				hash += ( "shopSpecialCurrency" + item.shopSpecialCurrency ).GetHashCode() * Pow();
				hash += ( "uniqueStack" + item.uniqueStack ).GetHashCode() * Pow();
				hash += ( "cartTrack" + item.cartTrack ).GetHashCode() * Pow();
				hash += ( "mountType" + item.mountType ).GetHashCode() * Pow();
				hash += ( "handOnSlot" + item.handOnSlot ).GetHashCode() * Pow();
				hash += ( "buffType" + item.buffType ).GetHashCode() * Pow();
				hash += ( "noWet" + item.noWet ).GetHashCode() * Pow();
				hash += ( "material" + item.material ).GetHashCode() * Pow();
				hash += ( "vanity" + item.vanity ).GetHashCode() * Pow();
				hash += ( "social" + item.social ).GetHashCode() * Pow();
				hash += ( "value" + item.value ).GetHashCode() * Pow();
				hash += ( "legSlot" + item.legSlot ).GetHashCode() * Pow();
				hash += ( "shoot" + item.shoot ).GetHashCode() * Pow();
				hash += ( "headSlot" + item.headSlot ).GetHashCode() * Pow();
				hash += ( "holdStyle" + item.holdStyle ).GetHashCode() * Pow();
				//////hash += ("type"+item.type).GetHashCode() * Pow();
				//hash += ("keepTime"+item.keepTime).GetHashCode() * Pow();
				//hash += ("ownTime"+item.ownTime).GetHashCode() * Pow();
				//hash += ("ownIgnore"+item.ownIgnore).GetHashCode() * Pow();
				hash += ( "instanced" + item.instanced ).GetHashCode() * Pow();
				hash += ( "paint" + item.paint ).GetHashCode() * Pow();
				hash += ( "hairDye" + item.hairDye ).GetHashCode() * Pow();
				hash += ( "expert" + item.expert ).GetHashCode() * Pow();
				hash += ( "expertOnly" + item.expertOnly ).GetHashCode() * Pow();
				hash += ( "useStyle" + item.useStyle ).GetHashCode() * Pow();
				hash += ( "makeNPC" + item.makeNPC ).GetHashCode() * Pow();
				hash += ( "fishingPole" + item.fishingPole ).GetHashCode() * Pow();
				hash += ( "dye" + item.dye ).GetHashCode() * Pow();
				hash += ( "wornArmor" + item.wornArmor ).GetHashCode() * Pow();
				hash += ( "tileWand" + item.tileWand ).GetHashCode() * Pow();
				hash += ( "spawnTime" + item.spawnTime ).GetHashCode() * Pow();
				//hash += ("isBeingGrabbed"+item.isBeingGrabbed).GetHashCode()* Pow();
				//hash += ("beingGrabbed"+item.beingGrabbed).GetHashCode()* Pow();
				//hash += ("noGrabDelay"+item.noGrabDelay).GetHashCode()* Pow();
				hash += ( "bodySlot" + item.bodySlot ).GetHashCode() * Pow();
				hash += ( "flame" + item.flame ).GetHashCode() * Pow();
				hash += ( "questItem" + item.questItem ).GetHashCode() * Pow();
				hash += ( "bait" + item.bait ).GetHashCode() * Pow();
				hash += ( "channel" + item.channel ).GetHashCode() * Pow();
				hash += ( "mech" + item.mech ).GetHashCode() * Pow();
				hash += ( "useAnimation" + item.useAnimation ).GetHashCode() * Pow();
				hash += ( "defense" + item.defense ).GetHashCode() * Pow();
				//hash += ("UseSound"+item.UseSound).GetHashCode()* Pow();
				hash += ( "scale" + item.scale ).GetHashCode() * Pow();
				hash += ( "glowMask" + item.glowMask ).GetHashCode() * Pow();
				hash += ( "alpha" + item.alpha ).GetHashCode() * Pow();
				hash += ( "color" + item.color ).GetHashCode() * Pow();
				hash += ( "useTurn" + item.useTurn ).GetHashCode() * Pow();
				hash += ( "autoReuse" + item.autoReuse ).GetHashCode() * Pow();
				hash += ( "consumable" + item.consumable ).GetHashCode() * Pow();
				hash += ( "potion" + item.potion ).GetHashCode() * Pow();
				hash += ( "healMana" + item.healMana ).GetHashCode() * Pow();
				hash += ( "knockBack" + item.knockBack ).GetHashCode() * Pow();
				hash += ( "healLife" + item.healLife ).GetHashCode() * Pow();
				hash += ( "placeStyle" + item.placeStyle ).GetHashCode() * Pow();
				hash += ( "createWall" + item.createWall ).GetHashCode() * Pow();
				hash += ( "createTile" + item.createTile ).GetHashCode() * Pow();
				hash += ( "tileBoost" + item.tileBoost ).GetHashCode() * Pow();
				hash += ( "hammer" + item.hammer ).GetHashCode() * Pow();
				hash += ( "axe" + item.axe ).GetHashCode() * Pow();
				hash += ( "pick" + item.pick ).GetHashCode() * Pow();
				hash += ( "maxStack" + item.maxStack ).GetHashCode() * Pow();
				hash += ( "useTime" + item.useTime ).GetHashCode() * Pow();
				hash += ( "damage" + item.damage ).GetHashCode() * Pow();
				hash += ( "accessory" + item.accessory ).GetHashCode() * Pow();
			}

			return hash;
		}
	}
}
