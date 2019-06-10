using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Recipes;
using System;
using System.Collections.Generic;
using Terraria.ID;

using Matcher = System.Func<Terraria.Item, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups.Defs {
	partial class EntityGroupDefs {
		internal static void DefineItemEquipmentGroups3( Action<string, string[], Matcher> addDef ) {
			// Equipment Tiers

			addDef( "Any Tiki Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					string name = ItemIdentityHelpers.GetQualifiedName( item );
					if( !hasEquip || !name.Contains( "Tiki" ) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Cactus Equipment",
				new string[] { "Any Equipment", },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Cactus }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				}
			);

			addDef( "Any Plain Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Wood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Boreal Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BorealWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Palm Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalmWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Rich Mahogany Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.RichMahogany }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Ebonwood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Ebonwood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shadewood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Shadewood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Pearlwood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Pearlwood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Dynasty Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DynastyWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Spooky Wood Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpookyWood }, 2 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Tin Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TinBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Copper Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CopperBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Iron Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.IronBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Lead Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LeadBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Silver Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SilverBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Tungsten Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TungstenBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Gold Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.GoldBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Platinum Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PlatinumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Meteor Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MeteoriteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Demonite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.DemoniteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Crimtane Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CrimtaneBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Jungle Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.JungleSpores }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Bee Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeeWax }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Bone Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.Bone }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Hellstone Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HellstoneBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Cobalt Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.CobaltBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Palladium Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.PalladiumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Mythril Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.MythrilBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Orichalcum Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.OrichalcumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Adamantite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AdamantiteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Titanium Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TitaniumBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Frost Core Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FrostCore }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Forbidden Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.AncientBattleArmorMaterial }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Hallow Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.HallowedBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Chlorophyte Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ChlorophyteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shroomite Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.ShroomiteBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Spectre Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.SpectreBar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Shell Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has1 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.BeetleShell }, 1 );
					var has2 = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.TurtleShell }, 1 );
					if( !hasEquip || (!has1 && !has2) ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );

			addDef( "Any Nebula Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentNebula }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Vortex Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentVortex }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Solar Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentSolar }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Stardust Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.FragmentStardust }, 1 );
					if( !hasEquip || !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
			addDef( "Any Luminite Ore Equipment",
				new string[] { "Any Equipment" },
				( item, grps ) => {
					bool hasEquip = RecipeHelpers.ItemHasIngredients( item.type, grps["Any Equipment"], 1 );
					var has = RecipeHelpers.ItemHasIngredients( item.type, new HashSet<int> { ItemID.LunarBar }, 1 );
					if( !has ) { return false; }
					return item.createTile == -1 && item.createWall == -1;
				} );
		}
	}
}
