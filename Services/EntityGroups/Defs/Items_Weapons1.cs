using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemWeaponGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Weapon Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Ranged Weapon", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.ranged;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>( 
				"Any Magic Weapon", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.magic;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Melee Weapon", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.melee;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Thrown Weapon", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.thrown;
				} )
			) );

			// Melee Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Swingable", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.melee && item.useStyle == 1;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Thrustable", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.melee && item.useStyle == 5;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Flail", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.melee || item.useStyle != 5 ) { return false; }
					if( item.type == ItemID.Anchor ) { return true; }

					if( item.shoot == 0 ) { return false; }
					var projPool = ModHelpersMod.Instance.EntityGroups.GetProjPool();

					switch( projPool[ item.shoot ].aiStyle ) {
					case 15:    // Standard
					case 13:    // Chain Knife, Boxing Glove
					case 69:    // Flairon
					case 75:    // Solar Eruption
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Boomerang", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.melee || item.useStyle != 1 ) { return false; }
					if( item.type == ItemID.FlyingKnife ) { return true; }

					if( item.shoot == 0 ) { return false; }
					var projPool = ModHelpersMod.Instance.EntityGroups.GetProjPool();

					switch( projPool[ item.shoot ].aiStyle ) {
					case 3:    // Boomerangs
					case 15:    // Thorn Chakram
						return true;
					}
					return false;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Yoyo", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeHelpers.IsYoyo( item );
				} )
			) );

			// Magic Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Magic Staff Or Scepter Or Wand", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Staff" ) ||
						name.Contains( "Scepter" ) ||
						name.Contains( "Wand" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Magic Rod", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Rod" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Magic Gun", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					switch( item.type ) {
					case ItemID.LeafBlower:
						return true;
					}

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Gun" ) ||
						name.Contains( "Rifle" ) ||
						name.Contains( "Ray" ) ||
						name.Contains( "Cannon" ) ||
						name.Contains( "Laser" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Magic Tome", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					switch( item.type ) {
					case ItemID.DemonScythe:
					case ItemID.WaterBolt:
					case ItemID.LunarFlareBook:
					case ItemID.MagnetSphere:
					case ItemID.RazorbladeTyphoon:
						return true;
					}

					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpellTome }, 1 );
					if( has ) { return true; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Book" ) ||
						name.Contains( "Tome" );
				} )
			) );

			// Ranged Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Ranger Gun", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Bullet ||
						item.useAmmo == AmmoID.CandyCorn ||
						item.useAmmo == AmmoID.Coin;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Ranger Bow", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Arrow ||
						item.useAmmo == AmmoID.Stake;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Ranger Launcher", null,
				new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Rocket ||
						item.useAmmo == AmmoID.StyngerBolt ||
						item.useAmmo == AmmoID.JackOLantern ||
						item.useAmmo == AmmoID.NailFriendly;
				} )
			) );

			// Summon Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Minion Summon Item", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.summon && !item.sentry;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				"Any Sentry Summon Item", null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.summon && item.sentry;
				} )
			) );

			// Vanity Classes
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
					return !item.accessory;
				} )
			) );
		}
	}
}
