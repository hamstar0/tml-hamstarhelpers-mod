using HamstarHelpers.Helpers.DotNetHelpers.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		public static string GetUniqueId( Item item ) {
			if( item.modItem != null ) {
				return item.modItem.mod.Name + " " + item.Name;
			}
			return ""+ item.netID;
		}


		public static int GetVanillaSnapshotHash( Item item, bool no_context, bool minimal ) {
			int hash = EntityHelpers.EntityHelpers.GetVanillaSnapshotHash( item, no_context );

			string id = ItemIdentityHelpers.GetUniqueId( item );
			
			hash ^= ( "id" + id ).GetHashCode();
			hash ^= ( "prefix" + item.prefix ).GetHashCode();
			hash ^= ( "stack" + item.stack ).GetHashCode();

			if( !no_context ) {
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


		public static int GetTypeByName( string name, Mod mod=null ) {
			if( mod == null ) {
				if( !ItemID.Search.ContainsName( name ) )
					return 0;
				return ItemID.Search.GetId( name );
			}

			ModItem moditem = mod.GetItem( name );
			return moditem == null ? 0 : moditem.item.type;
		}


		////////////////

		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get {
				return HamstarHelpersMod.Instance.ItemIdentityHelpers._NamesToIds;
			}
		}
		
		private ReadOnlyDictionaryOfSets<string, int> _NamesToIds = null;


		////////////////

		internal void PopulateNames() {
			var dict = new Dictionary<string, ISet<int>>();

			for( int i = 1; i < ItemLoader.ItemCount; i++ ) {
				string name = Lang.GetItemNameValue( i );

				if( dict.ContainsKey( name ) ) {
					dict[name].Add( i );
				} else {
					dict[name] = new HashSet<int>() { i };
				}
			}

			this._NamesToIds = new ReadOnlyDictionaryOfSets<string, int>( dict );
		}
	}
}
