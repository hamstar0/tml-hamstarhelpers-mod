using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemWeaponGroups1( Action<string, string[], Matcher> add_def ) {
			// Weapon Classes

			add_def( "Any Ranged Weapon", null,
				( item, grps ) => {
					return item.ranged;
				} );
			add_def( "Any Magic Weapon", null,
				( item, grps ) => {
					return item.magic;
				} );
			add_def( "Any Melee Weapon", null,
				( item, grps ) => {
					return item.melee;
				} );
			add_def( "Any Thrown Weapon", null,
				( item, grps ) => {
					return item.thrown;
				} );

			// Melee Sub Classes

			add_def( "Any Swingable", null,
				( item, grps ) => {
					return item.melee && item.useStyle == 1;
				} );
			add_def( "Any Thrustable", null,
				( item, grps ) => {
					return item.melee && item.useStyle == 5;
				} );
			add_def( "Any Flail", null,
				( item, grps ) => {
					if( !item.melee || item.useStyle != 5 ) { return false; }
					if( item.type == ItemID.Anchor ) { return true; }

					if( item.shoot == 0 ) { return false; }
					var proj_pool = this.GetProjPool();

					switch( proj_pool[ item.shoot ].aiStyle ) {
					case 15:    // Standard
					case 13:    // Chain Knife, Boxing Glove
					case 69:    // Flairon
					case 75:    // Solar Eruption
						return true;
					}
					return false;
				} );
			add_def( "Any Boomerang", null,
				( item, grps ) => {
					if( !item.melee || item.useStyle != 1 ) { return false; }
					if( item.type == ItemID.FlyingKnife ) { return true; }

					if( item.shoot == 0 ) { return false; }
					var proj_pool = this.GetProjPool();

					switch( proj_pool[ item.shoot ].aiStyle ) {
					case 3:    // Boomerangs
					case 15:    // Thorn Chakram
						return true;
					}
					return false;
				} );
			add_def( "Any Yoyo", null,
				( item, grps ) => {
					return ItemAttributeHelpers.IsYoyo( item );
				} );

			// Magic Sub Classes

			add_def( "Any Magic Staff Or Scepter Or Wand", null,
				( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Staff" ) ||
						name.Contains( "Scepter" ) ||
						name.Contains( "Wand" );
				} );
			add_def( "Any Magic Rod", null,
				( item, grps ) => {
					if( !item.magic ) { return false; }

					string name = ItemIdentityHelpers.GetQualifiedName( item );
					return name.Contains( "Rod" );
				} );
			add_def( "Any Magic Gun", null,
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
			add_def( "Any Magic Tome", null,
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

			add_def( "Any Ranger Gun", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Bullet ||
						item.useAmmo == AmmoID.CandyCorn ||
						item.useAmmo == AmmoID.Coin;
				} );
			add_def( "Any Ranger Bow", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Arrow ||
						item.useAmmo == AmmoID.Stake;
				} );
			add_def( "Any Ranger Launcher", null,
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					return item.useAmmo == AmmoID.Rocket ||
						item.useAmmo == AmmoID.StyngerBolt ||
						item.useAmmo == AmmoID.JackOLantern ||
						item.useAmmo == AmmoID.NailFriendly;
				} );

			// Summon Sub Classes

			add_def( "Any Minion Summon Item", null,
				( item, grps ) => {
					return item.summon && !item.sentry;
				} );
			add_def( "Any Sentry Summon Item", null,
				( item, grps ) => {
					return item.summon && item.sentry;
				} );
			
			// Vanity Classes
			add_def( "Any Vanity Accessory", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return item.accessory;
				} );
			add_def( "Any Vanity Garment", null,
				( item, grps ) => {
					if( !item.vanity ) { return false; }
					return !item.accessory;
				} );
		}


		private void DefineItemWeaponGroups2( Action<string, string[], Matcher> add_def ) {
			// Misc Sub Classes

			add_def( "Any Ranger Misc",
				new string[] { "Any Ranger Gun", "Any Ranger Bow", "Any Ranger Launcher" },
				( item, grps ) => {
					if( !item.ranged ) { return false; }
					bool ranger = grps["Any Ranger Gun"].Contains( item.type );
					bool bow = grps["Any Ranger Bow"].Contains( item.type );
					bool launcher = grps["Any Ranger Launcher"].Contains( item.type );
					return !ranger && !bow && !launcher;
				}
			);

			add_def( "Any Magic Misc",
				new string[] { "Any Magic Staff Or Scepter Or Wand", "Any Magic Rod", "Any Magic Gun", "Any Magic Tome" },
				( item, grps ) => {
					if( !item.magic ) { return false; }
					bool staff = grps["Any Magic Staff Or Scepter Or Wand"].Contains( item.type );
					bool rod = grps["Any Magic Rod"].Contains( item.type );
					bool gun = grps["Any Magic Gun"].Contains( item.type );
					bool tome = grps["Any Magic Tome"].Contains( item.type );
					return !staff && !rod && !gun && !tome;
				}
			);
		}
	}
}
