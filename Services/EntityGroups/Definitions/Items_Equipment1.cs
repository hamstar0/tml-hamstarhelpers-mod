using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class EntityGroupIDs {
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Weapon", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.damage > 0;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Tool", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeHelpers.IsTool( item );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanilla Explosive", null,
				new ItemGroupMatcher( ( item, grps ) => {
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
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Accessory", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.accessory && !item.vanity;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Armor", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeHelpers.IsArmor( item );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Garment", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Potion", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.potion;
				} )
			) );

			// Vanity Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>( "Any Vanity", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.vanity;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanity Accessory", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Vanity Garment", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} )
			) );
		}
	}
}
