using HamstarHelpers.Helpers.Items;
using System;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups1( Action<string, string[], Matcher> addDef ) {
			addDef( "Any Weapon", null,
				( item, grps ) => {
					return item.damage > 0;
				} );
			addDef( "Any Tool", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsTool( item );
				} );
			addDef( "Any Vanilla Explosive", null,
				( item, grps ) => {
					switch( item.type ) {
					case ItemID.Bomb:
					case ItemID.StickyBomb:
					case ItemID.BouncyBomb:
					case ItemID.Dynamite:
					case ItemID.StickyDynamite:
					case ItemID.BouncyDynamite:
					case ItemID.Grenade:
					case ItemID.StickyGrenade:
					case ItemID.BouncyGrenade:
					case ItemID.BombFish:
					case ItemID.PartyGirlGrenade:
					case ItemID.Explosives:	//?
					case ItemID.LandMine:   //?
					case ItemID.RocketI:
					case ItemID.RocketII:
					case ItemID.RocketIII:
					case ItemID.RocketIV:
					case ItemID.StyngerBolt:
					case ItemID.HellfireArrow:
					case ItemID.ExplosiveJackOLantern:
					case ItemID.ExplosiveBunny:
					case ItemID.Cannonball:
					case ItemID.Beenade:	//?
						return true;
					}
					return false;
				} );

			addDef( "Any Accessory", null,
				( item, grps ) => {
					return item.accessory && !item.vanity;
				} );
			addDef( "Any Armor", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsArmor( item );
				} );
			addDef( "Any Garment", null,
				( item, grps ) => {
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} );
			addDef( "Any Potion", null,
				( item, grps ) => {
					return item.potion;
				} );

			// Vanity Classes

			addDef( "Any Vanity", null,
				( item, grps ) => {
					return item.vanity;
				} );
			addDef( "Any Vanity Accessory", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} );
			addDef( "Any Vanity Garment", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} );
		}
	}
}
