using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyWeapon = "Any Weapon";
		/// <summary></summary>
		public const string AnyTool = "Any Tool";
		/// <summary></summary>
		public const string AnyVanillaExplosive = "Any Vanilla Explosive";
		/// <summary></summary>
		public const string AnyAccessory = "Any Accessory";
		/// <summary></summary>
		public const string AnyArmor = "Any Armor";
		/// <summary></summary>
		public const string AnyGarment = "Any Garment";
		/// <summary></summary>
		public const string AnyPotion = "Any Potion";
		/// <summary></summary>
		public const string AnyVanity = "Any Vanity";
		/// <summary></summary>
		public const string AnyVanityAccessory = "Any Vanity Accessory";
		/// <summary></summary>
		public const string AnyVanityGarment = "Any Vanity Garment";
	}




	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyWeapon, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.damage > 0;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyTool, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeHelpers.IsTool( item );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanillaExplosive, null,
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
				ItemGroupIDs.AnyAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.accessory && !item.vanity;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyArmor, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeHelpers.IsArmor( item );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyGarment, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyPotion, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.potion;
				} )
			) );

			// Vanity Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanity, null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.vanity;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanityAccessory, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				ItemGroupIDs.AnyVanityGarment, null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1;
				} )
			) );
		}
	}
}
