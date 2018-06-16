using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemWeaponGroups1( Action<string, Func<Item, bool>> add_def ) {
			// Weapon Classes

			add_def( "Any Ranged Weapon", ( Item item ) => {
				return item.ranged;
			} );
			add_def( "Any Magic Weapon", ( Item item ) => {
				return item.magic;
			} );
			add_def( "Any Melee Weapon", ( Item item ) => {
				return item.melee;
			} );
			add_def( "Any Thrown Weapon", ( Item item ) => {
				return item.thrown;
			} );

			// Melee Sub Classes

			add_def( "Any Swingable", ( Item item ) => {
				return item.melee && item.useStyle == 1;
			} );
			add_def( "Any Thrustable", ( Item item ) => {
				return item.melee && item.useStyle == 5;
			} );
			add_def( "Any Flail", ( Item item ) => {
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
			add_def( "Any Boomerang", ( Item item ) => {
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
			add_def( "Any Yoyo", ( Item item ) => {
				return ItemAttributeHelpers.IsYoyo( item );
			} );

			// Magic Sub Classes

			add_def( "Any Magic Staff Or Scepter Or Wand", ( Item item ) => {
				if( !item.magic ) { return false; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Staff" ) ||
					name.Contains( "Scepter" ) ||
					name.Contains( "Wand" );
			} );
			add_def( "Any Magic Rod", ( Item item ) => {
				if( !item.magic ) { return false; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Rod" );
			} );
			add_def( "Any Magic Gun", ( Item item ) => {
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
			add_def( "Any Magic Tome", ( Item item ) => {
				if( !item.magic ) { return false; }

				switch( item.type ) {
				case ItemID.DemonScythe:
				case ItemID.WaterBolt:
				case ItemID.LunarFlareBook:
				case ItemID.MagnetSphere:
				case ItemID.RazorbladeTyphoon:
					return true;
				}

				var has = RecipeHelpers.RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpellTome }, 1 );
				if( has ) { return true; }

				string name = ItemIdentityHelpers.GetQualifiedName( item );
				return name.Contains( "Book" ) ||
					name.Contains( "Tome" );
			} );

			// Ranged Sub Classes

			add_def( "Any Ranger Gun", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Bullet ||
					item.useAmmo == AmmoID.CandyCorn ||
					item.useAmmo == AmmoID.Coin;
			} );
			add_def( "Any Ranger Bow", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Arrow ||
					item.useAmmo == AmmoID.Stake;
			} );
			add_def( "Any Ranger Launcher", ( Item item ) => {
				if( !item.ranged ) { return false; }
				return item.useAmmo == AmmoID.Rocket ||
					item.useAmmo == AmmoID.StyngerBolt ||
					item.useAmmo == AmmoID.JackOLantern ||
					item.useAmmo == AmmoID.NailFriendly;
			} );

			// Summon Sub Classes

			add_def( "Any Minion Summon Item", ( Item item ) => {
				return item.summon && !item.sentry;
			} );
			add_def( "Any Sentry Summon Item", ( Item item ) => {
				return item.summon && item.sentry;
			} );
			
			// Vanity Classes
			add_def( "Any Vanity Accessory", ( Item item ) => {
				if( !item.vanity ) { return false; }
				return item.accessory;
			} );
			add_def( "Any Vanity Garment", ( Item item ) => {
				if( !item.vanity ) { return false; }
				return !item.accessory;
			} );
		}


		private void DefineItemWeaponGroups2( Action<string, Func<Item, bool>> add_def ) {
			// Misc Sub Classes

			add_def( "Any Ranger Misc", ( Item item ) => {
				if( !item.ranged ) { return false; }
				bool ranger = EntityGroups.ItemGroups["Any Ranger Gun"].Contains( item.type );
				bool bow = EntityGroups.ItemGroups["Any Ranger Bow"].Contains( item.type );
				bool launcher = EntityGroups.ItemGroups["Any Ranger Launcher"].Contains( item.type );
				return !ranger && !bow && !launcher;
			} );

			add_def( "Any Magic Misc", ( Item item ) => {
				if( !item.magic ) { return false; }
				bool staff = EntityGroups.ItemGroups["Any Magic Staff Or Scepter Or Wand"].Contains( item.type );
				bool rod = EntityGroups.ItemGroups["Any Magic Rod"].Contains( item.type );
				bool gun = EntityGroups.ItemGroups["Any Magic Gun"].Contains( item.type );
				bool tome = EntityGroups.ItemGroups["Any Magic Tome"].Contains( item.type );
				return !staff && !rod && !gun && !tome;
			} );
		}
	}
}
