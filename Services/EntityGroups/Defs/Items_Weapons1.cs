using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemWeaponGroups1( Action<string, string[], Matcher> addDef ) {
			// Weapon Classes

			addDef( "Any Ranged Weapon", null,
				( item, grps ) => {
					return item.ranged;
				} );
			addDef( "Any Magic Weapon", null,
				( item, grps ) => {
					return item.magic;
				} );
			addDef( "Any Melee Weapon", null,
				( item, grps ) => {
					return item.melee;
				} );
			addDef( "Any Thrown Weapon", null,
				( item, grps ) => {
					return item.thrown;
				} );

			// Melee Sub Classes

			addDef( "Any Swingable", null,
				( item, grps ) => {
					return item.melee && item.useStyle == 1;
				} );
			addDef( "Any Thrustable", null,
				( item, grps ) => {
					return item.melee && item.useStyle == 5;
				} );
			addDef( "Any Flail", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Boomerang", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Yoyo", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsYoyo( item );
				} );

			// Magic Sub Classes

			addDef( "Any Magic Staff Or Scepter Or Wand", null,
				( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Staff" ) ||
						name.Contains( "Scepter" ) ||
						name.Contains( "Wand" );
				} );
			addDef( "Any Magic Rod", null,
				( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Rod" );
				} );
			addDef( "Any Magic Gun", null,
				( item, grps ) => {
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
				} );
			addDef( "Any Magic Tome", null,
				( item, grps ) => {
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
				} );

			// Ranged Sub Classes

			addDef( "Any Ranger Gun", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Bullet ||
						item.useAmmo == AmmoID.CandyCorn ||
						item.useAmmo == AmmoID.Coin;
				} );
			addDef( "Any Ranger Bow", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Arrow ||
						item.useAmmo == AmmoID.Stake;
				} );
			addDef( "Any Ranger Launcher", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Rocket ||
						item.useAmmo == AmmoID.StyngerBolt ||
						item.useAmmo == AmmoID.JackOLantern ||
						item.useAmmo == AmmoID.NailFriendly;
				} );

			// Summon Sub Classes

			addDef( "Any Minion Summon Item", null,
				( item, grps ) => {
					return item.summon && !item.sentry;
				} );
			addDef( "Any Sentry Summon Item", null,
				( item, grps ) => {
					return item.summon && item.sentry;
				} );
			
			// Vanity Classes
			addDef( "Any Vanity Accessory", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} );
			addDef( "Any Vanity Garment", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return !item.accessory;
				} );
		}
	}
}
