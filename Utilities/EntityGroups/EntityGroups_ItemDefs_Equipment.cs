using HamstarHelpers.ItemHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Utilities.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemEquipmentGroups( IList<KeyValuePair<string, Func<Item, bool>>> matchers ) {
			void add_grp_def( string name, Func<Item, bool> matcher ) {
				matchers.Add( new KeyValuePair<string, Func<Item, bool>>( name, matcher ) );
			}

			add_grp_def( "Any Weapon", ( Item item ) => {
				return item.damage > 0;
			} );
			add_grp_def( "Any Tool", ( Item item ) => {
				return ItemAttributeHelpers.IsTool( item );
			} );

			add_grp_def( "Any Accessory", ( Item item ) => {
				return item.accessory && !item.vanity;
			} );
			add_grp_def( "Any Armor", ( Item item ) => {
				return ItemAttributeHelpers.IsArmor( item );
			} );
			add_grp_def( "Any Vanity", ( Item item ) => {
				return item.vanity;
			} );

			// Weapon Classes

			add_grp_def( "Any Ranged Weapon", ( Item item ) => {
				return item.ranged;
			} );
			add_grp_def( "Any Magic Weapon", ( Item item ) => {
				return item.magic;
			} );
			add_grp_def( "Any Melee Weapon", ( Item item ) => {
				return item.melee;
			} );
			add_grp_def( "Any Thrown Weapon", ( Item item ) => {
				return item.thrown;
			} );

			// Equipment Tiers

			add_grp_def( "Any Wood Equipment", ( Item item ) => {
				foreach( Recipe recipe in Main.recipe ) {
					if( recipe.createItem.type != item.type ) { continue; }
					
					foreach( Item reqitem in recipe.requiredItem ) {
						if( reqitem.stack == 1 ) { continue; }

						switch( reqitem.type ) {
						case ItemID.Wood:
						case ItemID.BorealWood:
						case ItemID.DynastyWood:
						case ItemID.PalmWood:
						case ItemID.SpookyWood:
						case ItemID.Ebonwood:
						case ItemID.Shadewood:
						case ItemID.RichMahogany:
						case ItemID.Pearlwood:
						}
					}
				}
			} );

			add_grp_def( "Any Tin Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Copper Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Iron Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Lead Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Silver Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Tungsten Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Gold Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Platinum Equipment", ( Item item ) => {
			} );

			add_grp_def( "Any Meteor Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Demonite Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Crimtane Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Jungle Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Bone Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Hellstone Equipment", ( Item item ) => {
			} );

			add_grp_def( "Any Cobalt Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Palladium Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Mythril Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Orichalcum Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Adamantite Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Titanium Equipment", ( Item item ) => {
			} );

			add_grp_def( "Any Frost Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Hallow Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Chlorophyte Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Shroomite Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Spectre Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Shell Equipment", ( Item item ) => {
			} );

			add_grp_def( "Any Nebula Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Vortex Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Solar Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Stardust Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Luminite Ore Equipment", ( Item item ) => {
			} );

			add_grp_def( "Any Wood Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Copper Or Tin Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Iron Or Lead Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Silver Or Tungsten Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Gold Or Platinum Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Demonite Or Crimtane Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Meteor Or Jungle Or Bone Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Cobalt Or Palladium Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Mythril Or Orichalcum Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Adamantite Or Titanium Ore Equipment", ( Item item ) => {
			} );
			add_grp_def( "Any Shroomite Or Spectre Ore Equipment", ( Item item ) => {
			} );

			// Weapon Sub Classes

			add_grp_def( "Any Swingable", ( Item item ) => {
			} );
			add_grp_def( "Any Thrustable", ( Item item ) => {
			} );
			add_grp_def( "Any Flail", ( Item item ) => {
			} );
			add_grp_def( "Any Boomerang", ( Item item ) => {
			} );
			add_grp_def( "Any Yoyo", ( Item item ) => {
			} );
			
			add_grp_def( "Any Magic Rod", ( Item item ) => {
			} );
			add_grp_def( "Any Magic Tome", ( Item item ) => {
			} );
			add_grp_def( "Any Yoyo", ( Item item ) => {
			} );

			// Accessory Classes

		}
	}
}
