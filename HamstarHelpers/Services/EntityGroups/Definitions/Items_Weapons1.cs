using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.Items.Attributes;
using HamstarHelpers.Libraries.Recipes;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		//"Any Ranged Weapon", null,
		//"Any Magic Weapon", null,
		//"Any Melee Weapon", null,
		//"Any Thrown Weapon", null,
		//"Any Swingable", null,
		//"Any Thrustable", null,
		//"Any Flail", null,
		//"Any Boomerang", null,
		//"Any Yoyo", null,
		//"Any Magic Staff Or Scepter Or Wand", null,
		//"Any Magic Rod", null,
		//"Any Magic Gun", null,
		//"Any Magic Tome", null,
		//"Any Ranger Gun", null,
		//"Any Ranger Bow", null,
		//"Any Ranger Launcher", null,
		//"Any Minion Summon Item", null,
		//"Any Sentry Summon Item", null,
	}




	partial class EntityGroupDefs {
		internal static void DefineItemWeaponGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			// Weapon Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ranged Weapon",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.ranged;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Weapon",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.magic;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Melee Weapon",
				grpDeps: null,
				new ItemGroupMatcher( ( item, grps ) => {
					return item.melee;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Thrown Weapon",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.thrown;
				} )
			) );

			// Melee Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Swingable",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.melee && item.useStyle == ItemUseStyleID.SwingThrow;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Thrustable",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.melee && item.useStyle == ItemUseStyleID.HoldingOut;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Flail",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.melee || item.useStyle != ItemUseStyleID.HoldingOut ) { return false; }
					if( item.type == ItemID.Anchor ) { return true; }

					if( item.shoot == ProjectileID.None ) { return false; }
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
				grpName: "Any Boomerang",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.melee || item.useStyle != ItemUseStyleID.SwingThrow ) { return false; }
					if( item.type == ItemID.FlyingKnife ) { return true; }

					if( item.shoot == ProjectileID.None ) { return false; }
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
				grpName: "Any Yoyo",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return ItemAttributeLibraries.IsYoyo( item );
				} )
			) );

			// Magic Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Staff Or Scepter Or Wand",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemAttributeLibraries.GetQualifiedName( item );
					return name.Contains( "Staff" ) ||
						name.Contains( "Scepter" ) ||
						name.Contains( "Wand" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Rod",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemAttributeLibraries.GetQualifiedName( item );
					return name.Contains( "Rod" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Gun",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					switch( item.type ) {
					case ItemID.LeafBlower:
						return true;
					}

					string name = ItemAttributeLibraries.GetQualifiedName( item );
					return name.Contains( "Gun" ) ||
						name.Contains( "Rifle" ) ||
						name.Contains( "Ray" ) ||
						name.Contains( "Cannon" ) ||
						name.Contains( "Laser" );
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Magic Tome",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.magic ) { return false; }

					switch( item.type ) {
					case ItemID.DemonScythe:
					case ItemID.WaterBolt:
					case ItemID.LunarFlareBook:
					case ItemID.MagnetSphere:
					case ItemID.RazorbladeTyphoon:
						return true;
					}

					bool isCraftedWith = RecipeLibraries.ItemHasIngredients( item.type, new Dictionary<int, int>{ {ItemID.SpellTome, 1} } );
					if( isCraftedWith ) { return true; }

					string name = ItemAttributeLibraries.GetQualifiedName( item );
					return name.Contains( "Book" ) ||
						name.Contains( "Tome" );
				} )
			) );

			// Ranged Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ranger Gun",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Bullet ||
						item.useAmmo == AmmoID.CandyCorn ||
						item.useAmmo == AmmoID.Coin;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ranger Bow",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Arrow ||
						item.useAmmo == AmmoID.Stake;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Ranger Launcher",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Rocket ||
						item.useAmmo == AmmoID.StyngerBolt ||
						item.useAmmo == AmmoID.JackOLantern ||
						item.useAmmo == AmmoID.NailFriendly;
				} )
			) );

			// Summon Sub Classes

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Minion Summon Item",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.summon && !item.sentry;
				} )
			) );
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Sentry Summon Item",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.summon && item.sentry;
				} )
			) );
		}
	}
}
