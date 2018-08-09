using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineItemEquipmentGroups1( Action<string, Func<Item, bool>> add_def ) {
			add_def( "Any Weapon", ( Item item ) => {
				return item.damage > 0;
			} );
			add_def( "Any Tool", ( Item item ) => {
				return ItemAttributeHelpers.IsTool( item );
			} );
			add_def( "Any Vanilla Explosive", ( Item item ) => {
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

			add_def( "Any Accessory", ( Item item ) => {
				return item.accessory && !item.vanity;
			} );
			add_def( "Any Armor", ( Item item ) => {
				return ItemAttributeHelpers.IsArmor( item );
			} );
			add_def( "Any Vanity", ( Item item ) => {
				return item.vanity;
			} );
			add_def( "Any Potion", ( Item item ) => {
				return item.potion;
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


		private void DefineItemEquipmentGroups2( Action<string, Func<Item, bool>> add_def ) {
			// Equipment Tiers

			add_def( "Any Tiki Equipment", ( Item item ) => {
				string name = ItemIdentityHelpers.GetQualifiedName( item );
				if( !name.Contains( "Tiki" ) ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Plain Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Wood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Boreal Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BorealWood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Palm Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PalmWood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Rich Mahogany Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.RichMahogany }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Ebonwood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Ebonwood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Shadewood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Shadewood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Pearlwood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Pearlwood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Dynasty Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.DynastyWood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Spooky Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpookyWood }, 2 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Tin Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TinBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Copper Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CopperBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Iron Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.IronBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Lead Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.LeadBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Silver Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SilverBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Tungsten Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TungstenBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Gold Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.GoldBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Platinum Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PlatinumBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Meteor Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.MeteoriteBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Demonite Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.DemoniteBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Crimtane Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CrimtaneBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Jungle Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.JungleSpores }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Bee Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BeeWax }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Bone Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.Bone }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Hellstone Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.HellstoneBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Cobalt Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.CobaltBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Palladium Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.PalladiumBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Mythril Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.MythrilBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Orichalcum Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.OrichalcumBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Adamantite Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.AdamantiteBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Titanium Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TitaniumBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Frost Core Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FrostCore }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Forbidden Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.AncientBattleArmorMaterial }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Hallow Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.HallowedBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Chlorophyte Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.ChlorophyteBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Shroomite Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.ShroomiteBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Spectre Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.SpectreBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Shell Equipment", ( Item item ) => {
				var has1 = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.BeetleShell }, 1 );
				var has2 = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.TurtleShell }, 1 );
				if( !has1 && !has2 ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );

			add_def( "Any Nebula Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentNebula }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Vortex Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentVortex }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Solar Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentSolar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Stardust Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.FragmentStardust }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Luminite Ore Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, new HashSet<int> { ItemID.LunarBar }, 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
		}


		private void DefineItemEquipmentGroups3( Action<string, Func<Item, bool>> add_def ) {
			add_def( "Any Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Weapon"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Tool"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Accessory"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Armor"].Contains( item.type );
			} );

			add_def( "Any Wood Equipment", ( Item item ) => {
				var has = RecipeHelpers.ItemHasIngredients( item, EntityGroups.ItemGroups["Any Wood"], 1 );
				if( !has ) { return false; }
				return item.createTile == -1 && item.createWall == -1;
			} );
			add_def( "Any Copper Or Tin Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Copper Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Tin Equipment"].Contains( item.type );
			} );
			add_def( "Any Iron Or Lead Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Iron Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Lead Equipment"].Contains( item.type );
			} );
			add_def( "Any Silver Or Tungsten Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Silver Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Tungsten Equipment"].Contains( item.type );
			} );
			add_def( "Any Gold Or Platinum Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Gold Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Platinum Equipment"].Contains( item.type );
			} );
			add_def( "Any Demonite Or Crimtane Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Demonite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Crimtane Equipment"].Contains( item.type );
			} );
			add_def( "Any Meteor Or Jungle Or Bone Or Bee Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Meteor Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Jungle Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Bone Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Bee Equipment"].Contains( item.type );
			} );
			add_def( "Any Cobalt Or Palladium Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Cobalt Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Palladium Equipment"].Contains( item.type );
			} );
			add_def( "Any Mythril Or Orichalcum Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Mythril Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Orichalcum Equipment"].Contains( item.type );
			} );
			add_def( "Any Adamantite Or Titanium Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Adamantite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Titanium Equipment"].Contains( item.type );
			} );
			add_def( "Any Frost Core Or Forbidden Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Frost Core Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Forbidden Equipment"].Contains( item.type );
			} );
			add_def( "Any Chlorophyte Or Shroomite Or Spectre Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Chlorophyte Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Shroomite Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Spectre Equipment"].Contains( item.type );
			} );
			add_def( "Any Celestial Equipment", ( Item item ) => {
				return EntityGroups.ItemGroups["Any Nebula Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Vortex Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Solar Equipment"].Contains( item.type ) ||
					EntityGroups.ItemGroups["Any Stardust Equipment"].Contains( item.type );
			} );
		}
	}
}
